using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBrain : Dog {

    [SerializeField]
    protected GameObject toyPrefab;
    GameObject newToy;
    [SerializeField]
    protected Transform mouth;

    public float wanderTimer;

    public bool toyCaught;
    public bool followPlayer;

    bool pickedUp;
    float move;

    bool wandering;
    public bool closeToToy;
    public bool closeToPlayer;

    GameObject toy;

    public bool toySeen;
    public Transform player;
    Vector3 playerFeet;


    Vector3 targetPos;

    public enum Seekable { Player, Toy };

    private void OnEnable()
    {
        SecondHand.DogCall += DogCall;
        MainHand.HeadScratch += HeadScratch;
        MainHand.BodyScratch += BodyScratch;
    }



    private void OnDisable()
    {
        SecondHand.DogCall -= DogCall;
        MainHand.HeadScratch -= HeadScratch;
        MainHand.BodyScratch -= BodyScratch;
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

        if (transform.position.y < 0.22f)
        {
            Vector3 fix = new Vector3(transform.position.x, 0.22f, transform.position.z);
            transform.position = fix;
        }

        if (OutOfBounds())
        {
            ResetPosition();
        }

        if (toySeen)
        {
            GoToPoint(targetPos, .01f);
        }

        if (toyCaught)
        {            
            if(!closeToPlayer)
            {
                followPlayer = true;
                newToy.transform.position = mouth.position;
            }
            if (closeToPlayer)
                DropToy();
        }

        if (followPlayer)
            GoToPoint(PlayerPos(), .01f);


    }

    public void CloseToSomething(Seekable seek)
    {
        switch (seek)
        {
            case Seekable.Player:
                followPlayer = false;
                closeToPlayer = true;
             break;
            case Seekable.Toy:
                toySeen = false;
                PickUpToy();
                break;
        }
    }

    
    private void ResetPosition()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        Quaternion quar = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        transform.SetPositionAndRotation(new Vector3(-10, .22f, -10), quar);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    private void GoToPoint(Vector3 pos, float speed)
    {
        transform.LookAt(pos);
        Quaternion quar = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        transform.SetPositionAndRotation(transform.position, quar);

        animator.SetFloat("Move", 2.6f);

        if (higherX(pos))
            Move(Direction.Right, speed);
        else
            Move(Direction.Left, speed);

        if (higherZ(pos))
            Move(Direction.Forward, speed);
        else
            Move(Direction.Back, speed);
    }

    void PickUpToy()
    {
        if (!toyCaught)
        {
            print("pick up");
            Destroy(GameObject.FindGameObjectWithTag("Toy"));
            newToy = Instantiate(toyPrefab, mouth.position, Quaternion.identity);
        }
        toyCaught = true;
        pickedUp = true;
    }

    void DropToy()
    {
        toyCaught = false;
        Vector3 newPos = new Vector3();
        newPos = new Vector3(-10, .22f, -10);
        while (transform.position != newPos)
            GoToPoint(newPos, .01f);
        transform.LookAt(PlayerPos());
    }

    private bool OutOfBounds()
    {
        if ((transform.position.x >= -3) || (transform.position.x <= -15))
            return true;
        else if ((transform.position.z >= 4) || (transform.position.z <= -12))
            return true;
        else
            return false;

    }

    void Wandering()
    {
        Lerping(gridScript.GetRandomNode());
        animator.SetFloat("Move", .6f);
    }

    void DogCall(Vector3 playerPos)
    {
        isWandering = false;
        attentionGiven = true;
        Node node = gridScript.coordToNode(playerPos);
        // Lerping(node);
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

    public void ToyPos(GameObject toy)
    {
        targetPos = toy.transform.position;
        targetPos.y = transform.position.y;
    }

    private int DistanceToPoint(Vector3 target)
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
        print(difference);
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

    Vector3 PlayerPos()
    {
        playerFeet = player.transform.position;
        playerFeet.y = 0.22f;
        return playerFeet;
    }

}
