using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMouth : MonoBehaviour {

    public DogBrain dogBrainScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Toy")
        {
            other.transform.SetParent(this.transform);
            other.transform.position = new Vector3(0, 0, 0);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Toy")
        {

            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Toy"))
        {
            
        }
    }
}
