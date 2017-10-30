using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : Controllers
{
    PlayerMovement movementScript;
    ControllerGRab grabScript;

    public delegate void OnControllerInput(Vector3 playerPos);

    public static event OnControllerInput DogCall;

    private void Awake()
    {
        movementScript = GetComponent<PlayerMovement>();
        grabScript = GetComponent<ControllerGRab>();
    }

    private void Update()
    {

        //movement
        if (TouchpadPressDown())
        {
            movementScript.InstantiateLaser();
        }
        else
        {
            movementScript.DeactivateLaser();
            movementScript.DeactivateReticle();
        }

        if (TouchpadPressUp() && movementScript.shouldTeleport)
            movementScript.Teleport();

        if (mainHand)
        {
            //Grab
            if (TriggerDown())
            {
                if (grabScript.collidingObject)
                {
                    grabScript.GrabObject();
                }
            }


            if (TriggerUp())
            {
                if (grabScript.objectInHand)
                    grabScript.ReleaseObject();
            }
        }

        if (secondHand)
        {
            if (TriggerDown())
            {
                DogCall(GetPlayerPos());
            }
        }
    }

    
}
