using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(GameManager))]
public class GameManager_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameManager gameManager = (GameManager)target;

        if (GUILayout.Button("Assign Enemy ID"))
        {
            gameManager.AssignEnemyID();
        }

        if (GUILayout.Button("Assign Powerup ID"))
        {
            gameManager.AssignPowerupID();
        } 
    }
}
