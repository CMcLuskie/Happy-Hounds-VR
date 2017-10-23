using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Dog : MonoBehaviour
{
    public enum Direction { Forward, Back, Left, Right }; //for movement

    public enum Stats { Happiness, Hunger, Thirst, Cleanliness, Obedience };//for stats

    public enum FoodTypes { Pup, Adult, Senior };
    private int foodType;


    [SerializeField]
    protected bool pup;
    [SerializeField]
    protected bool adult;
    [SerializeField]
    protected bool senior;

    float happiness;
    float hunger;
    float thirst;
    float cleanliness;
    float obedience;

    GameObject gridObject;
    Grid gridScript;

    void Start()
    {
        FindGrid();
        SetFoodType();
    }

    void FindGrid()
    {
        gridObject = GameObject.FindGameObjectWithTag("Grid");
        gridScript = gridObject.GetComponent<Grid>();
    }

    private void SetFoodType()
    {
        if (pup)
            foodType = (int)FoodTypes.Pup;
        if (adult)
            foodType = (int)FoodTypes.Adult;
        if (senior)
            foodType = (int)FoodTypes.Senior;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(LerpToRandomPoint());
        }
    }

    public float GetDogStats(Stats stats)
    {
        switch (stats)
        {
            case Stats.Happiness:
                if (happiness > 100)
                {
                    happiness = 100;
                }
                return happiness;
            case Stats.Hunger:
                if (hunger > 100)
                {
                    hunger = 100;
                }
                return hunger;
            case Stats.Thirst:
                if (thirst > 100)
                {
                    thirst = 100;
                }
                return thirst;
            case Stats.Cleanliness:
                if (cleanliness > 100)
                {
                    cleanliness = 100;
                }
                return cleanliness;
            case Stats.Obedience:
                if (obedience > 100)
                {
                    obedience = 100;
                }
                return obedience;
        }
        return 0;
    }

    public int GetFoodType()
    {
        return foodType;
    }

    public virtual void Move(Direction dir)
    {
        switch (dir)
        {
            //But what's your opinion on the death rate of bees
            case Direction.Forward:
                transform.localPosition += Vector3.forward * 2;
                break;
            case Direction.Back:
                transform.localPosition += Vector3.back * 2;
                break;
            case Direction.Left:
                transform.localPosition += Vector3.left * 2;
                break;
            case Direction.Right:
                transform.localPosition += Vector3.right * 2;
                break;
        }
    }

    IEnumerator LerpToRandomPoint()
    {
        float totalTime = 10;
        float currentTime = 0;

        Vector3 currentPos = new Vector3(0, 0, 0);
        currentPos = transform.position;
        Vector3 goalPos = new Vector3(0, 0, 0);
        goalPos = gridScript.GetRandomPoint();
        while (currentTime < totalTime)
        {
            currentTime += Time.deltaTime;
            transform.position = 
                Vector3.Lerp(currentPos, goalPos, currentTime / totalTime);
            yield return 0;
        }

        StartCoroutine(LerpToRandomPoint());
    }
}
