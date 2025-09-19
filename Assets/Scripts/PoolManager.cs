using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private readonly Dictionary<GameObject, Queue<GameObject>> _pool = new(); // prefab → coda di istanze
    private readonly Dictionary<GameObject, GameObject> _instanceToPrefab = new(); 

    protected override bool ShouldBeDestroyOnLoad() => true; 

    public void Prewarm(GameObject prefab, int count, Transform parent = null)
    {
        if (!_pool.ContainsKey(prefab))
            _pool[prefab] = new Queue<GameObject>();

        if (prefab.GetComponent<CoinPickUp>() != null)
            count *= 50; 

        for (int i = 0; i < count; i++)
        {
            var inst = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
            inst.SetActive(false);
            _instanceToPrefab[inst] = prefab;
            _pool[prefab].Enqueue(inst);
        }
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (!_pool.ContainsKey(prefab))
            _pool[prefab] = new Queue<GameObject>();

        GameObject instance;
        if (_pool[prefab].Count > 0)
        {
            instance = _pool[prefab].Dequeue();
            instance.SetActive(true);
        }
        else
        {
            instance = Instantiate(prefab, position, rotation, parent);
        }

        _instanceToPrefab[instance] = prefab;

        instance.transform.SetPositionAndRotation(position, rotation);
        if (parent != null)
            instance.transform.SetParent(parent);

        return instance;
    }

    public void Despawn(GameObject instance)
    {
        if (!_instanceToPrefab.TryGetValue(instance, out var originalPrefab))
        {
            Debug.LogWarning($"[PoolManager] Oggetto non tracciato: {instance.name}. Lo distruggo.");
            Destroy(instance);
            return;
        }

        instance.SetActive(false);
        _pool[originalPrefab].Enqueue(instance);
    }
}
