using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;


public class NodeGraph : MonoBehaviour
{
    //=========================================================//
    //[SerializeField] List<Node> nodes;
    //[SerializeField] private bool showGrid = false;
    //=========================================================//
    //private int gridX, gridY; //length and width of grid relative to size of grid nodes      
    //private Vector3 gridStartPos; //origin point of the grid, calculated based on the grid size
    //private float gridOffset;

    [SerializeField] private List<GameObject> nodeObjects; 

    private List<Node> graphNodes = new List<Node>();

    private void Start()
    {
        ConstructGraph(); 
    }

    public void ConstructGraph()
    {
        //int id = 0;
        //foreach(Transform section in platformSections)
        //{
        //    foreach (Transform child in section)
        //    {
        //        if (child.transform.GetComponentInChildren<BoxCollider>() != null)
        //        {
        //            float extents = child.transform.GetComponentInChildren<BoxCollider>().bounds.extents.x;

        //            Vector3 leftEdge = child.transform.GetComponentInChildren<BoxCollider>().bounds.center - new Vector3(extents - 0.5f, 0, 0);

        //            for (int i = 0; i < (int)extents * 2; i++)
        //            {
        //                Node node = new Node(true, leftEdge + new Vector3(i, 1f, 0), id);
        //                nodePositions.Add(node.worldPos);
        //                graphNodes.Add(node);
        //                id++;
        //            }
        //        }
        //    }
        //}  
    }

    //public List<Node> GetAdjacentNodes(Node node)
    //{
    //    Debug.Log(graphNodes.Count);
    //    Debug.Log(node.id);
    //    List<Node> adjacentNodes = new List<Node>();

    //    if (node.id - 1 > 0)
    //        adjacentNodes.Add(graphNodes[node.id - 1]);

    //    if (node.id + 1 < graphNodes.Count)
    //        adjacentNodes.Add(graphNodes[node.id + 1]);

        

    //    return adjacentNodes;
    //}

    public Node GetNearestNode(Vector3 pos)
    {
        return null;
    }

    public void ClearList()
    {
        graphNodes.Clear();
    }

    public void Test()
    {
        GetComponent<AStarPathfinding>().CalculatePath(graphNodes[0], graphNodes[13]);
    }

    private void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        //Gizmos.color = Color.yellow;

        //foreach(Vector3 pos in nodePositions)
        //{
        //    Gizmos.DrawWireSphere(pos, 0.5f);
        //}    
    }
}

