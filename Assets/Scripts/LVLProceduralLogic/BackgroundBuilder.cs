using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundBuilder : MonoBehaviour
{

    //Sebbene sia contrario all'uso di classi dentro altre classi , in questo caso lo faccio per tenere tutto più ordinato
    [System.Serializable]
    public class BackgroundSet // un set di background per un bioma specifico
    {
        public BIOMETIPE biome;
        public List<BGTileData> backgroundTiles;
        public BGTileData entryTransition;  // entrata
        public BGTileData exitTransition;   // uscita
    }

    // Lista dei Campi

    [SerializeField] List<BackgroundSet> backgroundSets;
    [SerializeField] private Transform backgroundParent;

    private float _lastBackgroundZ = 0f;
    private LVLParameters _levelParams;

    private Dictionary<BIOMETIPE, BackgroundSet> _backgroundMap;
    private Queue<GameObject> _activeBackgroundTiles = new();


    // Metodi
    void Awake()
    {
        _backgroundMap = new Dictionary<BIOMETIPE, BackgroundSet>();
        foreach (var bg in backgroundSets)
            _backgroundMap[bg.biome] = bg;
    }

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

    public void SpawnTransition(BIOMETIPE biome, Vector3 position, bool isEntry)
    {
        if (!_backgroundMap.ContainsKey(biome)) return;

        BGTileData transition = isEntry
            ? _backgroundMap[biome].entryTransition
            : _backgroundMap[biome].exitTransition;

        if (transition == null || transition.prefab == null) return;

        float totalWidth = _levelParams.numberOfLanes * _levelParams.laneWidth;
        float offsetX = totalWidth * 0.5f + (_levelParams.laneWidth * 0.5f); // aggiungo mezzo tile

        // DX
        GameObject rightBG = PoolManager.Instance.Spawn(
            transition.prefab,
            position + new Vector3(offsetX, 0, 0),
            Quaternion.identity,
            backgroundParent
        );
        _activeBackgroundTiles.Enqueue(rightBG);

        // SX (mirror)
        GameObject leftBG = PoolManager.Instance.Spawn(
            transition.prefab,
            position - new Vector3(offsetX, 0, 0),
            Quaternion.identity,
            backgroundParent
        );
        leftBG.transform.localScale = new Vector3(-1, 1, 1);
        _activeBackgroundTiles.Enqueue(leftBG);
    }

    public void SpawnBackgroundAt(BIOMETIPE biome, float zPos)
    {
        if (!_backgroundMap.ContainsKey(biome)) return;
        var set = _backgroundMap[biome];

        if (set.backgroundTiles == null || set.backgroundTiles.Count == 0) return;

        var selectedBG = set.backgroundTiles[Random.Range(0, set.backgroundTiles.Count)];
        if (selectedBG == null || selectedBG.prefab == null) return;

        Vector3 pos = new Vector3(0, 0, zPos);
        SpawnSingleBG(selectedBG, pos);

        _lastBackgroundZ = zPos + selectedBG.GetWorldLength(_levelParams);
    }

    public void ResetBackgroundZ(float newZ) => _lastBackgroundZ = newZ;
}
