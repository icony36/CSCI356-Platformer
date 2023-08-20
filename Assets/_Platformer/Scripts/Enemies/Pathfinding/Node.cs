using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO : Add additional properties for use in differentating different node levels
public class Node
{
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
    public int gCost; //distance from start point
    public int hCost; //distance from end point
    //=========================================================//
    public Vector3 worldPos; //position in world space
    public bool isWalkable = true;
    //=========================================================//
    public List<Node> adjacentNodes;
    public Node nodeParent;

    public Node(bool walkable, Vector3 _worldPos)
    {
        isWalkable = walkable;
        worldPos = _worldPos;
    }
}

