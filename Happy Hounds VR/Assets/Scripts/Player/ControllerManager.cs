using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{

    [SerializeField]
    protected bool mainHand;
    [SerializeField]
    protected bool secondHand;

    // Use this for initialization
    void Start()
    {
        if (mainHand)
        {
            GetComponent<MainHand>().enabled = true;
            GetComponent<SecondHand>().enabled = false;
        }
        else
        {
            GetComponent<MainHand>().enabled = false;
            GetComponent<SecondHand>().enabled = true;
        }
    }
}
	
