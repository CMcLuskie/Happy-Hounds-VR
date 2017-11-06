using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEyes : Dog {

    public DogBrain dogBrainScript;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

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
}
