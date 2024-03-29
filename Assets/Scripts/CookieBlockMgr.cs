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
    /* PUBLIC PARAMETERS */
    /* The maximum hits the cookie can receive before breaking */
    /* TODO: Make this magic number configurable somehow */
    public int maxIntegrity = 3;
    /* The number of hits remaining before it breaks */
    public int currentIntegrity;
    /* The prefab of the raisins */
    public GameObject raisinPrefab;
    /* The prefab of the chocolate chips */
    public GameObject chocolatePrefab;

    /* PRIVATE PARAMETERS */
    private GameObject[] cookieContents;
    private float angleInLoop;
    private float distanceToLoopCenter;
    

    /* Start() is called before the first frame update */
    void Start()
    {
        // Measure of the block structural integrity
        currentIntegrity = maxIntegrity;        
    }

    /* CreateContents() will create either raisins or chocolate chips when
     * the cookie block gets destroyed */
    void CreateContents( bool characterFacingRight )
    {
        cookieContents = new GameObject[1];
        // Randomnly could be a chocolate chip or a raisin
        
        if(Random.value<0.5f)
        {
            cookieContents[0] = Instantiate(chocolatePrefab,
                                            transform.position,
                                            transform.rotation);
            cookieContents[0].transform.parent = transform.parent;
        }
        else
        {
            cookieContents[0] = Instantiate(raisinPrefab, transform.position, transform.rotation);
            cookieContents[0].transform.parent = transform.parent; 
            cookieContents[0].GetComponent<RaisinMgr>().SetLoopPositionInfo(distanceToLoopCenter-0.25f, angleInLoop);
            cookieContents[0].GetComponent<RaisinMgr>().SetDirection( characterFacingRight );
        }     
        
    }

    // The block decreases the integrity score and evaluates if it needs to be destroyed
    public void TakeDamage( bool characterFacingRight )
    {
        currentIntegrity--;
        Debug.Log("Cookie Block gets damaged");

        if(currentIntegrity <= 0)
        {
            GetDestroyed( characterFacingRight );
        }        
    }

    // Procedure to disabl block and its children
    void GetDestroyed( bool characterFacingRight )
    {
        Debug.Log("Cookie Block destroyed");
        CreateContents( characterFacingRight );
        // Tell the Loop Manager to allow the character to move where the block was
        transform.parent.GetComponent<LoopMgr>().UpdateLimitAngles( 360.0f - transform.eulerAngles[2] );
        for( int idx = 0; idx < transform.childCount; idx++ )
        {
            transform.GetChild(idx).gameObject.SetActive(false);
        }
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        this.enabled = false;
    }

    // Serves to position the block with respect to the cookie
    public void setPosition(float angle_rad, float radius)
    {
        // PI/2 is added to make the zero at north
        transform.position = new Vector3( radius * Mathf.Cos( angle_rad + Mathf.PI *0.5f ),
                                          radius * Mathf.Sin( angle_rad + Mathf.PI *0.5f ),
                                          0.0f );
        transform.eulerAngles = new Vector3(0.0f,0.0f,angle_rad*Mathf.Rad2Deg);
        angleInLoop = angle_rad;
        distanceToLoopCenter = radius;
    }
}
