using UnityEngine;
public enum BGSide
{
    Left,
    Right,
    Both
}

[CreateAssetMenu(fileName = "BGTileData", menuName = "ScriptableObjects/BGTileData", order = 1)]
public class BGTileData : ScriptableObject
{
    public GameObject prefab;
    public int weight = 1;
    public int lengthInTiles = 1;
    public BGSide side = BGSide.Both;
    public float worldLength;


    public float GetWorldLength(LVLParameters levelParams)
    {
        return lengthInTiles * levelParams.tileLength;
    }
}