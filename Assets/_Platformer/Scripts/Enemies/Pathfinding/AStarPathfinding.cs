using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AStarPathfinding : MonoBehaviour
{
    [SerializeField] private float adjacentRange;
    public NodeGraph nodeGraph; //reference to node graph 
    public bool drawPath; //draws node path for debugging

    private List<Node> nodesToDraw = new List<Node>();

    public List<Node> CalculatePath(Vector3 startPos, Vector3 endPos)
    {
        Node startNode = nodeGraph.GetNearestNode(startPos);
        Node endNode = nodeGraph.GetNearestNode(endPos);

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(startNode);
        System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
        st.Start();

        while (openList.Count > 0)
        {
            Node currentNode = openList[openList.Count - 1];

            openList.RemoveAt(openList.Count - 1);
            closedList.Add(currentNode);

            if (currentNode == endNode)
            {
                st.Stop();
                Debug.Log("Pathfinding Complete : " + st.ElapsedMilliseconds.ToString("F3"));
                return RetracePath(startNode, endNode);              
            }

            List<Node> adjacentNodes = new List<Node>();

            foreach (Node adjacentNode in nodeGraph.GetAdjacentNodes(currentNode))
            {
                if (!adjacentNode.isWalkable || closedList.Contains(adjacentNode))
                {
                    continue;
                }

                if (!openList.Contains(adjacentNode))
                {
                    adjacentNode.nodeParent = currentNode;
                    adjacentNode.gCost = currentNode.gCost + GetManDist(adjacentNode, currentNode);
                    adjacentNode.hCost = GetManDist(adjacentNode, endNode);
                    adjacentNodes.Add(adjacentNode);
                }
            }

            adjacentNodes.Sort((x, y) => y.fCost - x.fCost);

            openList = MergeLists(openList, adjacentNodes);
        }

        return new List<Node>();
    }
    public int GetManDist(Node start, Node end)
    {
        //calculate heuristic using Manhattan distance 
        return (int)(Mathf.Abs(start.worldPos.x - end.worldPos.x) + Mathf.Abs(start.worldPos.y - end.worldPos.y));
    }

    public List<Node> MergeLists(List<Node> openList, List<Node> adjacentNodes)
    {
        List<Node> result = new List<Node>();

        int i = 0, j = 0;

        while (i < openList.Count || j < adjacentNodes.Count)
        {
            if (i >= openList.Count)
            {
                result.Add(adjacentNodes[j]);
                j++;
                continue;
            }
            if (j >= adjacentNodes.Count)
            {
                result.Add(openList[i]);
                i++;
                continue;
            }

            if (openList[i].fCost > adjacentNodes[j].fCost)
            {
                result.Add(openList[i]);
                i++;
            }
            else
            {
                result.Add(adjacentNodes[j]);
                j++;
            }
        }

        return result;
    }

    public List<Node> RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node current = end;

        //retrace path by iterating through node parents
        while (current != start)
        {
            path.Add(current);
            current = current.nodeParent;
        }

        //reverse list to put nodes in correct order
        path.Reverse();

        if (drawPath)
        {
            nodesToDraw = new List<Node>(path);
        }

        return path;
    }

    private void OnDrawGizmos()
    {
        //Draw the path in yellow if debugging
        if (drawPath)
        {
            Gizmos.color = Color.yellow;
            if (nodesToDraw.Count > 0)
            {
                foreach (Node n in nodesToDraw)
                {
                    //Gizmos.DrawWireCube(n.worldPos, new Vector3(1, 1, 1) * nodeGrid.nodeSize);
                }
            }
        }
    }
}

