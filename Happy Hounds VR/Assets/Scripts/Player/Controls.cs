using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : Controllers
{
    PlayerMovement movementScript;
    
    //General variables
    //public bool mainHand;
    //public bool secondHand;

    //grab variables
    GameObject collidingObject;
    GameObject objectInHand;

    //laser variables
    public GameObject laserPrefab;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitPoint;

    //Teleport Variables
    public Transform cameraRigTransform;
    public GameObject teleportReticlePrefab;
    private GameObject reticle;
    private Transform teleportReticleTransform;
    public Transform headTransform;
    public Vector3 teleportReticleOffset;
    public LayerMask teleportMask;
    [HideInInspector]
    public bool shouldTeleport;

    //petting
    public bool petting;
    //events
    public delegate void OnControllerInput(Vector3 playerPos);
    public static event OnControllerInput DogCall;
    public delegate void OnDogInteraction();
    public static event OnDogInteraction HeadScratch;
    public static event OnDogInteraction StopHeadScratch;
    public static event OnDogInteraction BodyScratch;
    public static event OnDogInteraction StopBodyScratch;

    private void Start()
    {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;

        reticle = Instantiate(teleportReticlePrefab);
        teleportReticleTransform = reticle.transform;
    }

    private void Update()
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

        //movement
        if (TouchpadPressDown())
        {
            InstantiateLaser();
        }
        else
        {
            DeactivateLaser();
            DeactivateReticle();
        }

        if (TouchpadPressUp() && shouldTeleport)
            Teleport();

        if (petting)
            ControllerVibrate();
    }
    

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
        if ((other.tag == "Head") || (other.tag=="Body"))
        {
            HeadScratch();
            petting = true;
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
            HeadScratch();
            petting = false;
        }
    }//Triggers
 
    private void SetCollidingObject(Collider col)
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

    public void InstantiateLaser() 
    {
        RaycastHit hit;

        if (Physics.Raycast
            (trackedObj.transform.position, transform.forward, out hit, 100, teleportMask))
        {
            hitPoint = hit.point;
            ShowLaser(hit);
            reticle.SetActive(true);
            teleportReticleTransform.position = hitPoint + teleportReticleOffset;
            shouldTeleport = true;
        }

    }

    /// <summary>
    /// Deactivates Laser
    /// </summary>
    public void DeactivateLaser()
    {
        laser.SetActive(false);
    }

    
    /// <summary>
    /// shows laser coming from player's hand
    /// </summary>
    /// <param name="hit"></param>
    private void ShowLaser(RaycastHit hit)
    {
        laser.SetActive(true);
        laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);
        laserTransform.LookAt(hitPoint);
        laserTransform.localScale =
            new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
    }//Laser

    /// <summary>
    /// allows the laser to teleport
    /// </summary>
    public void Teleport()
    {
        shouldTeleport = false;
        reticle.SetActive(false);
        Vector3 difference = cameraRigTransform.position - headTransform.position;
        difference.y = 0;
        cameraRigTransform.position = hitPoint + difference;
    }

    /// <summary>
    /// Deactivates Reticle
    /// </summary>
    public void DeactivateReticle()
    {
        reticle.SetActive(false);
    }//Teleport
}
