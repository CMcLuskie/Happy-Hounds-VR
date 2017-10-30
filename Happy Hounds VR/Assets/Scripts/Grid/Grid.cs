using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public List<Vector3> nodeCoords;
    public List<Vector3> blockedNodes;
    public Node[] nodes;

    int lengthX = 28;
    int lengthZ = 28;

    public float dogYPos;

    public bool showNodes;

    private void Start()
    {
        InstantiateGrid();
        
        if (showNodes)
            ShowNodes();
    }

    // lengthX = 3
    // lengthZ = 5
    // 0,  1,  2,  3,  4,
    // 5,  6,  7,  8,  9,
    // 10, 11 12, 13, 14,

    // 0,  1,  2,  3,  4, 5,  6,  7,  8,  9, 10, 11 12, 13, 14,

    /// <summary>
    /// converts world position to closest node
    /// </summary>
    /// <param name="coord"></param>
    /// <returns></returns>
    public Node coordToNode(Vector3 coord)
    {
        int x = (int)coord.x + 14;
        int z = (int)coord.z + 14;
        int nodeIndex = z + x * lengthZ;
        return nodes[nodeIndex];
    }

    /// <summary>
    /// gets traversable area in vector 3
    /// </summary>
    void InstantiateGrid()
    {
        for (int i = 0; i < lengthX; i++)
        {
            for(int j= 0; j < lengthZ; j++)
            {
                nodeCoords.Add(new Vector3(14 - i, dogYPos, 14 - j));
            }
        }

        foreach (Vector3 v in blockedNodes)
            nodeCoords.Remove(v);

        InstantiateNodes();
    }

    /// <summary>
    /// translates vectors 3 into nodes
    /// </summary>
    void InstantiateNodes()
    {
        nodes = new Node[nodeCoords.Count];
        for(int i=0; i < nodeCoords.Count; i++)
        {
            Node node = new Node();
            node.coord = nodeCoords[i];
            nodes[i] = node;
        }
    }

    /// <summary>
    /// used for visualisation of nodes for debugging
    /// </summary>
    void ShowNodes()
    {
        foreach(Vector3 v in nodeCoords)
        {
            GameObject nodeObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            nodeObject.transform.position = v;
        }
    }

    /// <summary>
    /// returns a random node
    /// </summary>
    /// <returns></returns>
    public Node GetRandomNode()
    {
        int i = Random.Range(0, nodes.Length);
        return nodes[i];
    }
}
