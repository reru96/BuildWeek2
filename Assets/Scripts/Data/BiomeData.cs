using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeData", menuName = "ScriptableObjects/BiomeData", order = 2)]

public class BiomeData : ScriptableObject
{
    public BIOMETIPE biomeType;


    public List<TileData> aviabletileDatas;

    public List<EnemyData> enemies;
    public List<ObstacleData> obstacles;
    public List<CollectableData> collectables;
    //public List<ChunkData> chunks;



    public Material tileMaterial;
    public AudioClip biomeMusic;
    public Color fogColor;
    public Color ambientLightColor;


}
