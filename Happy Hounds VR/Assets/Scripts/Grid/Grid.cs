﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public List<Vector3> nodeCoords;
    public List<Vector3> blockedNodes;
    public Node[] nodes;

    [SerializeField]
    protected int lengthX = 28;
    [SerializeField]
    protected int lengthZ = 28;
    [SerializeField]
    protected Vector3 gridCorner;

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
        coord.x = Mathf.Round(coord.x);
        coord.y = dogYPos;
        coord.z = Mathf.Round(coord.z);

        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i].coord == coord)
            {
                return nodes[i];
            }
        }
        return nodes[0];
    }

    /// <summary>
    /// gets traversable area in vector 3
    /// </summary>
    void InstantiateGrid()
    {
        for (int i = 0; i < lengthX; i++)
        {
            for (int j = 0; j < lengthZ; j++)
            {
                nodeCoords.Add(new Vector3(gridCorner.x - i, dogYPos, gridCorner.z - j));
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
        for (int i = 0; i < nodeCoords.Count; i++)
        {
            Node node = new Node();
            node.coord = nodeCoords[i];
            nodes[i] = node;
            print("Node" + i + nodes[i].coord);
        }

        for (int i = 0; i < nodes.Length; i++)
        {
            Node node = nodes[i];
            for (int x = 0; x < 3; x++)
            {
                for (int z = 0; z < 3; z++)
                {
                    Vector3 neighbourPos = node.coord;
                    neighbourPos.x += x - 1;
                    neighbourPos.z += z - 1;

                    Node neighbour = coordToNode(neighbourPos);
                    if (node != null)
                    {
                        node.connectedNodes.Add(neighbour);
                    }
                }
            }
        }
    }



    /// <summary>
    /// used for visualisation of nodes for debugging
    /// </summary>
    void ShowNodes()
    {
        foreach (Vector3 v in nodeCoords)
        {
            float sphereScale = .5f;
            GameObject nodeObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            nodeObject.transform.localScale = new Vector3(sphereScale, sphereScale, sphereScale);
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
        print("fucking last point: " + nodes[i].coord);
        return nodes[i];
    }

    //this is used for debugging only
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            for (int i = 0; i < nodes[439].connectedNodes.Count; i++)
                print("Node 439 neighbour " + i + ": " + nodes[439].connectedNodes[i].coord); 
                    }
}
