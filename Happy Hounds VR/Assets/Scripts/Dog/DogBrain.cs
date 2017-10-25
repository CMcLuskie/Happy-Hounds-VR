using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBrain : Dog {

    public float wanderTimer;
	// Use this for initialization
	void Start ()
    {
        GetGridObject();
        

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            StartDogLerp();

        wanderTimer += Time.deltaTime;
        if ((wanderTimer >= 5 || wanderTimer <= 6f) && isLerping == false)
        {
            wanderTimer = 0;
            StartDogLerp();
        }
	}

    
}
