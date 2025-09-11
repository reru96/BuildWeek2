using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PowerUpData", menuName = "ScriptableObjects/PowerUpData")]

public class CollectableData : ScriptableObject
{
    public string namePowerUp;
    public GameObject prefab;
    public int weight;
    public int cost;
    public Sprite icon;

    // da non spuntare in Inspector
    public bool shopped;
}
