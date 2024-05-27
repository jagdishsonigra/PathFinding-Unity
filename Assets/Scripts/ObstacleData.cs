using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObject to store data about obstacle positions on the grid
[CreateAssetMenu(fileName = "ObstacleData", menuName = "ScriptableObjects/ObstacleData", order = 1)]
public class ObstacleData : ScriptableObject
{
    public bool[] obstaclePositions = new bool[100]; // 10x10 grid stored as a flat array
}


