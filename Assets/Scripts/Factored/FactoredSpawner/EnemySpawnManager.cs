using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("Dependencies")]

    [SerializeField] private GameObject[] enemyPrefabs; 
    
    private ISpawnArea spawnArea; 

    [Header("Spawn Rules")]
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private int maxConcurrentEnemies = 5;

    private float spawnTimer;
    private int activeEnemyCount = 0;

    private void Awake()
    {
        spawnArea = GetComponent<ISpawnArea>();        
        if (spawnArea == null)
        {
            Debug.LogError("SpawnManager requires an ISpawnArea component to know where to spawn!");
        }
    }

    private void Start()
    {
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        if (activeEnemyCount >= maxConcurrentEnemies || spawnArea == null || enemyPrefabs.Length == 0) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        // 1. Pick a random enemy from our array
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedPrefab = enemyPrefabs[randomIndex];

        if (selectedPrefab == null) return;

        // 2. Get the position from our abstraction
        Vector3 spawnPosition = spawnArea.GetRandomSpawnPosition();

        // 3. Instantiate the selected enemy
        GameObject clonedEnemy = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        activeEnemyCount++; 

        // 4. Subscribe to its death event so we know when to decrease the count
        EnemyHealthController enemyHealth = clonedEnemy.GetComponent<EnemyHealthController>();
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath += HandleEnemyDeath;
        }
    }

    private void HandleEnemyDeath()
    {
        // Safely decrement the count, opening up a slot for the Update loop to spawn a new one
        activeEnemyCount--;
    }
}