using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private Transform player;

    [Header("Prefabs")]
    [SerializeField] private GameObject[] obstaclePrefabs; // сюда A и B

    [Header("Lanes (X positions)")]
    [SerializeField] private float[] lanesX = new float[] { -2f, 0f, 2f };

    [Header("Spawn settings")]
    [Min(1f)] [SerializeField] private float spawnDistanceAhead = 35f;
    [Min(1f)] [SerializeField] private float spawnIntervalZ = 12f;
    [SerializeField] private float spawnY = 0.5f;

    [SerializeField] private float minSpawnIntervalZ = 7f;   // на максимальной сложности
    [SerializeField] private float maxSpawnIntervalZ = 12f;  // на старте

    [Header("Cleanup")]
    [Min(5f)] [SerializeField] private float destroyDistanceBehind = 20f;

    [Header("Pickups")]
    [SerializeField] private GameObject[] pickupPrefabs;
    [Range(0f, 1f)] [SerializeField] private float pickupChance = 0.25f;
    [SerializeField] private float pickupY = 1.2f;

    private float nextSpawnZ;

    private void Start()
    {
        if (player == null || obstaclePrefabs == null || obstaclePrefabs.Length == 0)
        {
            Debug.LogError("Spawner is not configured (player or prefabs).");
            enabled = false;
            return;
        }

        nextSpawnZ = player.position.z + spawnDistanceAhead;
    }

    private void Update()
    {
        if (player == null) return;
        float mult = GameManager.I != null ? GameManager.I.SpeedMultiplier : 1f;
        float k = Mathf.InverseLerp(1f, 2.2f, mult); // 0..1 (под maxSpeedMultiplier)
        float currentIntervalZ = Mathf.Lerp(maxSpawnIntervalZ, minSpawnIntervalZ, k);
        float currentSpawnAhead = Mathf.Lerp(spawnDistanceAhead, spawnDistanceAhead + 10f, k);

        while (player.position.z + currentSpawnAhead >= nextSpawnZ)
        {
        SpawnAtZ(nextSpawnZ);
        nextSpawnZ += currentIntervalZ;
        }
    }

    private void SpawnAtZ(float z)
    {
        int prefabIndex = Random.Range(0, obstaclePrefabs.Length);
        int laneIndex = Random.Range(0, lanesX.Length);

        var prefab = obstaclePrefabs[prefabIndex];
        Vector3 pos = new Vector3(lanesX[laneIndex], spawnY, z);

        var obj = Instantiate(prefab, pos, Quaternion.identity);

        obj.AddComponent<DestroyBehindPlayer>()
           .Init(player, destroyDistanceBehind);

        if (pickupPrefabs != null && pickupPrefabs.Length > 0 && Random.value < pickupChance)
        {
        int pIndex = Random.Range(0, pickupPrefabs.Length);
        int laneIndex2 = Random.Range(0, lanesX.Length);
        var pPrefab = pickupPrefabs[pIndex];
        Instantiate(pPrefab, new Vector3(lanesX[laneIndex2], pickupY, z + 2f), Quaternion.identity);
        }
    }

    private class DestroyBehindPlayer : MonoBehaviour
    {
        private Transform player;
        private float distBehind;

        public DestroyBehindPlayer Init(Transform p, float d)
        {
            player = p;
            distBehind = d;
            return this;
        }

        private void Update()
        {
            if (player == null) { Destroy(gameObject); return; }
            if (player.position.z - transform.position.z > distBehind)
                Destroy(gameObject);
        }
    }
}