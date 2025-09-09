using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData", menuName = "ScriptableObjects/ObstacleData")]

public class ObstacleData : ScriptableObject
{
    public GameObject prefab;
    public int weight;
}