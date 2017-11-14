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
    [SerializeField]
    protected Transform foodBowl;
    [SerializeField]
    protected Transform waterBowl;

    float wanderTimer;

    bool pickedUp;
    float move;

     

    public bool closeToToy;
    public bool closeToPlayer;
    public bool toySeen;
    public bool toyCaught;
    public bool followPlayer;
    [HideInInspector]
    public GameObject toy;

    public enum Seekable { Player, Toy };
    enum DogBehaviours { FollowToy, Wandering, FollowPlayer, FollowFood};

    #region Unity Methods
        private void OnEnable()
        {

            MainHand.HeadScratch += HeadScratch;
            MainHand.BodyScratch += BodyScratch;
            MainHand.StopBodyScratch += StopBodyScratch;
            MainHand.StopHeadScratch += StopHeadScratch;
            SecondHand.DogSit += DogSit;
            SecondHand.DogCall += DogCall;
        }

   

    private void OnDisable()
        {
            MainHand.HeadScratch -= HeadScratch;
            MainHand.BodyScratch -= BodyScratch;
            MainHand.StopBodyScratch -= StopBodyScratch;
            MainHand.StopHeadScratch -= StopHeadScratch;
            SecondHand.DogSit -= DogSit;
            SecondHand.DogCall -= DogCall;
    }

        // Use this for initialization
        void Start()
        {

        }

    #endregion

    #region Decisions
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
           // ResetPosition();
        }

        if (toySeen)
            DecisionMaker(DogBehaviours.FollowToy);

        if (toyCaught)
            followPlayer = true;

        if (followPlayer)
            DecisionMaker(DogBehaviours.FollowPlayer);

        if (pickedUp)
        {

        }
    }

    void DecisionMaker(DogBehaviours behaviours)
    {
      
        switch (behaviours)
        {
            case DogBehaviours.FollowFood:
                GoToPoint(foodBowl.position, 0.01f);

                break; 
            case DogBehaviours.FollowPlayer:
                GoToPoint(PlayerPos(), 0.01f);

                if (toyCaught)
                {
                    if (closeToPlayer)
                        DropToy();
                    else
                        newToy.transform.position = mouth.position;
                }
                    
                break;
            case DogBehaviours.FollowToy:
                GoToPoint(ToyPos(toy), 0.01f);

                if (toyCaught)
                {
                    followPlayer = true;
                    toySeen = false;
                }

                break;
            case DogBehaviours.Wandering:
                GoToPoint(gridScript.GetRandomNode().coord, 0.01f);

                break;
        }
    }
#endregion

    #region Toy
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
            //Vector3 newPos = new Vector3();
            //newPos = new Vector3(-10, .22f, -10);
            //while (transform.position != newPos)
            //    GoToPoint(newPos, .01f);
            transform.LookAt(PlayerPos());
        }

        public Vector3 ToyPos(GameObject toy)
        {
            Vector3 targetPos = new Vector3();
            targetPos = toy.transform.position;
            targetPos.y = transform.position.y;
            return targetPos;
        }

        #endregion

    #region Interactions
        private void HeadScratch()
        {

        }
        private void BodyScratch()
        {


        }
    private void StopHeadScratch()
    {
        throw new NotImplementedException();
    }

    private void StopBodyScratch()
    {
        throw new NotImplementedException();
    }
    private void DogCall(Vector3 playerPos)
    {
        throw new NotImplementedException();
    }

    private void DogSit(Vector3 playerPos)
    {
        throw new NotImplementedException();
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

        

        #endregion

    #region Movement nad Positioning
    private int DistanceToPoint(Vector3 target)
    {
        int difference = 0;
        Vector3 differenceVector = new Vector3(0, 0, 0);
        if (target.x < transform.position.x)
        {
            differenceVector.x = transform.position.x - target.x;
        }
        else
        {
            differenceVector.x = target.x - transform.position.x;
        }
        if (target.z < transform.position.z)
        {
            differenceVector.z = transform.position.z - target.z;
        }
        else
        {
            differenceVector.z = target.z - transform.position.z;
        }
        difference = (int)differenceVector.x + (int)differenceVector.z;
        print(difference);
        return difference;
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

    void Wandering()
    {
        Lerping(gridScript.GetRandomNode());
        animator.SetFloat("Move", .6f);
    }

    bool OutOfBounds()
    {
        if ((transform.position.x >= -3) || (transform.position.x <= -15))
            return true;
        else if ((transform.position.z >= 4) || (transform.position.z <= -12))
            return true;
        else
            return false;

    }

    #endregion
}
