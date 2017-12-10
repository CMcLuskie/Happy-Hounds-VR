using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public abstract class Controllers : MonoBehaviour {

    [SerializeField]
    protected AudioManager audioScript;
    [SerializeField]
    protected UIMGR uiScript;

    public Animator animator;
    [SerializeField]
    protected PlayerStats playerStatsScript;
    [SerializeField]
    protected GameObject otherHand;

    public bool isPetting;

    [SerializeField]
    protected Transform palmTransform;
    [HideInInspector]
    public SteamVR_TrackedObject trackedObj;

    //grab variables
    [HideInInspector]
    public GameObject collidingObject;
    [HideInInspector]
    public GameObject objectInHand;

    public Transform playerHead;

    public delegate void OnDogPet();
    public static event OnDogPet HeadScratch;
    //public static event OnDogPet StopHeadScratch;
    public SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    public void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    #region Input
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
        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Grip))
            return true;
        else
            return false;
    }

    public bool MenuButtonDown()
    {
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            return true;
        else
            return false;
    }

    public bool MenuButtonUp()
    {
        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
            return true;
        else
            return false;
    }

    public float TriggerPos()
    {
        return Controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;
    }

    public Vector2 GetTouchpadPos()
    {
        Vector2 touchpad = Controller.GetAxis(EVRButtonId.k_EButton_Axis0);
        return touchpad;
    }

    public void HandAnimaiton(float handPos)
    {
        animator.SetFloat("Grab", handPos);
    }
#endregion
    public void ControllerVibrate(ushort vibration)
    {
        Controller.TriggerHapticPulse(vibration);
    }

#region Triggers
    public void OnTriggerEnter(Collider other)
    {;
        if ((other.tag == "Head") || (other.tag == "Body"))
        {
            playerStatsScript.pettingDog = true;
            HeadScratch();
            ControllerVibrate(500);

        }

        if (other.tag == "Button")
            uiScript.UseButton(other.name);
    }

    public void OnTriggerStay(Collider other)
    {
        if ((other.tag == "Head") || (other.tag == "Body"))
        {
            playerStatsScript.pettingDog = true;
            ControllerVibrate(500);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        
        playerStatsScript.pettingDog = false;
        if (!collidingObject)
            return;
        collidingObject = null;
        
    }
    #endregion

#region gets
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
    #endregion

}
