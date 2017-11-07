using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogSenses : MonoBehaviour {

    public DogBrain dogBrainScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Toy")
            dogBrainScript.closeToToy = true;
        else if (other.tag == "Player")
            dogBrainScript.closeToPlayer = true;
    }

    private void OnTriggerStay(Collider other)
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Toy")
            dogBrainScript.closeToToy = false;
        else if (other.tag == "Player")
            dogBrainScript.closeToPlayer = false;

    }
}
