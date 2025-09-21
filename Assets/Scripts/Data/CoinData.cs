using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CoinData", menuName = "ScriptableObjects/CoinData", order = 1)]
public class CoinData : ScriptableObject, IWeighted
{
    public string coinName;
    public GameObject prefab;  
    public int value = 1;
    public int weight = 0;
    public int minDifficult;
    public int maxDifficult;

    public GameObject Prefab => prefab;
    public int Weight => weight;


}