using System;
using System.Collections.Generic;
using UnityEngine;

public class Node :IComparable<Node>
{
    public bool visited;

    public Vector3 coord;

    public Node parent;

    public int f = 0;
    public int g = 0;
    public int h = 0;

    public List<Node> connectedNodes = new List<Node>();

    public int CompareTo(Node node)
    {
        if (f < node.f)
        {
            return -1;
        }
        else if (f > node.f)
        {
            return 1;
        }

        return 0;
    }
}
