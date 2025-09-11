using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PowerUpData", menuName = "ScriptableObjects/PowerUpData")]

public class CollectableData : ScriptableObject,IWeighted
{
    public string namePowerUp;
    public GameObject prefab;
    public int weight;
    public int cost;
    public int minDifficultyLevel;
    public int maxDifficultyLevel;
    public Sprite icon;

    public int maxDifficult;
    public int minDifficult;

    public GameObject Prefab => prefab;
    public int Weight => weight;

    // da non spuntare in Inspector
    public bool shopped;

    public virtual void Use(GameObject obj)
    {
        
    }
}
