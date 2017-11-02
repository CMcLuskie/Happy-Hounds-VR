﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public abstract class Controllers : MonoBehaviour {

    [HideInInspector]
    public SteamVR_TrackedObject trackedObj;

    //grab variables
    [HideInInspector]
    public GameObject collidingObject;
    [HideInInspector]

    public GameObject objectInHand;

    public Transform playerHead;

    public SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    public void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    /*
     * 
     * Start of button press bools
     * 
     */
    public bool TouchpadPressDown()
    {
        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            return true;
        else
            return false;
    }

    public bool TouchpadPressUp()
    {
        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
            return true;
        else
            return false;
    }

    public bool TriggerDown()
    {
        if (Controller.GetHairTriggerDown())
            return true;
        else
            return false;
    }

    public bool TriggerUp()
    {
        if (Controller.GetHairTriggerUp())
            return true;
        else
            return false;
    }

    public bool GripButtonDown()
    {
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            return true;
        else
            return false;
    }

    public bool GripButtonUp()
    {
        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
            return true;
        else
            return false;
    }

    public Vector2 GetTouchpadPos()
    {
        Vector2 touchpad = Controller.GetAxis(EVRButtonId.k_EButton_Axis0);
        return touchpad;
    }
    public void ControllerVibrate(ushort vibration)
    {
        Controller.TriggerHapticPulse(vibration);
    }

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
        if ((other.tag == "Head") || (other.tag == "Body"))
        {
            //petting = true;
            ControllerVibrate(500);
            //HeadScratch();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
            return;
        collidingObject = null;
        if ((other.tag == "Head") || (other.tag == "Body"))
        {
            ControllerVibrate(0);

        }
    }//Triggers
    /*
     * 
     * misc gets
     * 
     */
    public Vector3 ControllerVelocity()
    {
        return Controller.velocity;
    }

    public Vector3 ControllerAngularVelocity()
    {
        return Controller.angularVelocity;
    }

    public Vector3 GetPlayerPos()
    {
        Vector3 playerPos = playerHead.position;
        playerPos.y = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>().dogYPos;
        return playerPos;
    }

    public void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())//doesnt make GO potential grab if player already is holding something
        {
            return;
        }
        collidingObject = col.gameObject;//assigns object as potential grab object
    }



    public void GrabObject()
    {
        Debug.Log("Grab Object");
        objectInHand = collidingObject;//moves GO to players hand
        collidingObject = null;//removes it from colliding object variable
        var joint = AddFixJoint(); //sets joint variable
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }


    private FixedJoint AddFixJoint()
    {
        //creates new joint
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        //sets joint so it doesnt break easily
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    public void ReleaseObject()
    {
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            objectInHand.GetComponent<Rigidbody>().velocity = ControllerVelocity();
            objectInHand.GetComponent<Rigidbody>().angularVelocity = ControllerAngularVelocity();
        }
        objectInHand = null;
    }//Grab
}
