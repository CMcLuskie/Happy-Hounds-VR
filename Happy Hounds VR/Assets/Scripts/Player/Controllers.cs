using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public abstract class Controllers : MonoBehaviour {

    [SerializeField]
    protected AudioManager audioScript;
    [SerializeField]
    protected UIMGR uiScript;
    [SerializeField]
    protected GameObject handModel;
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
    {
        SetCollidingObject(other);
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

        SetCollidingObject(other);
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

#region pick up
    public void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())//doesnt make GO potential grab if player already is holding something
        {
            return;
        }
        collidingObject = col.gameObject;//assigns object as potential grab object
    }



  //  public void GrabObject()
//    {
//        handModel.SetActive(false);
//        Debug.Log("Grab Object");
//        objectInHand = collidingObject;//moves GO to players hand

//        #region Food
//        if (objectInHand.name == "Food Bag")
//            playerStatsScript.pickedUpFood = true;
//#endregion

//        #region Toy
//        if (objectInHand.tag == "Toy")
//            playerStatsScript.pickedUpToy = true;
//#endregion

//        #region Tablet
//        if (objectInHand.tag == "Tablet")
//        {
//            playerStatsScript.pickedUpTablet = true;
//            //puts object in players hand
//            objectInHand.transform.position = palmTransform.position;
//            #region Tablet Offset
//                        if (gameObject.name == "Right Hand")
//                            objectInHand.transform.position -= new Vector3(0, 0, .2f);
//                        else if (gameObject.name=="Left Hand")
//                            objectInHand.transform.position += new Vector3(0, 0, .2f);

//            #endregion
//            Quaternion quaternion = new Quaternion(objectInHand.transform.rotation.x, 90, objectInHand.transform.rotation.z, objectInHand.transform.rotation.w);
//            objectInHand.transform.SetPositionAndRotation(objectInHand.transform.position, quaternion);
//        }
//#endregion
//        else
//            objectInHand.transform.position = palmTransform.position;


//        collidingObject = null;//removes it from colliding object variable
//        var joint = AddFixJoint(); //sets joint variable
//        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();

//    }


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
            objectInHand.GetComponent<Rigidbody>().angularVelocity = ControllerAngularVelocity() * 1;
        }

        if (objectInHand.tag == "Toy")
            playerStatsScript.pickedUpToy = false;
        else if (objectInHand.tag == "Tablet")
            playerStatsScript.pickedUpTablet = false;

        handModel.SetActive(true);
        objectInHand = null;
    }
#endregion
}
