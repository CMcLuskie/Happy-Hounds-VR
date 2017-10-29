using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public Vector3 coord;

    public Node parent;

    public int f;
    public int g;
    public int h;

    public List<Node> ConnectedNodes(Vector3 current)
    {
        List<Node> connected = new List<Node>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Node node = new Node();
                node.coord = new Vector3
                    (current.x - 1 + i, current.y, current.z - 1 + j);

                if (node.coord == current)
                    connected.Remove(node);
            }
        }
        return connected;
    }
}
