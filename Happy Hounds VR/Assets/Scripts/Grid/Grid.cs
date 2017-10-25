using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public List<Vector3> nodes;
    public List<Vector3> blockedNodes;

    int lengthX = 28;
    int lengthZ = 28;

    public float dogYPos;

    public bool showNodes;
    private void Start()
    {
        InstantiateNodes();

        if (showNodes)
            ShowNodes();
    } 

    void InstantiateNodes()
    {
        for (int i = 0; i < lengthX; i++)
        {
            for(int j= 0; j < lengthZ; j++)
            {
                nodes.Add(new Vector3(14 - i, dogYPos, 14 - j));
            }
        }

        foreach (Vector3 v in blockedNodes)
            nodes.Remove(v);

    }

    void ShowNodes()
    {
        foreach(Vector3 v in nodes)
        {
            GameObject nodeObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            nodeObject.transform.position = v;
        }
    }

    public Vector3 GetRandomNode()
    {
        int i = Random.Range(0, nodes.Count);
        return nodes[i];
    }
}
