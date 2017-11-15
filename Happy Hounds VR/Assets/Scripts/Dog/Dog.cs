﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public abstract class Dog : MonoBehaviour
{

    #region Variables
        //Movement
        public enum Direction { Forward, Back, Left, Right }; //for movement
        public enum PathfindingTypes { AStar, BFS};
        public bool interrupted;
        public bool isLerping = false;
        public bool isWandering = true;
        public bool attentionGiven = false;
        public int rotationSpeed;
        Vector3 goalPos;

        //Stats
        public enum Stats { Happiness, Hunger, Thirst, Cleanliness, Obedience };//for stats
        float happiness;
        float hunger;
        float thirst;
        float cleanliness;
        float obedience;

        //age
        [SerializeField]
        protected bool pup;
        [SerializeField]
        protected bool adult;
        [SerializeField]
        protected bool senior;

        //Food Type
        public enum FoodTypes { Pup, Adult, Senior };
        private int foodType;

        //GameObjects and components
        public Animator animator;
        GameObject grid;
        public Grid gridScript;
        [SerializeField]
         protected Transform player;
    #endregion

    #region Stats
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
    #endregion

    #region Movement
    public virtual void Move(Direction dir, float speed)
    {
        switch (dir)
        {
            //But what's your opinion on the death rate of bees
            case Direction.Forward:
                transform.localPosition += Vector3.forward * speed;
                break;
            case Direction.Back:
                transform.localPosition += Vector3.back * speed;
                break;
            case Direction.Left:
                transform.localPosition += Vector3.left * speed;
                break;
            case Direction.Right:
                transform.localPosition += Vector3.right * speed;
                break;
        }
    }

    #region Steering Behaviour

    public void GoToPoint(Vector3 pos, float speed)
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
#endregion 


    #region Lerping
    public virtual void Lerping(Node end)
    {
        //to get start node
        Node currentNode = gridScript.coordToNode(transform.position);
        print(currentNode.coord);
        //to pathfind
        List<Node> path = new List<Node>();
        path = Pathfinding(PathfindingTypes.BFS, currentNode, end);
        //for lerping
        StartCoroutine(DogLerp(path));
        isLerping = true;
    }

    IEnumerator DogLerp(List<Node> path)
    {

        print("pathLength = " + path.Count);        
        while (path.Count > 0)
        {
            //this is to avoid error when 
            if (path.Count == 1)
                break;

            Node currentNode = path[0];
            path.Remove(currentNode);
            Vector3 current = currentNode.coord;
            //pops to get target
            Node targetNode = path[0];
            Vector3 target = targetNode.coord;
            
            float totalTime = 1;
            float currentTime = 0;

            while (currentTime < totalTime)
            {
                currentTime += Time.deltaTime;
                transform.LookAt(target);
                transform.position = Vector3.Lerp(current, target, currentTime / totalTime);
                if (currentTime >= totalTime && path.Count == 1)
                {
                    isLerping = false;
                    path.Remove(targetNode);
                }
                

                yield return 0;

            }
        }

    }
#endregion

    #region Pathfinding
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

    #endregion

    #endregion

    #region Player

        public Vector3 PlayerPos()
        {
            Vector3 playerFeet = new Vector3();
            playerFeet = player.position;
            playerFeet.y = 0.22f;
            return playerFeet;
        }
        #endregion

    #region Bug Prevention

    /// <summary>
    /// this resets all 3 coords, and rotation of the dog
    /// </summary>
    public  void ResetPosition()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        Quaternion quar = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        transform.SetPositionAndRotation(new Vector3(-10, 0, -10), quar);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    public void ResetYPosition()
    {
        Vector3 fix = new Vector3(transform.position.x, 0, transform.position.z);
        transform.position = fix;
    }

    #endregion

    #region Temp
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
#endregion
}
