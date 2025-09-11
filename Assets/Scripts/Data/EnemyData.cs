using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject, IWeighted
{
    public GameObject prefab;
    public int weight;
    public int minDifficultyLevel;
    public int maxDifficultyLevel;

    public GameObject Prefab => prefab;
    public int Weight => weight;
}