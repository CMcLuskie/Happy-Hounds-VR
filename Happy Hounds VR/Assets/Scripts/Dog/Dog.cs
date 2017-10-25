using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public abstract class Dog : MonoBehaviour
{
    public enum Direction { Forward, Back, Left, Right }; //for movement

    public enum Stats { Happiness, Hunger, Thirst, Cleanliness, Obedience };//for stats

    public enum FoodTypes { Pup, Adult, Senior };
    private int foodType;

    Vector3 goalPos;
     
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

    public bool isLerping = false;

    GameObject grid;
    Grid gridScript;

    void Start()
    {
        SetFoodType();
    }

   public virtual void GetGridObject()
    {
        grid = GameObject.FindGameObjectWithTag("Grid");
        gridScript = grid.GetComponent<Grid>();
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

    public virtual void StartDogLerp()
    {
        StartCoroutine(DogLerp());
        isLerping = true;
    }

    IEnumerator DogLerp()
    {
        float totalTime = 5;
        float currentTime = 0;
        Vector3 currentPos = transform.position;
        goalPos = gridScript.GetRandomNode();
        while (currentTime < totalTime)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(currentPos, goalPos, currentTime / totalTime);

            if (currentTime >= totalTime)
                isLerping = false;
            yield return 0;

        }
        

    }

    //public List<Vector3> Pathfinding(Vector3 startNode, Vector3 endNode)
    //{
    //    List<Vector3> openList = new List<Vector3>();
    //    List<Vector3> closedList = new List<Vector3>();

    //    openList.Add(startNode);

    //    while (openList.Count > 0)
    //    {
    //        openList.Sort();
    //        Vector3 currentNode = openList[0];

    //        openList.Remove(currentNode);
    //        closedList.Add(currentNode);


    //        if (currentNode == endNode)
    //        {
    //             //get found path
    //        }

    //        List<Vector3> connectedNodes = new List<Vector3>();
    //        for(int i=0; i < 3; i++)
    //        {
    //            for (int j = 0; j < 3; j++)
    //            {
    //                connectedNodes.Add(new Vector3(currentNode.x - 1 + i, currentNode.y, currentNode.z - 1 + j));
    //                connectedNodes.Remove(currentNode);
    //            }
    //        }

    //        for(int i=0; i<connectedNodes.Count; i++)
    //        {
    //            Vector3 connectedNode = connectedNodes[i];
    //            if (closedList.Contains(connectedNode))
    //            {
    //                continue;
    //            }

    //            int 
    //        }
    //    }
    //}

    private int ManhattanDistanceHeuristic(int currentX, int currentY, int targetX, int targetY)
    {
        int xDist = Mathf.Abs(currentX - targetX);
        int yDist = Mathf.Abs(currentY - targetY);

        return 10 * (xDist + yDist);
    }

    private int ChebyshevDistanceHeuristic(int currentX, int currentY, int targetX, int targetY)
    {
        int xDist = Mathf.Abs(currentX - targetX);
        int yDist = Mathf.Abs(currentY - targetY);

        return 10 * (xDist + yDist) + (14 - 2 * 10) * Mathf.Min(xDist, yDist);
    }

    private int EuclideanDistanceHeuristic(int currentX, int currentY, int targetX, int targetY)
    {
        int xDist = Mathf.Abs(currentX - targetX);
        int yDist = Mathf.Abs(currentY - targetY);

        return 10 * (int)Mathf.Sqrt((xDist * xDist + yDist * yDist) * (xDist * xDist + yDist * yDist));
    }

    //protected List<Vector3> GetFoundPath(Vector3 endNode)
    //{
    //    List<Vector3> foundPath = new List<Vector3>();
    //    if (endNode != null)
    //    {
    //        foundPath.Add(endNode);

    //        while(endNode)
    //    }
    //}
}
