using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftControl : Controllers {

    public delegate void OnControllerInput(Vector3 playerPos);
    public static event OnControllerInput DogCall;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (TriggerDown())
        {
            Debug.Log("second hand trig");
            DogCall(GetPlayerPos());
        }

        if (TouchpadPressDown())
            Debug.Log("touchpad press left");
    }
}
