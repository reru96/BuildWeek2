using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundBuilder : MonoBehaviour
{


    [SerializeField] private Transform backgroundParent;

    private float _lastBackgroundZ = 0f;
    private LVLParameters _levelParams;

    private Queue<GameObject> _activeBackgroundTiles = new();


    public void Initialize(LVLParameters parameters)
    {
        _levelParams = parameters;
    }



    private void SpawnSingleBG(BGTileData bgData, Vector3 basePos)
    {
        float totalWidth = _levelParams.numberOfLanes * _levelParams.laneWidth;
        float offsetX = totalWidth * 0.5f + (_levelParams.laneWidth * 0.5f);

        if (bgData.side == BGSide.Right || bgData.side == BGSide.Both)
        {
            GameObject rightBG = PoolManager.Instance.Spawn(
                bgData.prefab,
                basePos + new Vector3(offsetX, 0, 0),
                Quaternion.identity,
                backgroundParent
            );
            _activeBackgroundTiles.Enqueue(rightBG);
        }

        if (bgData.side == BGSide.Left || bgData.side == BGSide.Both)
        {
            GameObject leftBG = PoolManager.Instance.Spawn(
                bgData.prefab,
                basePos - new Vector3(offsetX, 0, 0),
                Quaternion.identity,
                backgroundParent
            );

            // Se il prefab non è già specchiato, lo specchio io
            leftBG.transform.localScale = new Vector3(-1, 1, 1);

            _activeBackgroundTiles.Enqueue(leftBG);
        }
    }
    public void SpawnBiomeTransition(BGTileData transition, Vector3 position)
    {
        if (transition == null || transition.prefab == null) return;
        float tileCenterOffset = _levelParams.tileLength * 0.5f;
        Vector3 centerPos = position + new Vector3(0, 0, tileCenterOffset);
        SpawnSingleBG(transition, centerPos);
    }

    public void SpawnBackgroundAt(BiomeData biome, float zPos)
    {
        if (biome.backgroundTiles == null || biome.backgroundTiles.Count == 0) return;

        var selectedBG = biome.backgroundTiles[Random.Range(0, biome.backgroundTiles.Count)];
        if (selectedBG == null || selectedBG.prefab == null) return;

        float tileCenterOffset = _levelParams.tileLength * 0.5f;
        float spawnZ = zPos + tileCenterOffset;

        SpawnSingleBG(selectedBG, new Vector3(0, 0, spawnZ));
        _lastBackgroundZ = spawnZ + selectedBG.GetWorldLength(_levelParams);
    }


    public void DespawnOldBackgrounds(float despawnZ)
    {
        while (_activeBackgroundTiles.Count > 0)
        {
            GameObject bg = _activeBackgroundTiles.Peek();
            var renderer = bg.GetComponentInChildren<Renderer>();
            float length = renderer != null ? renderer.bounds.size.z : _levelParams.tileLength;

            if (bg.transform.position.z + length < despawnZ)
                PoolManager.Instance.Despawn(_activeBackgroundTiles.Dequeue());
            else
                break;
        }
    }



    public void ResetBackgroundZ(float newZ) => _lastBackgroundZ = newZ;
}
