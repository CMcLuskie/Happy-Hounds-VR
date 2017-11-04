using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEyes : Dog {

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
            toySeen = true;
        }
    }
}
