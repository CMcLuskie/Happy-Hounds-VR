using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBrain : Dog {

    [SerializeField]
    protected PlayerStats playerStatsScript;
    [SerializeField]
    protected GameObject toyPrefab;
    GameObject newToy;

    [SerializeField]
    protected Transform mouth;
    [SerializeField]
    protected Transform foodBowl;
    [SerializeField]
    protected Transform waterBowl;
    [SerializeField]
    protected Transform foodBag;

    float wanderTimer;

    float move;

    Vector3 wanderPos;

    private bool idleTimerOn;
    private bool ballInterest;
    public bool isWaking;

    [HideInInspector]
    public GameObject toy;

    public enum Seekable { Player, Toy };
    public enum DogBehaviours { FollowToy, Wandering, FollowPlayer, FollowFood, FollowWater, Eating, Drinking, Sitting, PickedUp };
    public DogBehaviours previousBehaviour;
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
        ChangeBehaviour(DogBehaviours.FollowPlayer);
        isWaking = false;
        idleTimerOn = false;
    }

    #endregion

    #region Decisions
    // Update is called once per frame
    void Update()
    {
        DecisionMaker(currentBehaviour);

        if (transform.position.y < 0)
            ResetYPosition();

        if (OutOfBounds())
            ResetPosition();

        if (idleTimerOn)
            idleTimer += Time.deltaTime;

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
        print(behaviours);
        //this is just to make sure idle timer doesnt keep running
        if (previousBehaviour == DogBehaviours.Sitting)
            WakeUp();
        switch (behaviours)
        {
            case DogBehaviours.FollowFood:
                FollowFood();
                break;
            case DogBehaviours.Eating:
                Eating();
                break;
            case DogBehaviours.Drinking:
                Drinking();
                break;
            case DogBehaviours.FollowWater:
                FollowWater();
                break;
            case DogBehaviours.FollowPlayer:
                FollowPlayer();
                break;
            case DogBehaviours.FollowToy:
                FollowToy();
                break;
            case DogBehaviours.Wandering:
                Wandering();
                break;
            case DogBehaviours.Sitting:
                Sitting();
                break;


        }
    }

    #region Behaviours

    private void FollowPlayer()
    {

        float followTimer = 0;
        followTimer += Time.deltaTime;
        if(followTimer>= 10)
        {
            if (statList[(int)Stats.Energy] < 60)
                ChangeBehaviour(DogBehaviours.Sitting);
            else
                ChangeBehaviour(DogBehaviours.Wandering);
        }

        if (!ClosetoPoint(transform.position, PlayerPos(), 1))
            GoToPoint(PlayerPos(), 0.01f, 1);

        if(ClosetoPoint(transform.position, PlayerPos(), 1))
        {
            if ((previousBehaviour == DogBehaviours.FollowToy))
                DropToy();

            ChangeBehaviour(DogBehaviours.Sitting);
        }        
    }

    private void FollowToy()
    {
        GoToPoint(ToyPos(toy), 0.01f, 0.4f);

        if (ClosetoPoint(transform.position, ToyPos(toy), 0.2f))
        {
            PickUpToy();
            ChangeBehaviour(DogBehaviours.FollowPlayer);
        }
    }

    private void FollowFood()
    {
        if (playerStatsScript.pickedUpFood)
        {
            GoToPoint(foodBag.position, 0.01f, 0.3f);
            //implement jump later
        }
        else
        {
            GoToPoint(foodBowl.position, 0.01f, 0.3f);
            if ((ClosetoPoint(transform.position, foodBowl.transform.position, 0.3f)) && (statList[(int)Stats.Hunger] < 80))
                ChangeBehaviour(DogBehaviours.Eating);
        }
            
    }

    private void FollowWater()
    {
        GoToPoint(waterBowl.position, 0.01f, 0.3f);
        if (ClosetoPoint(transform.position, waterBowl.transform.position, 0.3f) && (statList[(int)Stats.Thirst] < 80))
            ChangeBehaviour(DogBehaviours.Drinking);
    }

    private void Eating()
    {

        animator.SetBool("Consume", true);
        animator.SetBool("Eat", true);

        statList[(int)Stats.Hunger] += Time.deltaTime * 2;
            
            if (GetDogStats(Stats.Hunger) == 100)
        {
            animator.SetBool("Consume", false);
            animator.SetBool("Eat", false);
            wanderPos = gridScript.GetRandomNode().coord;
            ChangeBehaviour(DogBehaviours.Sitting);
        }
    }

    private void Drinking()
    {
        animator.SetBool("Consume", true);
        animator.SetBool("Drink", true);
        statList[(int)Stats.Thirst] += Time.deltaTime * 2;
        if (GetDogStats(Stats.Thirst) == 100)
        {
            animator.SetBool("Consume", false);
            animator.SetBool("Drink", false);
            wanderPos = gridScript.GetRandomNode().coord;
            ChangeBehaviour(DogBehaviours.Sitting);
        }
            
    }

    private void Sitting()
    {
        #region Idle
        idleTimerOn = true;
        animator.SetFloat("IdleLength", idleTimer);
        if (statList[(int)Stats.Energy] < 50)
        {
            if ((idleTimer > 15 && idleTimer < 16) || (idleTimer > 25 && idleTimer < 26))
                ScratchCheck();
            else
                animator.SetBool("Scratch", false);
        }
        else if (idleTimer >= 20)
        {
            WakeUp();
            ChangeBehaviour(DogBehaviours.Wandering);
        }
        #endregion

        if (playerStatsScript.pickedUpToy)
        {
            WakeUp();
            ChangeBehaviour(DogBehaviours.FollowToy);
        }

        if (playerStatsScript.calledDog)
        {
            WakeUp();
            ChangeBehaviour(DogBehaviours.FollowPlayer);
        }

    }

    private void Wandering()
    {

        if (ClosetoPoint(transform.position, wanderPos, 1))
            wanderPos = gridScript.GetRandomNode().coord;

        GoToPoint(wanderPos, 0.01f, 0);
        if (playerStatsScript.calledSit)
            ChangeBehaviour(DogBehaviours.Sitting);

        if (playerStatsScript.pickedUpToy)
            ChangeBehaviour(DogBehaviours.FollowToy);

        if (playerStatsScript.calledDog)
            ChangeBehaviour(DogBehaviours.FollowPlayer);

        if (playerStatsScript.pickedUpFood)
        {
            if ((statList[(int)Stats.Hunger] < 80) && (ClosetoPoint(transform.position, PlayerPos(), 5)))
                ChangeBehaviour(DogBehaviours.FollowFood);
        }

        if (isHungry())
            ChangeBehaviour(DogBehaviours.FollowFood);

        if (isThirsty())
            ChangeBehaviour(DogBehaviours.FollowWater);
    }

    public void ChangeBehaviour(DogBehaviours latest)
    {
        previousBehaviour = currentBehaviour;
        currentBehaviour = latest;
        if (currentBehaviour == DogBehaviours.Wandering)
            wanderPos = gridScript.GetRandomNode().coord;
    }
#endregion
    #endregion

    #region Toy
    void PickUpToy()
        {
        if(ClosetoPoint(mouth.position,toy.transform.position, 0.2f))
        {
            toy.transform.position = mouth.position;
            var joint = AddFixJoint();
            joint.connectedBody = toy.GetComponent<Rigidbody>();
            
        }

        }
    private FixedJoint AddFixJoint()
    {
        //creates new joint
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        //sets joint so it doesnt break easily
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }
    void DropToy()
    {
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            
        }            
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

    //    public void CloseToSomething(Seekable seek)
    //{
    //    switch (seek)
    //    {
    //        case Seekable.Player:
    //            followPlayer = false;
    //            closeToPlayer = true;
    //         break;
    //        case Seekable.Toy:
    //            toySeen = false;
    //            PickUpToy();
    //            break;
    //    }
    //}

   

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
    }


    #endregion
}
