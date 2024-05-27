using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObstacleData))]
public class ObstacleEditor : Editor
{
    private bool[] obstaclePositions;

    void OnEnable()
    {
        obstaclePositions = ((ObstacleData)target).obstaclePositions;
    }

    public override void OnInspectorGUI()
    {
        if (obstaclePositions.Length != 100)
        {
            obstaclePositions = new bool[100];
        }

        GUILayout.Label("Obstacle Grid", EditorStyles.boldLabel);

        for (int y = 0; y < 10; y++)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < 10; x++)
            {
                int index = y * 10 + x;
                obstaclePositions[index] = GUILayout.Toggle(obstaclePositions[index], "");
            }
            GUILayout.EndHorizontal();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
