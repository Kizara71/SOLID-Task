using UnityEngine;

public class DestructionHandler : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private EnemyHealthController enemyHealthController;
    
    [Header("VFX")]
    [SerializeField] private ParticleSystem destructionVFX;

    private void OnEnable()
    {
        if (enemyHealthController != null)
        {
            enemyHealthController.OnDeath += HandleDestruction;
        }
    }

    private void HandleDestruction()
    {
        if (destructionVFX != null)
        {
            ParticleSystem effectInstance = Instantiate(destructionVFX, transform.position, transform.rotation);
            effectInstance.Play(); 
            Destroy(effectInstance.gameObject, effectInstance.main.duration + effectInstance.main.startLifetime.constantMax);
        }
        Destroy(gameObject);
    }

        private void OnDisable()
    {
        if (enemyHealthController != null)
        {
            enemyHealthController.OnDeath -= HandleDestruction;
        }
    }
}