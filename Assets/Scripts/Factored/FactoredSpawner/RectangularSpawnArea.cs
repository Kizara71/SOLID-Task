using UnityEngine;

public class RectangularSpawnArea : MonoBehaviour, ISpawnArea
{
    [Header("Aimlabs Wall Boundaries")]
    [SerializeField] private float minX = -4f;
    [SerializeField] private float maxX = 4f;
    [SerializeField] private float minY = 1f;
    [SerializeField] private float maxY = 6f;
    [SerializeField] private float fixedZ = 5f;

    // ISpawnArea method
    public Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        
        return new Vector3(randomX, randomY, fixedZ);
    }
}