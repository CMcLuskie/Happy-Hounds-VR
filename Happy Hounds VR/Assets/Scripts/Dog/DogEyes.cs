using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEyes : MonoBehaviour
{

    public DogBrain dogBrainScript;

    public GameObject tempCam;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Toy"))
        {
            dogBrainScript.toy = other.gameObject;
            dogBrainScript.toySeen = true;
        }
    }

    
    private void OnTriggerStay(Collider other)
    {
        if ((other.tag == "Toy"))
        {
            dogBrainScript.toy = other.gameObject;
            dogBrainScript.toySeen = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Toy"))
        {
            if (!(gameObject.name == "tempCam"))
                StartCoroutine(BodyCamTimer());
            else
                dogBrainScript.toySeen = false;
        }
    }

    IEnumerator BodyCamTimer()
    {
        tempCam.SetActive(true);
        yield return new WaitForSeconds(3);
        tempCam.SetActive(false);
       
    }
}
