using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBrain : Dog {

    public float wanderTimer;
<<<<<<< HEAD

    public bool toyCaught;
    public bool followPlayer;

    bool pickedUp;
=======
    
>>>>>>> parent of ec4e8df... Code/Tidy Update
    float move;
    
    bool wandering;

<<<<<<< HEAD
    GameObject toy;

    public Transform player;
    Vector3 playerFeet;


    Vector3 targetPos;
=======
>>>>>>> parent of ec4e8df... Code/Tidy Update
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

        if (transform.position.y < 0)
        {
            Vector3 fix = new Vector3(transform.position.x, 0, transform.position.z);
            transform.position = fix;
        }
<<<<<<< HEAD

        if (toySeen)
        {
            GoToPoint(targetPos);
        }
        if (toyCaught)
        {
            GoToPoint(PlayerPos());
        }

        if (followPlayer)
            GoToPoint(PlayerPos());

        

    }

    private void GoToPoint(Vector3 pos)
    {
        transform.LookAt(pos);
        Quaternion quar = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        transform.SetPositionAndRotation(transform.position, quar);

        if (DistanceToToy() < 1)
        {
            toySeen = false;
            animator.SetFloat("Move", 0.5f);

        }
        else if (DistanceToToy() < 2)
            animator.SetFloat("Move", 2.6f);
        else if (DistanceToToy() < 3)
            animator.SetFloat("Move", 3.6f);
        else if (DistanceToToy() < 4)
            animator.SetFloat("Move", 4.6f);


        if (higherX(pos))
            Move(Direction.Right, 0.01f);
        else
            Move(Direction.Left, 0.01f);

        if (higherZ(pos))
            Move(Direction.Forward, 0.01f);
        else
            Move(Direction.Back, 0.01f);
    }

    private bool OutOfBounds()
    {
        if ((transform.position.x >= -3) || (transform.position.x <= -15))
            return true;
        else if ((transform.position.z >= 4) || (transform.position.z <= -12))
            return true;
        else
            return false;
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
=======
            
	}
>>>>>>> parent of ec4e8df... Code/Tidy Update

    void Wandering()
    {
         Lerping(gridScript.GetRandomNode());
       // TempWander(gridScript.GetRandomNode());
        animator.SetFloat("Move", .6f);
    }

    void DogCall(Vector3 playerPos)
    {
        isWandering = false;
        attentionGiven = true;
        Node node = gridScript.coordToNode(playerPos);
        // Lerping(node);
        TempWander(playerPos);
        animator.SetFloat("Move", 6f);

    }

    private void HeadScratch()
    {
      
    }
    private void BodyScratch()
    {
<<<<<<< HEAD

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

    Vector3 PlayerPos()
    {
        playerFeet = player.transform.position;
        playerFeet.y = 0.22f;
        return playerFeet;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            followPlayer = false;

    }

    public void ToyPos(GameObject toy)
    {
        targetPos = toy.transform.position;
        targetPos.y = transform.position.y;
=======
>>>>>>> parent of ec4e8df... Code/Tidy Update
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
