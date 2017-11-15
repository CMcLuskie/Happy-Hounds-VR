using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogSenses : MonoBehaviour {

    public DogBrain dogBrainScript;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Toy") && !(dogBrainScript.toyCaught))
            dogBrainScript.CloseToSomething(DogBrain.Seekable.Toy);
        else if (other.tag == "Player")
            dogBrainScript.CloseToSomething(DogBrain.Seekable.Player);
    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.tag == "Toy") && !(dogBrainScript.toyCaught))
            dogBrainScript.CloseToSomething(DogBrain.Seekable.Toy);
        else if (other.tag == "Player")
            dogBrainScript.CloseToSomething(DogBrain.Seekable.Player);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Toy")
            dogBrainScript.toySeen = false;
        else if (other.tag == "Player")
            dogBrainScript.closeToPlayer = false;
    }
}
