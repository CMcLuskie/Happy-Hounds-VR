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
    //[SerializeField]
    //protected GameObject indexCollider;
    //[SerializeField]
    //protected GameObject handCollider;
#endregion
 

    //petting
    public bool petting;


#endregion
    private void Start()
    {
      
        //indexCollider.SetActive(false);
        //handCollider.SetActive(true);
    }

    private void Update()
    {
        //if (playerStatsScript.pickedUpTablet)
        //    Point();
        //else
        //    DontPoint();


        if (Input.GetKeyDown(KeyCode.Space))
            Point();

        //Grab
        if (TriggerDown())
        {
            animator.SetBool("Grab", true);

        }


        if (TriggerUp())
        {
            animator.SetBool("Grab", false);
        }


        if (MenuButtonDown())
        {
            playerStatsScript.calledDog = true;
            audioScript.PlayClip(AudioManager.AudioSources.Player, AudioManager.ClipNames.Whistle);
        }

        if (petting)
            ControllerVibrate(500);

        if (GripButtonDown())
            tabletObject.transform.position = transform.position;            
    }


    #region UI

    void Point()
    {
        print("point");
        animator.SetBool("Point", true);
        //indexCollider.SetActive(true);
        //handCollider.SetActive(false);
    }

    /// <summary>
    /// it's rude
    /// </summary>
    void DontPoint()
    {
        animator.SetBool("Point", false);
        //indexCollider.SetActive(false);
        //handCollider.SetActive(true);
    }
#endregion
}
