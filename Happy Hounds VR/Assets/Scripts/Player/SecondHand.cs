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
            if (collidingObject)
            {
                GrabObject();
            }
        }


        if (TriggerUp())
        {
            if (objectInHand)
                ReleaseObject();
        }

        if (TouchpadPressDown())
        {

            //if (GetTouchpadPos().x > 0)
            //{
            //    DogCall(playerHead.transform.position);
            //}
            //else if (GetTouchpadPos().x < 0)
            //{
            //   //DogSit(playerHead.transform.position);
            //}

            //if (GetTouchpadPos().x > 0)
            //{
            //    DogCall(playerHead.transform.position);
            //}
            //else if (GetTouchpadPos().x < 0)
            //{

            //   //DogSit(playerHead.transform.position);
            //}

                
        }

        if (GripButtonDown())
        {
            dogBrainScript.followPlayer = true;
        }
    }

    
}
