using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeData", menuName = "ScriptableObjects/BiomeData", order = 2)]

public class SO_BiomeData : ScriptableObject
{
    public BIOMETIPE biomeType;


    public List<SO_TileData> aviabletileDatas;

    public List<GameObject> enemy
    public List<GameObject> obstacles;


    public Material tileMaterial;
    public AudioClip biomeMusic;
    public Color fogColor;
    public Color ambientLightColor;


}
