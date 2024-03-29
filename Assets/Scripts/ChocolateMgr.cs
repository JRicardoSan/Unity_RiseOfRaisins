using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocolateMgr : MonoBehaviour
{
    public LayerMask characterLayer;
    private float angleCubeSeparation_deg;


    void Start()
    {
        angleCubeSeparation_deg = transform.parent.GetComponent<LoopMgr>().GetAngleCubeSeparation();
    }

    void Update()
    {


        if (( transform.eulerAngles[2] < 0.3f*angleCubeSeparation_deg) || (360.0f-transform.eulerAngles[2] < 0.3f*angleCubeSeparation_deg))
        {
            GetEaten();
        }

    }

    void GetEaten()
    {
        Debug.Log("Acquired chocolate!");
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        this.enabled = false;
    }



}
