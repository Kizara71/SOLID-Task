using UnityEngine;

// Any script implementing this interface promises to return a valid Vector3.
// The Spawner doesn't care HOW it gets the math, just that it gets a position.
public interface ISpawnArea
{
    Vector3 GetRandomSpawnPosition();
}