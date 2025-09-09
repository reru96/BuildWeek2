using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]

public class EnemyData : ScriptableObject
{
    public GameObject prefab;
    public int weight;
}