using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DogBrain : Dog {

    public float wanderTimer;

    bool pickedUp;
    float move;

    bool wandering;

    Vector3 targetPos;
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
    void Start()
    {
        GetGridObject();
    }

    // Update is called once per frame
    void Update() {
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
            Vector3 fix = new Vector3(transform.position.x, 0.22f, transform.position.z);
            transform.position = fix;
        }

        if (toySeen)
        {           
            transform.LookAt(targetPos);
            Quaternion quar = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
            transform.SetPositionAndRotation(transform.position, quar);

            if (DistanceToToy()< 1)
            {
                toySeen = false;
                animator.SetFloat("Move", 0.5f);
            }
                

            if (higherX(targetPos))
                Move(Direction.Right, 0.01f);
            else
                Move(Direction.Left, 0.01f);

            if (higherZ(targetPos))
                Move(Direction.Forward, 0.01f);
            else
                Move(Direction.Back, 0.01f);
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
        animator.SetFloat("Move", 5f);
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

    public void ToyPos(GameObject toy)
    {
        targetPos = toy.transform.position;
        targetPos.y = transform.position.y;
    }

    private int DistanceToToy()
    {
        int difference = 0;
        Vector3 differenceVector = new Vector3(0, 0, 0);
        if (targetPos.x < transform.position.x)
        {
            differenceVector.x = transform.position.x - targetPos.x; 
        }
        else
        {
            differenceVector.x = targetPos.x - transform.position.x;
        }
        if (targetPos.z < transform.position.z)
        {
            differenceVector.z = transform.position.z - targetPos.z;
        }
        else
        {
            differenceVector.z = targetPos.z - transform.position.z;
        }
        difference = (int)differenceVector.x + (int)differenceVector.z;
        return difference;
    }

    private bool higherX(Vector3 toyPos)
    {
        if (toyPos.x > transform.position.x)
            return true;
        else
            return false;
    }

    private bool higherZ(Vector3 toyPos)
    {
        if (toyPos.z > transform.position.z)
            return true;
        else
            return false;
    }

}
