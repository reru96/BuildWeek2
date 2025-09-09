using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ChunkData")]
public class ChunkData : ScriptableObject
{
    public GameObject chunkPrefab;
    public int lengthInTiles;
    public int weight;
}