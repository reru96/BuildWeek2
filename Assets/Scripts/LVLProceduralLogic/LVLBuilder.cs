using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LVLBuilder : MonoBehaviour
{
    [SerializeField] private TileContentSpawner contentSpawner;

    [Header("Runner Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private int numberOfLanes = 3;
    [SerializeField] private float laneWidth = 4f;
    [SerializeField] private float tileLength = 10f;

    [Header("Generation Settings")]
    [SerializeField] private int safeStartTiles = 10; //Sono le tile iniziali senza buchi
    [SerializeField] private float generateDistance = 100f;
    [SerializeField] private float despawnDistance = 50f;

    [Header("Biome")]
    [SerializeField] private List<BiomeData> availableBiomes;
    [SerializeField] private float biomeChangeDistance = 200f;
    [SerializeField] private BiomeData currentBiome;
    private float nextBiomeChangeZ;

    [Header("Content Probabilities")] //Probabilità di spawnare ostacoli, nemici e power-up in una singola corsia DA SISTEMARE 
    [SerializeField, Range(0f, 1f)] private float obstacleChance = 0.3f;
    [SerializeField, Range(0f, 1f)] private float enemyChance = 0.15f;
    [SerializeField, Range(0f, 1f)] private float powerupChance = 0.1f;

    [Header("Chunk System")]
    [SerializeField] private int tilesPerChunk = 5;
    [SerializeField] private List<ChunkData> availableChunks;

    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = true;

    private TILETIPE[] lastLaneTypes; // Tiene traccia dell'ultimo tipo di corsia generata per ogni corsia

    private Queue<TileSegment> activeSegments = new(); // Coda per gestire i segmenti attivi
    private float lastGeneratedZ = 0f;  // Tiene traccia della posizione Z dell'ultima tile generata
    private int segmentCounter = 0;   // Conta il numero di segmenti generati

    void Start() => Initialize();

    void Update()
    {
        if (!player) return;

        GenerateAhead();
        CleanupBehind();
        CheckBiomeChange();
    }

    void Initialize() // Inizializza il generatore di livelli
    {
        if (availableBiomes.Count == 0)
        {
            Debug.LogError("No biomes assigned.");
            return;
        }
        foreach (var biome in availableBiomes) /// Prewarm delle pool per tutti i prefab usati nei biomi DA SISTEMARE E INTEGRARE CON IL CAMBIAMENTO DINAMICO DEI BIOMI
        {
            foreach (var tile in biome.aviabletileDatas) // in pratica carico in memoria tutti i prefab che potrebbero servire per ogni bioma
                PoolManager.Instance.Prewarm(tile.tilePrefab, 10, transform);

            foreach (var enemy in biome.enemies)
                PoolManager.Instance.Prewarm(enemy.prefab, 5, transform);

            foreach (var obstacle in biome.obstacles)
                PoolManager.Instance.Prewarm(obstacle.prefab, 5, transform);

            foreach (var collectable in biome.collectables)
                PoolManager.Instance.Prewarm(collectable.prefab, 5, transform);
        
        }


        currentBiome = availableBiomes[0];
        nextBiomeChangeZ = biomeChangeDistance;

        // Faccio in modo che le prime tile siano sempre sicure (bisogna pulirle dai nemici e ostacoli dopo)

        lastLaneTypes = new TILETIPE[numberOfLanes];
        for (int i = 0; i < numberOfLanes; i++)
            lastLaneTypes[i] = TILETIPE.FLOOR; // 

        for (int i = 0; i < safeStartTiles; i++)
            GenerateTileSegment(i * tileLength);

        ApplyBiomeEnvironment();
    }

    void GenerateAhead() // genera nuove tile davanti al giocatore o il chunk se necessario
    {
        while (lastGeneratedZ < player.position.z + generateDistance) //se la z dell'ultima tile generata è minore della z del giocatore + distanza di generazione
        {
            if (segmentCounter > 0 && segmentCounter % tilesPerChunk == 0) // ogni tot segmenti genero un chunk
            {
                GenerateChunk(lastGeneratedZ);
            }
            else
            {
                GenerateTileSegment(lastGeneratedZ);
            }
        }
    }

    void CleanupBehind()
    {
        while (activeSegments.Count > 0 && activeSegments.Peek().ZPosition < player.position.z - despawnDistance) // mentre ci sono segmenti attivi e la z del segmento più vecchio è minore della z del giocatore - distanza di despawn leva tutto
            DestroySegment(activeSegments.Dequeue());
    }

    void DestroySegment(TileSegment segment)
    {
        foreach (var laneObject in segment.LaneObjects)
        {
            if (laneObject != null)
                PoolManager.Instance.Despawn(laneObject);
        }

        foreach (var (instance, _) in segment.SpawnedContents)
        {
            if (instance != null)
                PoolManager.Instance.Despawn(instance);
        }

        if (segment.FloorObject != null)
            PoolManager.Instance.Despawn(segment.FloorObject);
    }



    //GESTIONE CAMBIO BIOMI
    void CheckBiomeChange()
    {
        if (player.position.z < nextBiomeChangeZ) return;

        List<BiomeData> otherBiomes = availableBiomes
            .Where(b => b.biomeType != currentBiome.biomeType)
            .ToList();

        if (otherBiomes.Count == 0) return;

        currentBiome = otherBiomes[Random.Range(0, otherBiomes.Count)];
        nextBiomeChangeZ += biomeChangeDistance;



        ApplyBiomeEnvironment();

        if (showDebugInfo)
            PrintDebugInfo();
    }

    void ApplyBiomeEnvironment()
    {
        RenderSettings.fogColor = currentBiome.fogColor;
        RenderSettings.ambientLight = currentBiome.ambientLightColor;

        if (!currentBiome.biomeMusic) return;

        var audio = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        if (audio.clip != currentBiome.biomeMusic)
        {
            audio.clip = currentBiome.biomeMusic;
            audio.loop = true;
            audio.Play();
        }
    }

    void ApplyBiomeMaterial(GameObject target)
    {
        if (currentBiome.tileMaterial == null) return;

        MeshRenderer[] renderers = target.GetComponentsInChildren<MeshRenderer>();
        foreach (var r in renderers)
        {
            r.material = currentBiome.tileMaterial;
        }
    }

    //GETIONE CHUNK 
    void GenerateChunk(float zPos)
    {
        ChunkData chunk = SelectWeightedChunk(availableChunks);

        if (chunk == null || chunk.chunkPrefab == null)
        {
            Debug.LogWarning("No valid chunk found");
            return;
        }

        Vector3 spawnPos = new(0, 0, zPos);
        GameObject chunkObj = PoolManager.Instance.Spawn(chunk.chunkPrefab, spawnPos, Quaternion.identity, transform); 

        ApplyBiomeMaterial(chunkObj); 

        activeSegments.Enqueue(new TileSegment(zPos, 1) // TileSegment è una struct che contiene le info di ogni segmento generato in questo caso il chunk è considerato come un singolo segmento con lunghezza 1
        {
            FloorObject = chunkObj, // l'oggetto pavimento del segmento è il chunk stesso
            Biome = currentBiome    // assegno il bioma corrente al segmento
        });

        lastGeneratedZ += tileLength * chunk.lengthInTiles; // Aggiorno la posizione Z dell'ultima tile generata in base alla lunghezza del chunk così da saltare le tile che il chunk occupa
        segmentCounter++;// Incremento il contatore dei segmenti generati
    }

    ChunkData SelectWeightedChunk(List<ChunkData> chunks) // E' un metodo per gestire la probabilità di spawnare i chunk in base a un peso assegnato a ciascun chunk
    {
        int totalWeight = 0;
        foreach (var chunk in chunks)
        {
            totalWeight += chunk.weight;
        }

        int range = Random.Range(0, totalWeight);
        int sum = 0;

        foreach (var chunk in chunks)
        {
            sum += chunk.weight;
            if (range < sum) return chunk;
        }

        return chunks.Last();
    }

    int ChooseLaneAccordingToRules(List<int> viableLanes)
    {
        // Caso base: scegli random
        if (segmentCounter == 0 || viableLanes.Count == 1)
            return viableLanes[0];

        List<int> previousViable = new();

        for (int i = 0; i < numberOfLanes; i++)
            if (lastLaneTypes[i] == TILETIPE.FLOOR)
                previousViable.Add(i);

        // Se la pista precedente ha solo una lane percorribile
        if (previousViable.Count == 1)
        {
            int prev = previousViable[0];
            if (viableLanes.Contains(prev))
                return prev;

            // Altrimenti scegli una lane adiacente
            return viableLanes.OrderBy(l => Mathf.Abs(l - prev)).First();
        }

        // Se pista precedente aveva più vie, scegli tra quelle non collegate
        List<int> nonConnected = viableLanes.Where(v => !previousViable.Contains(v)).ToList();
        if (nonConnected.Count > 0)
            return nonConnected[Random.Range(0, nonConnected.Count)];

        return viableLanes[Random.Range(0, viableLanes.Count)];
    }


    //GESTIONE TILE SINGOLE

    void GenerateTileSegment(float zPosition)
    {
        TileSegment segment = new(zPosition, numberOfLanes);
        segment.Biome = currentBiome;

        List<int> viableLanes = new();
        List<int> safeLanesForContent = new();

        for (int lane = 0; lane < numberOfLanes; lane++)
        {
            bool canBeHole = segmentCounter >= safeStartTiles && lastLaneTypes[lane] != TILETIPE.HOLE;
            if (canBeHole && Random.value < 0.3f) continue;

            viableLanes.Add(lane);

            bool hasSafeConnection =
                (lane > 0 && lastLaneTypes[lane - 1] == TILETIPE.FLOOR) ||
                (lane < numberOfLanes - 1 && lastLaneTypes[lane + 1] == TILETIPE.FLOOR) ||
                lastLaneTypes[lane] == TILETIPE.FLOOR;

            if (hasSafeConnection)
                safeLanesForContent.Add(lane);
        }

        if (viableLanes.Count == 0)
        {
            int fallbackLane = Random.Range(0, numberOfLanes);
            viableLanes.Add(fallbackLane);
        }

        int selectedLane = ChooseLaneAccordingToRules(viableLanes);



        bool hasSpawnedContentThisRow = false;

        for (int lane = 0; lane < numberOfLanes; lane++)
        {
            float x = -(numberOfLanes - 1) * laneWidth * 0.5f + lane * laneWidth;
            Vector3 lanePos = new(x, 0, zPosition);

            TILETIPE laneType = viableLanes.Contains(lane) ? TILETIPE.FLOOR : TILETIPE.HOLE;
            lastLaneTypes[lane] = laneType;
            segment.LaneTypes[lane] = laneType;

            if (laneType == TILETIPE.HOLE) continue;

            TileData tile = currentBiome.aviabletileDatas
                .Where(t => t.tileType == laneType)
                .OrderBy(t => Random.value * t.weight)
                .FirstOrDefault();

            if (tile != null && tile.tilePrefab != null)
            {
                GameObject tileObj = PoolManager.Instance.Spawn(tile.tilePrefab, lanePos, Quaternion.identity, transform);
                tileObj.transform.localScale = new Vector3(laneWidth, 1f, tileLength);
                ApplyBiomeMaterial(tileObj);
                segment.LaneObjects[lane] = tileObj;

                if (!hasSpawnedContentThisRow && safeLanesForContent.Contains(lane))
                {
                    GameObject content = null;
                    GameObject contentPrefab = null;

                    float rand = Random.value;

                    if (rand < powerupChance)
                    {
                        content = contentSpawner.SpawnPowerUp(currentBiome, lanePos, transform);
                        contentPrefab = contentSpawner.GetLastSelectedPowerUpPrefab();
                    }
                    else if (rand < powerupChance + enemyChance)
                    {
                        content = contentSpawner.SpawnEnemy(currentBiome, lanePos, transform);
                        contentPrefab = contentSpawner.GetLastSelectedEnemyPrefab();
                    }
                    else if (rand < powerupChance + enemyChance + obstacleChance)
                    {
                        content = contentSpawner.SpawnObstacle(currentBiome, lanePos, transform);
                        contentPrefab = contentSpawner.GetLastSelectedObstaclePrefab();
                    }

                    if (content != null && contentPrefab != null)
                    {
                        segment.SpawnedContents.Add((content, contentPrefab));
                        hasSpawnedContentThisRow = true;
                    }
                }
            }
        }

        activeSegments.Enqueue(segment);
        lastGeneratedZ += tileLength;
        segmentCounter++;
    }






    //DEBBUG PURPOSES ONLY

    public void PrintDebugInfo()
    {
        if (!showDebugInfo || !player) return;

        Debug.Log($"[LVLBuilder] Segmenti attivi: {activeSegments.Count}");
        Debug.Log($"[LVLBuilder] Ultima Z generata: {lastGeneratedZ}");
        Debug.Log($"[LVLBuilder] Bioma corrente: {currentBiome?.biomeType}");
        Debug.Log($"[LVLBuilder] Prossimo cambio bioma a Z: {nextBiomeChangeZ}");
    }

    void OnDrawGizmos()
    {
        if (!showDebugInfo || !player) return;

        for (int i = 0; i < numberOfLanes; i++)
        {
            float x = -(numberOfLanes - 1) * laneWidth * 0.5f + i * laneWidth;
            Vector3 start = new(x, 0, player.position.z - despawnDistance);
            Vector3 end = new(x, 0, player.position.z + generateDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(start, end);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(0, 1, player.position.z + generateDistance), new Vector3(numberOfLanes * laneWidth, 2, 1));

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(0, 1, player.position.z - despawnDistance), new Vector3(numberOfLanes * laneWidth, 2, 1));
    }

}
