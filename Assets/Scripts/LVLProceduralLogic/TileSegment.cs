using System.Collections.Generic;
using UnityEngine;


//COSTRUTTORE DI UN PEZZO DI PISTA, CONTIENE TUTTE LE INFORMAZIONI SUL PEZZO DI PISTA STESSO (TIPO DI CORSIA, OGGETTI SPAWNATI, POSIZIONE Z, BIOME, ECC...)
public class TileSegment
{
    public float ZPosition { get; private set; }


    public GameObject[] LaneObjects { get; private set; } 
    public GameObject[] LanePrefabs { get; private set; } // Mi serve il prefab originale per fare il despawn

    public TILETIPE[] LaneTypes { get; private set; }
    public BiomeData Biome { get; set; }

    public GameObject FloorObject { get; set; }

    public List<(GameObject instance, GameObject prefab)> SpawnedContents { get; private set; }

    public TileSegment(float z, int laneCount)
    {
        ZPosition = z;
        LaneObjects = new GameObject[laneCount];
        LanePrefabs = new GameObject[laneCount];
        LaneTypes = new TILETIPE[laneCount];
        SpawnedContents = new List<(GameObject instance, GameObject prefab)>();

    }
}
