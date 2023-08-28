using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NodeGraph))]
public class NodeGraph_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NodeGraph aStarGrid = (NodeGraph)target;

        if(GUILayout.Button("Construct Graph"))
        {
            aStarGrid.ConstructGraph();
        }

        if (GUILayout.Button("Clear Graph"))
        {
            aStarGrid.ClearList();
        }

        if (GUILayout.Button("Test"))
        {
            aStarGrid.Test();
        }
    }
}
