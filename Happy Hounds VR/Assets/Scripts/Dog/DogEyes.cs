using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEyes : Dog {

    public DogBrain dogBrainScript;

<<<<<<< HEAD
    public GameObject tempCam;
=======
    public GameObject bodyCam;
>>>>>>> 8962adc0a80569f3d2c26d8d514ceef1b0afa548

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
<<<<<<< HEAD
        if((other.tag == "Toy") && !(gameObject.name == "tempCam"))
        { 
=======
        if(other.tag == "Toy")
        {
>>>>>>> 8962adc0a80569f3d2c26d8d514ceef1b0afa548
            StartCoroutine(BodyCamTimer());
        }
    }

    IEnumerator BodyCamTimer()
    {
<<<<<<< HEAD
        tempCam.SetActive(true);
        yield return new WaitForSeconds(3);
        tempCam.SetActive(false);
=======
        bodyCam.SetActive(true);
        yield return new WaitForSeconds(3);
        bodyCam.SetActive(false);
>>>>>>> 8962adc0a80569f3d2c26d8d514ceef1b0afa548
    }
}
