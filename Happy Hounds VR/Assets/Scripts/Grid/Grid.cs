using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public List<Vector3> nodes;
    public List<Vector3> blockedNodes;

    int lengthX = 24;
    int indoorLengthZ = 8;
    int outdoorLengthZ = 10;

    private void Start()
    {
        InstantiateNodesX();
        InstantiateNodesZ();
    } 

    void InstantiateNodesX()
    {
        for (int i = 0; i < lengthX; i++)
            nodes.Add(new Vector3(-19 + i, 0, 4));
    }

    void InstantiateNodesZ()
    {
        for (int i = 0; i < indoorLengthZ; i++)
            nodes.Add(new Vector3(4, 0, 4 - indoorLengthZ));
        for (int j = 0; j < outdoorLengthZ; j++)
            nodes.Add(new Vector3(-19, 0, -4 - outdoorLengthZ));
    }

    /// <summary>
    /// Returns random point in traversable
    /// </summary>
    public Vector3 GetRandomPoint()
    {
        int i = Random.Range(0, 42);
        
        return nodes[i];
    }
}
