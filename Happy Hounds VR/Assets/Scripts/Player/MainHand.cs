using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHand : Controllers {

#region Variables

    //General variables
    //public bool mainHand;
    //public bool secondHand;

#region UI
    [SerializeField]
    protected GameObject tabletObject;
    [SerializeField]
    protected GameObject indexCollider;
    [SerializeField]
    protected GameObject handCollider;
#endregion
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
    public delegate void OnDogInteraction();
    //public static event OnDogInteraction HeadScratch;
    public static event OnDogInteraction StopHeadScratch;
    public static event OnDogInteraction BodyScratch;
    public static event OnDogInteraction StopBodyScratch;

#endregion
    private void Start()
    {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;

        reticle = Instantiate(teleportReticlePrefab);
        teleportReticleTransform = reticle.transform;

        indexCollider.SetActive(false);
        handCollider.SetActive(true);
    }

    private void Update()
    {
        if (playerStatsScript.pickedUpTablet)
            Point();
        else
            DontPoint();


        if (Input.GetKeyDown(KeyCode.Space))
            Point();

        //Grab
        if (TriggerDown())
        {
            animator.SetBool("Grab", true);
            if (collidingObject)
                GrabObject();
        }

        //if (!isPetting)
        //    StopHeadScratch();

        if (TriggerUp())
        {
            animator.SetBool("Grab", false);

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

        if (MenuButtonDown())
            playerStatsScript.dogCalled = true;

        if (TouchpadPressUp() && shouldTeleport)
            Teleport();

        if (petting)
            ControllerVibrate(500);

        if (GripButtonDown())
            tabletObject.transform.position = transform.position;            
    }

    #region teleport

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
    }
    #endregion

    #region UI

    void Point()
    {
        print("point");
        animator.SetBool("Point", true);
        indexCollider.SetActive(true);
        handCollider.SetActive(false);
    }

    /// <summary>
    /// it's rude
    /// </summary>
    void DontPoint()
    {
        animator.SetBool("Point", false);
        indexCollider.SetActive(false);
        handCollider.SetActive(true);
    }
#endregion
}
