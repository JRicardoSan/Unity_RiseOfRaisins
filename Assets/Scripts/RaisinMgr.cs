using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaisinMgr : MonoBehaviour
{

    // PRIVATE
    // Distance from the raisin to the cookie center
    private float distanceToLoopCenter;
    // Angle with respect to the axis perpendicular to the cookie and
    // at its center
    private float angleInLoop;
    // Angle the raisin will intend to reach, where the character is located
    private float targetAngleInLoop;
    // -1 go left, 1 go right (from player's point of view)
    private int direction;
    // Separation between cubes
    private float angleCubeSeparation_deg;

    // PUBLIC
    // Layer for the Main Character, must be set inside Unity
    public LayerMask characterLayer;
    // Game Object corresponding to the Main Character, must be set inside Unity
    public GameObject mainCharacter;

    // Start is called before the first frame update
    void Start()
    {
        // The angle of separation between cubes, which comes as a function of the cookie radius
        // Serves later to make the speed of the raisin independent from the cookie radius
        angleCubeSeparation_deg = transform.parent.GetComponent<LoopMgr>().GetAngleCubeSeparation();
    }

    // Update is called once per frame
    void Update()
    {

        targetAngleInLoop = transform.parent.eulerAngles[2];
        
        transform.RotateAround(transform.parent.position, Vector3.forward, direction * 1.0f * angleCubeSeparation_deg * Time.deltaTime);

        if (( transform.eulerAngles[2] < 0.3f*angleCubeSeparation_deg) || (360.0f-transform.eulerAngles[2] < 0.3f*angleCubeSeparation_deg))
        {
            mainCharacter = GameObject.Find("MinerMixamoRigModel");
            mainCharacter.GetComponent<MC_Mgr>().Die();
        }

    }

    // Allows the cube, just before disappearing, to place the raisin at the correct position
    public void SetLoopPositionInfo(float radius, float angle_rad)
    {
        distanceToLoopCenter = radius;
        angleInLoop = angle_rad;
        transform.localPosition= new Vector3( distanceToLoopCenter * Mathf.Cos( angleInLoop + Mathf.PI *0.5f  ),
                                              distanceToLoopCenter * Mathf.Sin( angleInLoop + Mathf.PI *0.5f  ),
                                              0.0f );
    }

    // Makes the raisin go left or right according to player position when it removes a cookie block
    public void SetDirection( bool goToRight )
    {

        if (goToRight)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

    }

    // Inmediately disappears when receives a hit with the pickaxe
    public void TakeDamage()
    {
        GetDestroyed();
    }

    // Procedure to disable raisin and its child meshes
    private void GetDestroyed()
    {

        Debug.Log("Raisin destroyed");
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        this.enabled = false;
        for( int idx = 0; idx < transform.childCount; idx++ )
        {
            transform.GetChild(idx).gameObject.SetActive(false);
        }

    }

}
