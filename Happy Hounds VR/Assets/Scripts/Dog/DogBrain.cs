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

    float move;


    [HideInInspector]
    public bool closeToToy;
    [HideInInspector]
    public bool closeToPlayer;
    [HideInInspector]
    public bool toySeen;
    [HideInInspector]
    public bool toyCaught;
    [HideInInspector]
    public bool followPlayer;
    [HideInInspector]
    public bool isSitting;
    [HideInInspector]
    public bool isPickedUp;
    [HideInInspector]
    public bool isEating;
    [HideInInspector]
    public bool isDrinking;

    private bool ballInterest;
    bool isWaking;

    [HideInInspector]
    public GameObject toy;

    public enum Seekable { Player, Toy };
    enum DogBehaviours { FollowToy, Wandering, FollowPlayer, FollowFood, FollowWater, Eating, Drinking, Sitting, PickedUp};


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
        InitialiseStats(200);
        idleTimer = 0;
        }

    #endregion

    #region Decisions
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ChangeState(DogBehaviours.FollowFood);

        if (isSitting)
            DecisionMaker(DogBehaviours.Sitting);

        if (transform.position.y < 0)
            ResetYPosition();

        if (OutOfBounds())
            ResetPosition();

        if (ballInterest)
            if (toySeen)
                DecisionMaker(DogBehaviours.FollowToy);

        if (toyCaught)
            followPlayer = true;

        if (followPlayer)
            DecisionMaker(DogBehaviours.FollowPlayer);

        if (isPickedUp)
            DecisionMaker(DogBehaviours.PickedUp);

        if (isHungry)
            DecisionMaker(DogBehaviours.FollowFood);

        if (isSitting)
        {
            idleTimer += Time.deltaTime;
            if ((idleTimer > 15 && idleTimer < 16) || (idleTimer > 25 && idleTimer < 26))
                ScratchCheck();
            else
                animator.SetBool("Scratch", false);
            animator.SetFloat("IdleLength", idleTimer);
        }

        if (isWaking)
        { 
            idleTimer -= Time.deltaTime * 4;
            animator.SetFloat("IdleLength", idleTimer);
            if (idleTimer <= 0)
            {
                idleTimer = 0;
                isWaking = false;
            }
        }

    }

    void ScratchCheck()
    {
            animator.SetBool("Scratch", true);
    }
    void DecisionMaker(DogBehaviours behaviours)
    {
      
        switch (behaviours)
        {
            case DogBehaviours.FollowFood:

                    GoToPoint(foodBowl.position, 0.01f, 0.3f);
                //if(ClosetoPoint(foodBowl.transform.position, 0.3f))
                
                break;

            case DogBehaviours.Eating:
                animator.SetBool("Consume", true);
                animator.SetBool("Eat", true);
                break;

            case DogBehaviours.Drinking:

                break;

            case DogBehaviours.FollowWater:


                break; 
            case DogBehaviours.FollowPlayer:
                GoToPoint(PlayerPos(), 0.01f, 1);

                if (toyCaught)
                {
                    if (closeToPlayer)
                    {
                        DropToy();
                        StartCoroutine(BallInterestTimer());
                    }                        
                    else
                        newToy.transform.position = mouth.position;
                }
                    
                break;
            case DogBehaviours.FollowToy:
                GoToPoint(ToyPos(toy), 0.01f, 0.4f);

                if (toyCaught)
                {
                    followPlayer = true;
                    toySeen = false;
                }

                break;
            case DogBehaviours.Wandering:
                GoToPoint(gridScript.GetRandomNode().coord, 0.01f, 200f);

                break;
        }
    }

    void ChangeState(DogBehaviours behaviours)
    {
        if (behaviours != DogBehaviours.Sitting)
            WakeUp();
        switch (behaviours)
        {
            case DogBehaviours.FollowFood:
                isHungry = true;
                isThirtsy = false;
                followPlayer = false;
                isPickedUp = false;
                ballInterest = false;
                break;
            case DogBehaviours.FollowPlayer:
                isHungry = false;
                isThirtsy = false;
                followPlayer = true;
                isPickedUp = false;
                ballInterest = false;
                break;
            case DogBehaviours.FollowToy:
                isHungry = false;
                isThirtsy = false;
                followPlayer = false;
                isSitting = false;
                isPickedUp = false;
                ballInterest = true;
                break;
            case DogBehaviours.PickedUp:
                isHungry = false;
                isThirtsy = false;
                followPlayer = false;
                isPickedUp = true;
                ballInterest = false;
                break;
            case DogBehaviours.Sitting:
                isHungry = false;
                isThirtsy = false;
                followPlayer = false;
                isPickedUp = false;
                ballInterest = false;
                break;
            case DogBehaviours.Wandering:
                isHungry = false;
                isThirtsy = false;
                followPlayer = false;
                isPickedUp = false;
                ballInterest = false;
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
        isPickedUp = false;
            GetComponent<Rigidbody>().useGravity = true;
        }
        private void DogDropped()
        {
            isPickedUp = false;
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

    #region Bug Prevention
    
    IEnumerator BallInterestTimer()
    {
        ballInterest = false;
        yield return new WaitForSeconds(10);
        ballInterest = true;
    }

    /// <summary>
    /// this takes the dog out of its idle animations
    /// </summary>
    void WakeUp()
    {
        isWaking = true;
        isSitting = false;

    }


    #endregion
}
