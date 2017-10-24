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
            nodes.Add(new Vector3(-18 + i, 1, 3));
    }

    void InstantiateNodesZ()
    {
        for (int i = 0; i < indoorLengthZ; i++)
            nodes.Add(new Vector3(4, 1, 3 - indoorLengthZ));
        for (int j = 0; j < outdoorLengthZ; j++)
            nodes.Add(new Vector3(-18, 1, -3 - outdoorLengthZ));
    }

    public Vector3  GetRandomNode()
    {
        int i = Random.Range(0, nodes.Count);
        Debug.Log(nodes[i]);
        return nodes[i];
    }
}
