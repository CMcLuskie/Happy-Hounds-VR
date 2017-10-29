using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGRab : Controllers
{
    [HideInInspector]
    public GameObject collidingObject;//stores the GO the trigger is colliding with 
    [HideInInspector]
    public GameObject objectInHand;//reference to GO player is currently grabbing


    

    

    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())//doesnt make GO potential grab if player already is holding something
        {
            return;
        }
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
    }

    private void Update()
    {
        if (TriggerDown())
        {
            if (collidingObject)
            {
                Debug.Log("this happens");
                GrabObject();
            }
        }
            

        if (TriggerUp())
        {
            if (objectInHand)
                ReleaseObject();
        }
            
    }
}
