using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Controllers
{

    public GameObject laserPrefab;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitPoint;

    /// <summary>
    /// Teleport Variables
    /// </summary>
    public Transform cameraRigTransform;
    public GameObject teleportReticlePrefab;
    private GameObject reticle;
    private Transform teleportReticleTransform;
    public Transform headTransform;
    public Vector3 teleportReticleOffset;
    public LayerMask teleportMask;
    [HideInInspector]
    public bool shouldTeleport;

    

    private void Start()
    {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;

        reticle = Instantiate(teleportReticlePrefab);
        teleportReticleTransform = reticle.transform;
    }


    private void Update()
    {
        if (TouchpadPressDown())
        {
            InstantiateLaser();
        }
        else
        {
            DeactivateLaser();
            DeactivateReticle();
        }

        if(TouchpadPressUp() && shouldTeleport)
            Teleport();
            
    }

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
    /// Deactivates Reticle
    /// </summary>
    public void DeactivateReticle()
    {
        reticle.SetActive(false);
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
    }

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
}
