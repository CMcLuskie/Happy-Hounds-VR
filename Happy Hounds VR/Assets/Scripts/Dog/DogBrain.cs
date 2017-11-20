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

    Vector3 wanderPos;
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
    // [HideInInspector]
    public bool isEating;
    [HideInInspector]
    public bool isDrinking;

    private bool ballInterest;
    bool isWaking;

    [HideInInspector]
    public GameObject toy;

    public enum Seekable { Player, Toy };
    enum DogBehaviours { FollowToy, Wandering, FollowPlayer, FollowFood, FollowWater, Eating, Drinking, Sitting, PickedUp };
    DogBehaviours previousBehaviour;
    DogBehaviours currentBehaviour;

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
        wanderPos = new Vector3(100, 100, 100);
    }

    #endregion

    #region Decisions
    // Update is called once per frame
    void Update()
    {
        DecisionMaker(DogBehaviours.Sitting);

        if (transform.position.y < 0)
            ResetYPosition();

        if (OutOfBounds())
            ResetPosition();

        

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
        Debug.Log(behaviours);
        //this is just to make sure idle timer doesnt keep running
        if (behaviours != DogBehaviours.Sitting)
            WakeUp();
        switch (behaviours)
        {
            case DogBehaviours.FollowFood:
                
                break;

            case DogBehaviours.Eating:
                

                print(GetDogStats(Stats.Hunger));
                reak;

            case DogBehaviours.Drinking:
                GoToPoint(waterBowl.position, 0.01f, 0.3f);
                if (GetDogStats(Stats.Thirst) >= 80)
                    ChangeState(DogBehaviours.Wandering);
                break;

            case DogBehaviours.FollowWater:

                if (ClosetoPoint(waterBowl.transform.position, 0.3f))
                    DecisionMaker(DogBehaviours.Drinking);
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
                

                break;


        }
    }

   

    #region Behaviours

    private void FollowPlayer() { }

    private void FollowToy() { }

    private void FollowFood()
    {
        GoToPoint(foodBowl.position, 0.01f, 0.3f);
        if (ClosetoPoint(foodBowl.transform.position, 0.3f))
            DecisionMaker(DogBehaviours.Eating);
    }

    private void FollowWater() { }

    private void Eating()
    {
        isEating = true;
        animator.SetBool("Consume", true);
        animator.SetBool("Eat", true);

        statList[(int)Stats.Hunger] += Time.deltaTime * 2;
        if (GetDogStats(Stats.Hunger) >= 100)
            ChangeBehaviour(DogBehaviours.Wandering);
    }

    private void Drinking()
    {
        statList[(int)Stats.Thirst] += Time.deltaTime * 2;
        if (GetDogStats(Stats.Thirst) == 100)
            ChangeBehaviour(DogBehaviours.Wandering);
    }

    private void Sitting()
    {
        idleTimer += Time.deltaTime;
        if ((idleTimer > 15 && idleTimer < 16) || (idleTimer > 25 && idleTimer < 26))
            ScratchCheck();
        else
            animator.SetBool("Scratch", false);
        animator.SetFloat("IdleLength", idleTimer);
    }

    private void Wandering()
    {
        if (transform.position == wanderPos)
            wanderPos = gridScript.GetRandomNode().coord;

        GoToPoint(wanderPos, 0.01f, 200f);
    }

    private void ChangeBehaviour(DogBehaviours latest)
    {
        previousBehaviour = currentBehaviour;
        currentBehaviour = latest;
        DecisionMaker(currentBehaviour);
    }
#endregion
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
