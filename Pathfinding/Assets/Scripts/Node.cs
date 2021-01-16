using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Node parent;

    public bool walkable;
    public Vector3 worldPos;

    public int gCost;
    public int hCost;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public Node(bool _walkable, Vector3 _worldPos)
    {
        walkable = _walkable;
        worldPos = _worldPos;
    }
}
