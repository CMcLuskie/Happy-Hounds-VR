using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEyes : Dog {

    public DogBrain dogBrainScript;

    public GameObject bodyCam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Toy")
        {
            dogBrainScript.toySeen = true;
            dogBrainScript.ToyPos(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Toy")
        { 

            dogBrainScript.ToyPos(other.gameObject);
         dogBrainScript.toySeen = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Toy")
        {
            StartCoroutine(BodyCamTimer());
        }
    }

    IEnumerator BodyCamTimer()
    {
        bodyCam.SetActive(true);
        yield return new WaitForSeconds(3);
        bodyCam.SetActive(false);
    }
}
