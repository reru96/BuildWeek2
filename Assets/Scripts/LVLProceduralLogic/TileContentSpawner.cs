using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileContentSpawner : MonoBehaviour
{
    private GameObject _lastSelectedEnemy;
    private GameObject _lastSelectedObstacle;
    private GameObject _lastSelectedPowerUp;
    private GameObject _lastSelectedCoin;

    public GameObject GetLastSelectedEnemyPrefab() => _lastSelectedEnemy;
    public GameObject GetLastSelectedObstaclePrefab() => _lastSelectedObstacle;
    public GameObject GetLastSelectedPowerUpPrefab() => _lastSelectedPowerUp;
    public GameObject GetLastSelectedCoinPrefab() => _lastSelectedCoin;

    public GameObject SpawnEnemy(BiomeData biome, Vector3 position, Transform parent, int difficulty)
    {
        var filtered = biome.enemies
            .Where(e => e.minDifficultyLevel <= difficulty && difficulty <= e.maxDifficultyLevel)
            .ToList();

        var enemy = GetWeightedRandom(filtered);
        _lastSelectedEnemy = enemy?.prefab;

        return enemy != null
            ? PoolManager.Instance.Spawn(enemy.prefab, position + Vector3.up, Quaternion.identity, parent)
            : null;
    }

    public GameObject SpawnObstacle(BiomeData biome, Vector3 position, Transform parent, int difficulty)
    {
        var filtered = biome.obstacles
            .Where(e => e.minDifficult <= difficulty && difficulty <= e.maxDifficult)
            .ToList();

        var obstacle = GetWeightedRandom(filtered);
        _lastSelectedObstacle = obstacle?.prefab;

        return obstacle != null
            ? PoolManager.Instance.Spawn(obstacle.prefab, position + Vector3.up, Quaternion.identity, parent)
            : null;
    }

    public GameObject SpawnPowerUp(BiomeData biome, Vector3 position, Transform parent, int difficulty)
    {
        var filtered = biome.collectables
            .Where(e => e.minDifficult <= difficulty && difficulty <= e.maxDifficult)
            .ToList();

        var powerup = GetWeightedRandom(filtered);
        _lastSelectedPowerUp = powerup?.prefab;

        return powerup != null
            ? PoolManager.Instance.Spawn(powerup.prefab, position + Vector3.up * 1.2f, Quaternion.identity, parent)
            : null;
    }

    public GameObject SpawnCoin(BiomeData biome, Vector3 position, Transform parent, int difficulty)
    {
      
        var filtered = biome.coins
            .Where(c => c.minDifficult <= difficulty && difficulty <= c.maxDifficult) 
            .ToList();

        if (filtered.Count == 0) return null;

     
        CoinData coinData = filtered[Random.Range(0, filtered.Count)];
        _lastSelectedCoin = coinData.prefab;


        GameObject coin = PoolManager.Instance.Spawn(coinData.prefab, position + Vector3.up * 1f, Quaternion.identity, parent);

       
        CoinPickUp pickup = coin.GetComponent<CoinPickUp>();
        if (pickup != null)
            pickup.coinData = coinData;

        return coin;
    }


    private T GetWeightedRandom<T>(List<T> spawnables) where T : ScriptableObject
    {
        var weighted = spawnables
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