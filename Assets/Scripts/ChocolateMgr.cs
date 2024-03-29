/* Chocolate Chip Manager Class
 *
 * Created by J Ricardo San
 * GitHub: @JRicardoSan
 * E-mail address: jricardosan.tech@gmail.com
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocolateMgr : MonoBehaviour
{
    /************************* PUBLIC PARAMETERS *****************************/
    /* Layer of the Main Character */
    public LayerMask characterLayer;

    /************************* PRIVATE PARAMETERS ****************************/
    /* Separation between cubes in angle with respect to loop base centre */
    private float angleCubeSeparation_deg;

    /************************* PUBLIC FUNCTIONS ******************************/
    /* None */

    /************************* PRIVATE FUNCTIONS *****************************/
    /* Start() is called before the first frame update */
    void Start()
    {
        /* This angle depends on the number of cubes defined in loop base */
        angleCubeSeparation_deg = 
        transform.parent.GetComponent<LoopMgr>().GetAngleCubeSeparation();
    }

    /* Update() is called once per frame */
    void Update()
    {
        /* If the chocolate is close enough to the character, it is eaten
         * Here is determined by checking if its angle transform is close
         * enough to 0 degrees, as it happens when it is at the centre of
         * the game screen */
        if (( transform.eulerAngles[2] < 0.3f*angleCubeSeparation_deg) || 
            (360.0f-transform.eulerAngles[2] < 0.3f*angleCubeSeparation_deg))
        {
            GetEaten();
        }
    }

    /* GetEaten() is called when the main character is close enough to the 
     * chocolate chip */
    void GetEaten()
    {
        /* Make this chocolate chip disappear */
        Debug.Log("Acquired chocolate!");
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        this.enabled = false;
    }
}