using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHand : Controllers {


    //General variables
    //public bool mainHand;
    //public bool secondHand;

   

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
            ControllerVibrate(500);
    }


    

    

    public void InstantiateLaser()
    {
        RaycastHit hit;

        if (Physics.Raycast
            (trackedObj.transform.position, transform.forward, out hit, 100, teleportMask))
        {
            print("cunt");

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
