using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileData", menuName = "ScriptableObjects/TileData", order = 1)]
public class TileData : ScriptableObject, IWeighted
{
    public TILETIPE tileType;
    public GameObject tilePrefab;
    public int weight;
    public int maxDifficult;
    public int minDifficult;

    public GameObject Prefab => tilePrefab;
    public int Weight => weight;
}
