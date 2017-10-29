using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controllers : MonoBehaviour {

    [HideInInspector]
    public SteamVR_TrackedObject trackedObj;

    public bool mainHand;
    public bool secondHand;

    private SteamVR_Controller.Device Controller
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

    /*
     * 
     * Velocitys
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
}
