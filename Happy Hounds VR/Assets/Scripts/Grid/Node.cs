using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public Vector3 coord;

    public Node parent;

    public int f = 0;
    public int g = 0;
    public int h = 0;

    public List<Node> ConnectedNodes()
    {
        List<Node> connected = new List<Node>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Node node = new Node();
                node.coord = new Vector3
                    (coord.x - 1 + i, coord.y, coord.z - 1 + j);

                if (node.coord == coord)
                    connected.Remove(node);
            }
        }
        return connected;
    }
}
