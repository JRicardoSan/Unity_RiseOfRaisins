/* Cookie Block Manager Class
 *
 * Created by J Ricardo San
 * GitHub: @JRicardoSan
 * E-mail address: jricardosan.tech@gmail.com
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieBlockMgr : MonoBehaviour
{
    /************************* PUBLIC PARAMETERS *****************************/
    /* The maximum hits the cookie can receive before breaking */
    /* TODO: Make this magic number configurable somehow */
    public int maxIntegrity = 3;
    /* The number of hits remaining before it breaks */
    public int currentIntegrity;
    /* The prefab of the raisins */
    public GameObject raisinPrefab;
    /* The prefab of the chocolate chips */
    public GameObject chocolatePrefab;

    /************************* PRIVATE PARAMETERS ****************************/
    /* Here will go the products resulting from the block breaking */
    private GameObject[] cookieContents;
    /* The angle at which the block is placed wrt the loop base centre */
    private float angleInLoop_rad;
    /* Distance between the block and the loop base centres */
    private float distanceToLoopCentre;

    /************************* PUBLIC FUNCTIONS ******************************/
    /* setPose() serves to place the block with respect to the loop base centre 
     * Input:
     * - angle_rad: angle at which the block is positioned
     * - radius_m: radius of the loop base */
    public void SetPose( float angle_rad,
                         float radius_m )
    {
        /* Position the block taking into account that PI/2 must be added to 
         * consider the zero at upside direction (north) */
        transform.position = new Vector3( 
            radius_m * Mathf.Cos( angle_rad + Mathf.PI *0.5f ),
            radius_m * Mathf.Sin( angle_rad + Mathf.PI *0.5f ),
            0.0f );

        /* Set its orientation */
        transform.eulerAngles = new Vector3( 0.0f,
                                             0.0f,
                                             angle_rad*Mathf.Rad2Deg);

        /* Set the base loop angle and the distance to base loop centre */
        angleInLoop_rad = angle_rad;
        distanceToLoopCentre = radius_m;
    }

    /* TakeDamage() decreases the integrity score and evaluates if it is needed
     * to destroy the block
     * Input:
     *  - characterFacingRight: indicates whether the Main Character is facing
     *    the right side of the screen or not */
    public void TakeDamage( bool characterFacingRight )
    {
        /* Decrease the integrity score */
        currentIntegrity--;
        Debug.Log("Cookie Block gets damaged");

        /* Evaluates if block must be destroyed or not */
        if(currentIntegrity <= 0)
        {
            GetDestroyed( characterFacingRight );
        }        
    }

    /************************* PRIVATE FUNCTIONS *****************************/
    /* Start() is called before the first frame update */
    void Start()
    {
        /* The current integrity score is set as the maximum */
        currentIntegrity = maxIntegrity;        
    }

    /* GetDestroyed() serves to disable the cookie block and its children
     * Input:
     *  - characterFacingRight: indicates whether the Main Character is facing
     *    the right side of the screen or not */
    void GetDestroyed( bool characterFacingRight )
    {
        Debug.Log("Cookie Block destroyed");
        /* The destruction of the cookie block entails bringing either
         * chocolate chips or raisins */
        CreateContents( characterFacingRight );
        /* Update the Loop Manager so the angle range the character can walk
         * into increases */
        transform.parent.GetComponent<LoopMgr>().UpdateLimitAngles( 360.0f - 
            transform.eulerAngles[2] );
        /* Disable this and its children */
        for( int idx = 0; idx < transform.childCount; idx++ )
        {
            transform.GetChild(idx).gameObject.SetActive(false);
        }
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        this.enabled = false;
    }


    /* CreateContents() will create either raisins or chocolate chips when
     * the cookie block gets destroyed
     * Input:
     *  - characterFacingRight: indicates whether the Main Character is facing
     *    the right side of the screen or not */
    void CreateContents( bool characterFacingRight )
    {
        /* Create new Game Object to store either chocolate or raisins */
        cookieContents = new GameObject[1];
        
        /* 50% of having a raisin, 50% a chocolate chip */
        if(Random.value<0.5f)
        {
            /* Great! We get a chocolate chip :) */
            cookieContents[0] = Instantiate(chocolatePrefab,
                                            transform.position,
                                            transform.rotation);
            cookieContents[0].transform.parent = transform.parent;
        }
        else
        {
            /* Oh no! We get a raisin :( */
            cookieContents[0] = Instantiate(raisinPrefab,
                                            transform.position, 
                                            transform.rotation);
            cookieContents[0].transform.parent = transform.parent; 

            /* Update position of the raisin */
            cookieContents[0].GetComponent<RaisinMgr>().SetLoopPositionInfo(
                distanceToLoopCentre-0.25f,
                angleInLoop_rad);
                
            /* Raisin will start moving towards the main character */
            cookieContents[0].GetComponent<RaisinMgr>().SetDirection( 
                characterFacingRight );
        }            
    }
}