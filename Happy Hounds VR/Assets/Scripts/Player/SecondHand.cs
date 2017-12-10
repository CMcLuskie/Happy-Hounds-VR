using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondHand : Controllers {

    public delegate void OnControllerInput(Vector3 playerPos);
    public static event OnControllerInput DogCall;
    public static event OnControllerInput DogSit;

    public DogBrain dogBrainScript;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Grab
        if (TriggerDown())
        {

        }

        //if (playerStatsScript.pickedUpTablet)
        //    animator.SetBool("Point", true);
        //else
        //    animator.SetBool("Point", false);

        


        if (TriggerUp())
        {

        }

        if (TouchpadPressDown())
        {

                
        }

        if (GripButtonDown())
        {

        }
    }

    
}
