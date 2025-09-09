using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileContentSpawner : MonoBehaviour
{
    private GameObject _lastSelectedEnemy;
    private GameObject _lastSelectedObstacle;
    private GameObject _lastSelectedPowerUp;

    public GameObject GetLastSelectedEnemyPrefab() => _lastSelectedEnemy;
    public GameObject GetLastSelectedObstaclePrefab() => _lastSelectedObstacle;
    public GameObject GetLastSelectedPowerUpPrefab() => _lastSelectedPowerUp;

    public GameObject SpawnEnemy(BiomeData biome, Vector3 position, Transform parent)
    {
        if (biome.enemies == null || biome.enemies.Count == 0) return null;

        var enemy = GetWeightedRandom(biome.enemies);
        _lastSelectedEnemy = enemy?.prefab;

        return enemy != null
            ? PoolManager.Instance.Spawn(enemy.prefab, position + Vector3.up, Quaternion.identity, parent)
            : null;
    }

    public GameObject SpawnObstacle(BiomeData biome, Vector3 position, Transform parent)
    {
        if (biome.obstacles == null || biome.obstacles.Count == 0) return null;

        var obstacle = GetWeightedRandom(biome.obstacles);
        _lastSelectedObstacle = obstacle?.prefab;

        return obstacle != null
            ? PoolManager.Instance.Spawn(obstacle.prefab, position + Vector3.up * 0.5f, Quaternion.identity, parent)
            : null;
    }

    public GameObject SpawnPowerUp(BiomeData biome, Vector3 position, Transform parent)
    {
        if (biome.collectables == null || biome.collectables.Count == 0) return null;

        var powerup = GetWeightedRandom(biome.collectables);
        _lastSelectedPowerUp = powerup?.prefab;

        return powerup != null
            ? PoolManager.Instance.Spawn(powerup.prefab, position + Vector3.up * 1.2f, Quaternion.identity, parent)
            : null;
    }

    private T GetWeightedRandom<T>(List<T> items) where T : ScriptableObject
    {
        var weighted = items
            .Select(x => new { Item = x, Weight = GetWeight(x) })
            .ToList();

        int totalWeight = weighted.Sum(i => i.Weight);
        int rand = Random.Range(0, totalWeight);
        int sum = 0;

        foreach (var entry in weighted)
        {
            sum += entry.Weight;
            if (rand < sum) return entry.Item;
        }

        return weighted.LastOrDefault()?.Item;
    }

    private int GetWeight(ScriptableObject obj)
    {
        switch (obj)
        {
            case EnemyData e: return e.weight;
            case ObstacleData o: return o.weight;
            case CollectableData c: return c.weight;
            default: return 1;
        }
    }
}
