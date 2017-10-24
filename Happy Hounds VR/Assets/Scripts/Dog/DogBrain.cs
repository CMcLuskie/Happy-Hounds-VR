using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBrain : Dog {

	// Use this for initialization
	void Start () {
        GetGrid();
        SetFoodType();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            StartDogLerp(); 
    }
}
