// Created by J Ricardo Sanchez Ibanez
// jricardosan.tech@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopMgr : MonoBehaviour
{

    [SerializeField] private uint loopRadius = 3;
    private float characterFootprint = 1.0f;
    private float nCubes;
    // Angle of separation between cubes wrt the loop center
    private float angleCubeSeparation_rad;
    // Distance from bottom of the cube to the loop center
    private float cubeBottomToLoopCenter;
    private float cubeToLoopCenter;

    private int rectifiedNCubes;
    // To count the blocks destroyed by the character
    private int destroyedBlocksCounter = 0;

    private GameObject loopBase;
    private GameObject[] cookieBlocks;

    public GameObject cookieBlock;
    private float rightLimitAngle_deg;
    private float leftLimitAngle_deg;

    [SerializeField] private float moveSpeed = 6.0f;

    public bool isBlocked = false;

    public GameObject cookieBasePrefab;

    // Start is called before the first frame update
    void Start()
    {

        // Initialization of transform
        createLoopBase();
        createCookieBlocks();
        transform.position = new Vector3(0.0f,-cubeToLoopCenter,0.0f);
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!isBlocked)
        {
            rotateCookie();
        }
        
    }


    void rotateCookie()
    {
        float xAngle = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed * angleCubeSeparation_rad * Mathf.Rad2Deg;
        float previousZangle = transform.eulerAngles[2];
        transform.Rotate(0.0f, 0.0f, xAngle, Space.Self);

        //Debug.Log("Tentative Rotation = " + transform.eulerAngles[2]);

        if ((transform.eulerAngles[2] > rightLimitAngle_deg) && (transform.eulerAngles[2] < leftLimitAngle_deg))
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles[0],
                                            transform.eulerAngles[1],
                                            previousZangle);
        }

        //Debug.Log("Actual Rotation = " + transform.eulerAngles[2]);


    }




    void createLoopBase()
    {

        loopBase =  Instantiate(cookieBasePrefab, transform.position, transform.rotation);
        loopBase.transform.position = Vector3.zero;
        loopBase.transform.eulerAngles = new Vector3(0.0f,0.0f,0.0f);
        loopBase.transform.localScale = new Vector3(100.0f * (float)loopRadius, 100.0f * (float)loopRadius, 50f * characterFootprint);
        loopBase.transform.parent = transform;

    }

    void createCookieBlocks()
    {
        nCubes = Mathf.PI / ( Mathf.Asin( 0.5f * characterFootprint / (float)loopRadius ) );
        angleCubeSeparation_rad = 2*Mathf.PI / nCubes;
        rectifiedNCubes = Mathf.FloorToInt( nCubes ) - 1;
        cubeBottomToLoopCenter = (float)loopRadius * Mathf.Cos( angleCubeSeparation_rad / 2 );
        cubeToLoopCenter = cubeBottomToLoopCenter + characterFootprint * 0.5f;

        Debug.Log("Number of cubes is " + nCubes);
        Debug.Log("Rectified number of cubes is " + rectifiedNCubes);
        Debug.Log("Angle of separation is " + angleCubeSeparation_rad * Mathf.Rad2Deg);

        cookieBlocks = new GameObject[rectifiedNCubes];

        float cubeRotation_rad;
        float angleRemainder = 2*Mathf.PI - angleCubeSeparation_rad * (float)(rectifiedNCubes+1);

        for (int idx = 0; idx < rectifiedNCubes; idx++)
        {
            cubeRotation_rad = (float)idx * angleCubeSeparation_rad + 0.5f*angleRemainder + angleCubeSeparation_rad;
            cookieBlocks[idx] = Instantiate(cookieBlock, new Vector3(0, 0, 0), Quaternion.identity);
            cookieBlocks[idx].GetComponent<CookieBlockMgr>().setPosition(cubeRotation_rad, cubeToLoopCenter);
            cookieBlocks[idx].transform.parent = transform;

            //GameObject.CreatePrimitive(PrimitiveType.Cube); 
            
        }

        rightLimitAngle_deg = ( 0.5f*angleRemainder + 
                                0.1f*angleCubeSeparation_rad) * Mathf.Rad2Deg;
        leftLimitAngle_deg = 360.0f - ( 0.5f*angleRemainder + 
                                        0.1f*angleCubeSeparation_rad) * Mathf.Rad2Deg;

        Debug.Log("Right limit = " + rightLimitAngle_deg);
        Debug.Log("Left limit = " + leftLimitAngle_deg);
        

    }

    public void UpdateLimitAngles( float destroyedBlockAngle_deg )
    {

        destroyedBlocksCounter++;
        if (destroyedBlocksCounter == rectifiedNCubes)
        {
            leftLimitAngle_deg = -1.0f;
            rightLimitAngle_deg = 361.0f;
            return;
        }

        Debug.Log( "Input destroyed block angle is " + destroyedBlockAngle_deg);

        if( Mathf.Abs(leftLimitAngle_deg - destroyedBlockAngle_deg) < Mathf.Abs(rightLimitAngle_deg - destroyedBlockAngle_deg) )
        {
            leftLimitAngle_deg -= angleCubeSeparation_rad * Mathf.Rad2Deg;
            Debug.Log("Left limit updated to " + leftLimitAngle_deg);
        }
        else
        {
            rightLimitAngle_deg += angleCubeSeparation_rad * Mathf.Rad2Deg;
            Debug.Log("Right limit updated to " + rightLimitAngle_deg);
        }

    }

    public float GetAngleCubeSeparation()
    {
        return angleCubeSeparation_rad * Mathf.Rad2Deg;
    }

    // Prevents the character from moving when it dies
    public void BlockMotion()
    {
        isBlocked = true;
    }

}
