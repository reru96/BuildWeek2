using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PowerUpData", menuName = "ScriptableObjects/PowerUpData")]

public class CollectableData : ScriptableObject
{
    public GameObject prefab;
    public int weight;
}
