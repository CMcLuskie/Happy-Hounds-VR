﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DogBrain : Dog {

    public float wanderTimer;

    bool pickedUp;
    float move;
    
    bool wandering;

    private void OnEnable()
    {
        SecondHand.DogCall += DogCall;
        MainHand.HeadScratch += HeadScratch;
        MainHand.BodyScratch += BodyScratch;
        MainHand.DogPickedUp += DogPickedUp;
        MainHand.DogDropped += DogDropped;
    }

    

    private void OnDisable()
    {
        SecondHand.DogCall -= DogCall;
        MainHand.HeadScratch -= HeadScratch;
        MainHand.BodyScratch -= BodyScratch;
        MainHand.DogPickedUp -= DogPickedUp;
        MainHand.DogDropped -= DogDropped;

    }

    // Use this for initialization
    void Start ()
    {
        GetGridObject();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            Wandering();

        if (!isLerping)
        {
            animator.SetFloat("Move", 0.5f);
        }

        if (isWandering && !isLerping)
            Wandering();

        if ((transform.position.y < 0.22f) && (!pickedUp))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            
            Vector3 fix = new Vector3(transform.position.x, 0.22f , transform.position.z);

            transform.position = fix;
        }
         
           
	}

    void Wandering()
    {
        Lerping(gridScript.GetRandomNode(), 1);
        animator.SetFloat("Move", .6f);
    }

    void DogCall(Vector3 playerPos)
    {
        isWandering = false;
        attentionGiven = true;
        Node node = gridScript.coordToNode(playerPos);
        Lerping(node, 2);
        animator.SetFloat("Move", 6f);
    }

    private void HeadScratch()
    {
      
    }
    private void BodyScratch()
    {

    }

    private void DogPickedUp()
    {
        pickedUp = true;
        GetComponent<Rigidbody>().useGravity = true;
    }
    private void DogDropped()
    {
        pickedUp = false;
    }
}
