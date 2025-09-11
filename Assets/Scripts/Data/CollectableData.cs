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
    public int minDifficultyLevel;
    public int maxDifficultyLevel;
    public Sprite icon;

    // da non spuntare in Inspector
    public bool shopped;

    public virtual void Use(GameObject obj)
    {
        
    }
}
