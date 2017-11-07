using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondHand : Controllers {

    public DogBrain dogBrainScript;
    public delegate void OnControllerInput(Vector3 playerPos);
    public static event OnControllerInput DogCall;
    public static event OnControllerInput DogSit;
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
<<<<<<< HEAD
            //if (GetTouchpadPos().x > 0)
            //{
            //    DogCall(playerHead.transform.position);
            //}
            //else if (GetTouchpadPos().x < 0)
            //{
            //   //DogSit(playerHead.transform.position);
            //}
=======
            if (GetTouchpadPos().x > 0)
            {
                DogCall(playerHead.transform.position);
            }
            else if (GetTouchpadPos().x < 0)
            {
               //DogSit(playerHead.transform.position);
            }
>>>>>>> 8962adc0a80569f3d2c26d8d514ceef1b0afa548
        }

        if (GripButtonDown())
        {
            //Vector3 pos = new Vector3(transform.position.x, .22f, transform.position.z);
            //DogCall(pos);
            dogBrainScript.followPlayer = true;
        }

        if (petting)
            ControllerVibrate(500);
        else
            ControllerVibrate(0);
    }

    
}
