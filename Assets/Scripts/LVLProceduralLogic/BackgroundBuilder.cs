using System.Collections.Generic;
using UnityEngine;

public class BackgroundBuilder : MonoBehaviour
{
    [SerializeField] private Transform backgroundParent;
    [Header("Background Settings")]
    [SerializeField] private int maxBackgroundTiles = 50;

    private float _lastBackgroundZ = 0f;
    private LVLParameters _levelParams;
    private Queue<GameObject> _activeBackgroundTiles = new();

    public void Initialize(LVLParameters parameters)
    {
        _levelParams = parameters;
        _lastBackgroundZ = 0f;
        _activeBackgroundTiles.Clear();
    }

    private void SpawnSingleBG(BGTileData bgData, Vector3 basePos)
    {
        float totalWidth = _levelParams.numberOfLanes * _levelParams.laneWidth;
        // Correzione del calcolo dell'offset - posiziona ai lati delle corsie
        float offsetX = (totalWidth + _levelParams.laneWidth) * 0.5f;

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

            // Specchia rispettando la scala originale
            Vector3 s = leftBG.transform.localScale;
            leftBG.transform.localScale = new Vector3(-Mathf.Abs(s.x), s.y, s.z);

            _activeBackgroundTiles.Enqueue(leftBG);
        }

        // Gestione limite massimo tile attive
        while (_activeBackgroundTiles.Count > maxBackgroundTiles)
        {
            GameObject oldTile = _activeBackgroundTiles.Dequeue();
            if (oldTile != null)
                PoolManager.Instance.Despawn(oldTile);
        }
    }

    public void SpawnBiomeTransition(BGTileData transition, Vector3 position)
    {
        if (transition == null || transition.prefab == null) return;

        float transitionLength = transition.worldLength > 0
            ? transition.worldLength
            : _levelParams.tileLength;

        // CORREZIONE: Non aggiungere offset al centro, usa la posizione esatta
        SpawnSingleBG(transition, position);

        // Aggiorna _lastBackgroundZ alla fine della transizione
        _lastBackgroundZ = position.z + transitionLength;
    }

    public void FillBackgroundUpTo(BiomeData biome, float targetZ)
    {
        if (biome.backgroundTiles == null || biome.backgroundTiles.Count == 0) return;

        while (_lastBackgroundZ < targetZ)
        {
            var selectedBG = biome.backgroundTiles[Random.Range(0, biome.backgroundTiles.Count)];
            if (selectedBG == null || selectedBG.prefab == null) return;

            float length = selectedBG.GetWorldLength(_levelParams);

            Vector3 spawnPos = new Vector3(0, 0, _lastBackgroundZ);
            SpawnSingleBG(selectedBG, spawnPos);

            _lastBackgroundZ += length;
        }
    }

    public void SpawnBackgroundAt(BiomeData biome, float zPos)
    {
        if (biome.backgroundTiles == null || biome.backgroundTiles.Count == 0) return;

        var selectedBG = biome.backgroundTiles[Random.Range(0, biome.backgroundTiles.Count)];
        if (selectedBG == null || selectedBG.prefab == null) return;

        float length = selectedBG.GetWorldLength(_levelParams);

        // Forza continuità: non spawnare mai prima di _lastBackgroundZ
        float spawnZ = Mathf.Max(zPos, _lastBackgroundZ);

        SpawnSingleBG(selectedBG, new Vector3(0, 0, spawnZ));

        // Aggiorna ultima Z alla fine della tile
        _lastBackgroundZ = spawnZ + length;
    }

    public void DespawnOldBackgrounds(float despawnZ)
    {
        while (_activeBackgroundTiles.Count > 0)
        {
            GameObject bg = _activeBackgroundTiles.Peek();
            if (bg == null)
            {
                _activeBackgroundTiles.Dequeue();
                continue;
            }

            var renderer = bg.GetComponentInChildren<Renderer>();
            float length = renderer != null ? renderer.bounds.size.z : _levelParams.tileLength;

            if (bg.transform.position.z + length < despawnZ)
                PoolManager.Instance.Despawn(_activeBackgroundTiles.Dequeue());
            else
                break;
        }
    }

    public void SyncBackgroundTo(float zPos)
    {
        // Allinea il cursore del BG solo in avanti, mai indietro
        if (zPos > _lastBackgroundZ)
            _lastBackgroundZ = zPos;
    }

    public void ResetBackgroundZ(float newZ)
    {
        // Riallinea _lastBackgroundZ a una griglia di tileLength per evitare drift
        float t = Mathf.Round(newZ / _levelParams.tileLength);
        _lastBackgroundZ = t * _levelParams.tileLength;
    }

    // NUOVO METODO: Ferma la generazione di background normale per permettere transizioni
    public void SuppressBackgroundGeneration(float fromZ, float toZ)
    {
        // Se ci sono gap, li riempiamo prima della soppressione
        if (_lastBackgroundZ < fromZ)
        {
            // Questo dovrebbe essere chiamato con il bioma corrente
            Debug.LogWarning("[BackgroundBuilder] Gap nel background prima della transizione!");
        }

        // Salta alla fine della zona soppressa
        _lastBackgroundZ = toZ;
    }
}