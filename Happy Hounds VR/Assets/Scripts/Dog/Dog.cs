using System;
using System.Collections;
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
    [HideInInspector]
    public bool inDogHouse;
    [SerializeField]
    protected Transform dogHouseExit;
    [HideInInspector]
    public bool inPool;
    [SerializeField]
    protected Transform poolExit;

    //Stats
    public enum Stats { Happiness, Hunger, Thirst, Cleanliness, Obedience, Energy };//for stats
     public List<float> statList;
     public enum StatDepletion { Happiness, Hunger, Thirst, Cleanliness, Obedience, Energy };
     public List<float> statDepletions;

    //age
    [SerializeField]
    protected enum Age { Pup, Adult, Senior};

    public float idleTimer;

    [SerializeField]
    protected bool demoDog;

   
    //GameObjects and components
    [SerializeField]
    protected Animator animator;
    GameObject grid;
    public Grid gridScript;
    [SerializeField]
    protected Transform player;
    public  List<GameObject> costumeList;
    public List<String> costumeStringList;
    protected enum Costumes { Wings, Crown, Glasses, Hat};
    #endregion

    #region Stats
    public float GetDogStats(Stats stats)
    {
        switch (stats)
        {
            case Stats.Happiness:
                if (statList[(int)Stats.Happiness] > 100)
                {
                    statList[(int)Stats.Happiness] = 100;
                }
                return statList[(int)Stats.Happiness];
            case Stats.Hunger:
                if (statList[(int)Stats.Hunger] > 100)
                {
                    statList[(int)Stats.Hunger] = 100;
                }
                return statList[(int)Stats.Hunger];
            case Stats.Thirst:
                if (statList[(int)Stats.Thirst] > 100)
                {
                    statList[(int)Stats.Thirst] = 100;
                }
                return statList[(int)Stats.Thirst];
            case Stats.Cleanliness:
                if (statList[(int)Stats.Cleanliness] > 100)
                {
                    statList[(int)Stats.Cleanliness] = 100;
                }
                return statList[(int)Stats.Cleanliness];
            case Stats.Obedience:
                if (statList[(int)Stats.Obedience] > 100)
                {
                    statList[(int)Stats.Obedience] = 100;
                }
                return statList[(int)Stats.Obedience];

            case Stats.Energy:
                if (statList[(int)Stats.Energy] > 100)
                {
                    statList[(int)Stats.Energy] = 100;
                }
                return statList[(int)Stats.Energy];                
        }
        return 0;
    }

    void InitialiseStatList()
    {
        statList = new List<float>();
        for (int i = 0; i < 6; i++)
            statList.Add(0);
    }
    public void InitialiseStats(float total)
    {
        InitialiseStatList();
        if (demoDog)
            DemoDogStats();
        else
            RandomDogStats(total);
        PrintStats();
    }

    private void DemoDogStats()
    {
        statList[(int)Stats.Happiness] = 80;
        statList[(int)Stats.Hunger] = 80;
        statList[(int)Stats.Thirst] = 80;
        statList[(int)Stats.Cleanliness] = 100;
        statList[(int)Stats.Obedience] = 100;
        statList[(int)Stats.Energy] = 100;
    }

    private void RandomDogStats(float total)
    {
        for (int i = 0; i < statList.Count; i++)
        {
            if (total <= 50)
                statList[i] = UnityEngine.Random.Range(0, total);
            else
                statList[i] = UnityEngine.Random.Range(0, 75);

            if (statList[i] < 20)
                statList[i] += 20;

            if (total - statList[i] < 0)
                total -= statList[i];
        }
    }
    private void PrintStats()
    {
        print("happiness: " + statList[(int)Stats.Happiness]);
        print("hunger: " + statList[(int)Stats.Hunger]);
        print("thirst: " + statList[(int)Stats.Thirst]);
        print("cleanliness: " + statList[(int)Stats.Cleanliness]);
        print("obedience: " + statList[(int)Stats.Obedience]);
    }

    public bool isHungry()
    {

        if ((statList[(int)Stats.Hunger] < 50)
            && (statList[(int)Stats.Hunger] < statList[(int)Stats.Thirst]))
            return true;

        else return false;
    }

    public bool isThirsty()
    {
        if ((statList[(int)Stats.Thirst] < 50) 
            && (statList[(int)Stats.Thirst] < statList[(int)Stats.Hunger]))
            return true;

        else return false;
    }
    #endregion

    #region Decision Check

    public bool DecisionCheck(Stats stats, float parameter)
    {

        if (GetDogStats(stats) == 100)
            return true;

        float check = UnityEngine.Random.Range(0, 10) * GetDogStats(stats);

        if (check >= parameter)
            return true;
        else return false;
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

    /// <summary>
    /// distance is how close to point it should get
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="speed"></param>
    /// <param name="distance"></param>
    public void GoToPoint(Vector3 pos, float speed, float distance)
    {
        #region collision avoidance
        if ((inDogHouse) &&!(isLerping))
        {
            isLerping = true;
            StartCoroutine(LeaveArea(1, transform.position, dogHouseExit.position));            
        }

#endregion

        if (idleTimer == 0)
        {
            transform.LookAt(pos);
            Quaternion quar = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
            transform.SetPositionAndRotation(transform.position, quar);

            if (!ClosetoPoint(transform.position, pos, distance))
            {
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
            if (ClosetoPoint(transform.position, pos, distance))
                animator.SetFloat("Move", 0f);
        }
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

    public bool ClosetoPoint(Vector3 current, Vector3 point, float finalDist)
    {
        Vector3 direction = point - current;
        float distance = direction.magnitude;


        if (distance < finalDist)
            return true;
        else
        return false;
    }

    IEnumerator LeaveArea(float timeTaken,Vector3 startPos, Vector3 endPos)
    {
        float currentTime = 0;
        while (currentTime <= timeTaken)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, currentTime / timeTaken);
            yield return 0;
        }
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

    #region Costumes

    public void InitCostumeList()
    {
        print("here");
        costumeList[(int)Costumes.Wings] = GameObject.FindGameObjectWithTag("Wings");
        costumeList[(int)Costumes.Crown] = GameObject.FindGameObjectWithTag("Crown");
        costumeList[(int)Costumes.Glasses] = GameObject.FindGameObjectWithTag("Glasses");
        costumeList[(int)Costumes.Hat] = GameObject.FindGameObjectWithTag("Hat");

        costumeStringList = new List<string>();
        for (int i = 0; i < costumeList.Count - 1; i++)
        {
            costumeStringList.Add(costumeList[i].name);
        }
    }

    /// <summary>
    /// will put a costume ont he dog
    /// </summary>
    /// <param name="costume"></param>
    public void ActivateCostume(string costume)
    {
        switch (costume)
        {
            case "Wings":
                costumeList[(int)Costumes.Wings].SetActive(true);
                break;
            case "Crown":
                costumeList[(int)Costumes.Crown].SetActive(true);
                break;
            case "Glasses":
                costumeList[(int)Costumes.Glasses].SetActive(true);
                break;
            case "Hat":
                costumeList[(int)Costumes.Hat].SetActive(true);
                break;
        }
    }

    /// <summary>
    /// will take a costume off the dog
    /// </summary>
    /// <param name="costume"></param>
    public void DeactivateCostume(string costume)
    {
        switch (costume)
        {
            case "Wings":
                costumeList[(int)Costumes.Wings].SetActive(false);
                break;
            case "Crown":
                costumeList[(int)Costumes.Crown].SetActive(false);
                break;
            case "Glasses":
                costumeList[(int)Costumes.Glasses].SetActive(false);
                break;
            case "Hat":
                costumeList[(int)Costumes.Hat].SetActive(false);
                break;
        }
    }
    #endregion

    #region Triggers

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Kennel")
            inDogHouse = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Kennel")
            inDogHouse = false;
    }
    #endregion
}





