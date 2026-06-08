using UnityEngine;

public class UnfactoredSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab; 
    public float spawnInterval = 1f; 

    [Header("Spawn Boundaries (Aimlabs Wall Area)")]
    public float minX = -4f;
    public float maxX = 4f;
    public float minY = 1f;
    public float maxY = 6f;
    public float fixedZ = 5f; 

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

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(randomX, randomY, fixedZ);

        GameObject clonedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        //clonedEnemy.name = "Target_Sphere_Clone";
    }
}