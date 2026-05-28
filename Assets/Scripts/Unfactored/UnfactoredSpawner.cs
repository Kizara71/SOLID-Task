using UnityEngine;

public class UnfactoredSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab; // Drag your Sphere target prefab here
    public float spawnInterval = 2f; // Tries to spawn an enemy every 2 seconds

    [Header("Spawn Boundaries (Aimlabs Wall Area)")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = 1f;
    public float maxY = 6f;
    public float fixedZ = 5f; // Set this to match the distance of your aim wall floor coordinate

    private float spawnTimer;

    void Start()
    {
        spawnTimer = spawnInterval;
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            // Reset timer interval
            spawnTimer = spawnInterval;

            // Raw, un-refactored lookup find check (SRP / OCP violation example for class)
            // It counts how many objects with the "Enemy" tag are currently present in the active scene hierarchy
            int currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (currentEnemyCount < 3)
            {
                SpawnTargetSphere();
            }
        }
    }

    private void SpawnTargetSphere()
    {
        if (enemyPrefab == null) return;

        // Generate a random spatial coordinate on our target arena wall zone bounding profile
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(randomX, randomY, fixedZ);

        // Instantiate a new target ball instance directly into space geometry
        GameObject clonedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        clonedEnemy.name = "Target_Sphere_Clone";
    }
}