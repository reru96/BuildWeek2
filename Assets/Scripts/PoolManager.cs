using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private readonly Dictionary<GameObject, Queue<GameObject>> _pool = new(); // Dizionario che mappa i prefab alle loro code di istanze
    private readonly Dictionary<GameObject, GameObject> _instanceToPrefab = new(); // Dizionario per tracciare quale istanza proviene da quale prefab


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[PoolManager] Esiste già un'istanza, distruggo il duplicato.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void Prewarm(GameObject prefab, int count, Transform parent = null)
    {
        if (!_pool.ContainsKey(prefab))
            _pool[prefab] = new Queue<GameObject>();

        for (int i = 0; i < count; i++)
        {
            var inst = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent); 

            _instanceToPrefab[inst] = prefab;

            inst.SetActive(false);

            _pool[prefab].Enqueue(inst);
        }
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null) // Metodo per spawnare un oggetto dal pool usando il prefab originale
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
