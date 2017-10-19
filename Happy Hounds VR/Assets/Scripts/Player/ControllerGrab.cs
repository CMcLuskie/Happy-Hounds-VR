using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrab : MonoBehaviour {


    private SteamVR_TrackedObject trackedObj;

    private GameObject collidingObject;//stores the GO the trigger is colliding with 
    private GameObject objectInHand;//reference to GO player is currently grabbing


    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())//doesnt make GO potential grab if player already is holding something
        {
            return;
        }
        Debug.Log("this happens");

        collidingObject = col.gameObject;//assigns object as potential grab object
    }

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
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
    }

    private void GrabObject()
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

    private void ReleaseObject()
    {
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
            objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
        }
        objectInHand = null;
    }

    private void Update()
    {
        if (Controller.GetHairTriggerDown())
        {

            if (collidingObject)
            {
                GrabObject();
            }
        }


        if (Controller.GetHairTriggerUp())
        {
            if (objectInHand)
                ReleaseObject();
        }

    }
}
