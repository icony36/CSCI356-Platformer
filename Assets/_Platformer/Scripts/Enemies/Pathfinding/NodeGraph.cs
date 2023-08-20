using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class NodeGraph : MonoBehaviour
{
    //=========================================================//
    //[SerializeField] List<Node> nodes;
    [SerializeField] private bool showGrid = false;
    //=========================================================//
    //private int gridX, gridY; //length and width of grid relative to size of grid nodes      
    //private Vector3 gridStartPos; //origin point of the grid, calculated based on the grid size
    //private float gridOffset;

    private void Start()
    {
        ConstructGraph();
    }

    public void ConstructGraph()
    {
        foreach(Transform child in transform) 
        {
            if(child.gameObject.GetComponentInChildren<BoxCollider>() != null)
            {

            }
        }
    }

    public List<Node> GetAdjacentNodes(Node node)
    {
        List<Node> adjacentNodes = new List<Node>();

        //int x = node.gridPos.x;
        //int y = node.gridPos.y;

        ////checking for edge cases
        //bool left = x != 0;
        //bool right = x != gridX - 1;
        //bool down = y != 0;
        //bool up = y != gridY - 1;

        ////add adjacent nodes to list
        //if (left)
        //{
        //    adjacentNodes.Add(nodes[x - 1, y]);
        //}
        //if (right)
        //{
        //    adjacentNodes.Add(nodes[x + 1, y]);
        //}
        //if (down)
        //{
        //    adjacentNodes.Add(nodes[x, y - 1]);
        //}
        //if (up)
        //{
        //    adjacentNodes.Add(nodes[x, y + 1]);
        //}

        //if (diagonalMovement)
        //{
        //    if (left)
        //    {
        //        if (down)
        //        {
        //            if (nodes[x - 1, y].isWalkable && nodes[x, y - 1].isWalkable)
        //            {
        //                adjacentNodes.Add(nodes[x - 1, y - 1]);
        //            }
        //        }
        //        if (up)
        //        {
        //            if (nodes[x - 1, y].isWalkable && nodes[x, y + 1].isWalkable)
        //            {
        //                adjacentNodes.Add(nodes[x - 1, y + 1]);
        //            }
        //        }
        //    }
        //    if (right)
        //    {
        //        if (down)
        //        {
        //            if (nodes[x + 1, y].isWalkable && nodes[x, y - 1].isWalkable)
        //            {
        //                adjacentNodes.Add(nodes[x + 1, y - 1]);
        //            }
        //        }
        //        if (up)
        //        {
        //            if (nodes[x + 1, y].isWalkable && nodes[x, y + 1].isWalkable)
        //            {
        //                adjacentNodes.Add(nodes[x + 1, y + 1]);
        //            }
        //        }
        //    }
        //}

        return adjacentNodes;
    }

    public Node GetNearestNode(Vector3 pos)
    {
        return null;
    }

    public void ClearList()
    {

    }

    private void OnDrawGizmos()
    {
        //if (showGrid && nodes != null)
        //{
        //    //outlines unwalkable tiles in red, walkable tiles in white
        //    foreach (Node n in nodes)
        //    {
        //        if (n.isWalkable)
        //        {
        //            Gizmos.color = Color.white;
        //        }
        //        else
        //        {
        //            Gizmos.color = Color.red;
        //        }

        //        Gizmos.DrawWireCube(n.worldPos, new Vector3(1, 1, 1) * nodeSize);
        //    }
        //}
    }
}

