using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ChunkData")]
public class ChunkData : ScriptableObject
{
    public GameObject chunkPrefab;
    public BGTileData overridBeckGround;
    public int lengthInTiles = 5;
    public int weight = 1;

    public float GetWorldLength(LVLParameters levelParams)
    {
        return lengthInTiles * levelParams.tileLength;
    }
}
