﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEyes : MonoBehaviour
{

    public DogBrain dogBrainScript;

    public GameObject tempCam;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Toy") && !(dogBrainScript.toyCaught) && !(dogBrainScript.closeToPlayer))
        {
            dogBrainScript.toySeen = true;
            dogBrainScript.toy = other.gameObject;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.tag == "Toy") && !(dogBrainScript.toyCaught) && !(dogBrainScript.closeToPlayer))
        {             
            dogBrainScript.toySeen = true;
            dogBrainScript.toy = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Toy") && !(gameObject.name == "tempCam"))
        {
                StartCoroutine(BodyCamTimer());
        }
    }

    IEnumerator BodyCamTimer()
    {
        tempCam.SetActive(true);
        yield return new WaitForSeconds(3);
        tempCam.SetActive(false);
       
    }
}
