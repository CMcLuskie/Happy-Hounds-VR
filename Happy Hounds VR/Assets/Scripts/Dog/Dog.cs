using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public abstract class Dog : MonoBehaviour
{
    public enum Direction { Forward, Back, Left, Right }; //for movement
    public enum PathfindingTypes { AStar, BFS};

    public enum Stats { Happiness, Hunger, Thirst, Cleanliness, Obedience };//for stats

    public enum FoodTypes { Pup, Adult, Senior };
    private int foodType;


    public bool interrupted;
    public int rotationSpeed;

    Vector3 goalPos;

    public Animator animator;

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
    public bool isWandering = true;
    public bool attentionGiven = false;

    GameObject grid;
    public Grid gridScript;

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

    public virtual void Lerping(Node end)
    {
        //to get start node
        Node currentNode = gridScript.coordToNode(transform.position);
        //to pathfind
        List<Node> path = new List<Node>();
        path = Pathfinding(PathfindingTypes.BFS, currentNode, end);
        for (int i = 0; i < path.Count; i++)
        {
            print(path[i].coord);
        }
        for (int i= 0; i < path[1].connectedNodes.Count; i++)
        {
            print("current nodes: " + path[1].coord + "connected nodes" + path[1].connectedNodes[i].coord);
        }
        //for lerping
        StartCoroutine(DogLerp(path));
        isLerping = true;
    }

    IEnumerator DogLerp(List<Node> path)
    {

        print("pathLength = " + path.Count);
        //pops to get current node
        Node currentNode = path[0];
        path.Remove(currentNode);
        Vector3 current = currentNode.coord;

        while (path.Count > 0)
        {
            //pops to get target
            Node targetNode = path[0];
            path.Remove(targetNode);
            Vector3 target = targetNode.coord;
            
            float totalTime = 1;
            float currentTime = 0;

            while (currentTime < totalTime)
            {
                currentTime += Time.deltaTime;
                transform.LookAt(target);
                transform.position = Vector3.Lerp(current, target, currentTime / totalTime);
                if (currentTime >=  totalTime)
                    isLerping = false;

                yield return 0;

            }
        }

    }



    /// <summary>
    /// uses either AStar or BFS
    /// </summary>
    /// <param name="pathfindingTypes"></param>
    /// <param name="startNode"></param>
    /// <param name="endNode"></param>
    /// <returns></returns>
    public List<Node> Pathfinding(PathfindingTypes pathfindingTypes, Node startNode, Node endNode)
    {
        switch (pathfindingTypes)
        {
            case PathfindingTypes.AStar:
                List<Node> openList = new List<Node>();
                List<Node> closedList = new List<Node>();

                openList.Add(startNode);

                while (openList.Count > 0)
                {
                    openList.Sort();
                    Node currentNode = openList[0];
                    openList.Remove(currentNode);
                    closedList.Add(currentNode);


                    if (currentNode == endNode)
                    {
                        GetFoundPath(endNode);
                    }

                    List<Node> connectedNodes = currentNode.connectedNodes;

                    int connectedNodesCount = connectedNodes.Count;
                    for (int i = 0; i < connectedNodesCount; i++)
                    {
                        Node connectedNode = connectedNodes[i];
                        if (closedList.Contains(connectedNode))
                            continue;

                        int g = currentNode.g + 10;
                        int h = EuclideanDistanceHeuristic((int)connectedNode.coord.x, (int)connectedNode.coord.z, (int)endNode.coord.x, (int)endNode.coord.z);
                        int f = g + h;
                        if (f <= connectedNode.f || !(openList.Contains(connectedNode)))
                        {

                            connectedNode.g = g;
                            connectedNode.f = f;
                        }

                        if (!(openList.Contains(connectedNode)))
                        {
                            connectedNode.parent = currentNode;
                            openList.Add(connectedNode);
                        }
                    }
                }

                Debug.Log("Path not found!");
                return GetFoundPath(null);

            case PathfindingTypes.BFS:
                startNode.visited = true;

                Queue<Node> nodesStack = new Queue<Node>();
                nodesStack.Enqueue(startNode);


                while (nodesStack.Count > 0)
                {
                    //if (interrupted)
                    //    break;


                    Node currentNode = nodesStack.Dequeue();


                    if (currentNode == endNode)
                    {

                        return GetFoundPath(endNode);
                    }

                    List<Node> connectedNodes = currentNode.connectedNodes;
                    int connectedNodesCount = connectedNodes.Count;
                    for (int connectedNodesIndex = 0; connectedNodesIndex < connectedNodesCount; ++connectedNodesIndex)
                    {
                        Node connectedNode = connectedNodes[connectedNodesIndex];
                        if (!connectedNode.visited)
                        {
                            connectedNode.visited = true;
                            connectedNode.parent = currentNode;

                            nodesStack.Enqueue(connectedNode);
                        }
                    }
                }
                Debug.Log("Path not found!");
                return GetFoundPath(null);
        }
        Debug.Log("Path not found!");
        return GetFoundPath(null);

    }

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

    protected List<Node> GetFoundPath(Node endNode)
    {
        List<Node> foundPath = new List<Node>();
        if (endNode != null)
        {
            foundPath.Add(endNode);

            while (endNode.parent != null)
            {
                foundPath.Add(endNode.parent);
                endNode = endNode.parent;
            }

            foundPath.Reverse();
        }
        return foundPath;
    }

    void LookatTarget()
    {

    }

    /*
         * 
         * 
         * TEMP
         * 
         * 
         * 
         * 
         */

public void TempWander(Vector3 end)
    {
        isLerping = true;
        transform.LookAt(end);
        StartCoroutine(TempLerp(transform.position, end));
    }

    
    IEnumerator TempLerp(Vector3 start, Vector3 end)
    {
        float totalTime = 5;
        float currentTime = 0;
        while (currentTime < totalTime)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, currentTime / totalTime);

            if (currentTime >= totalTime)
                isLerping = false;
            yield return 0;
        }
    }
}
