using UnityEngine;
using UnityEngine.UI;

public class UnfactoredEnemy : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI Component")]
    public Slider healthBarSlider;
    
    [Header("VFX")]
    public ParticleSystem destructionVFX;

    void Start()
    {
        currentHealth = maxHealth;

        // Initialize the health bar slider parameters
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " took damage! Current Health: " + currentHealth);

        // Directly update the UI slider value
        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
        }

        // Check if the target is destroyed
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " has been destroyed!");
        if (destructionVFX != null)
        {
            // Spawn the particle system at the enemy's current position and rotation
            ParticleSystem effectInstance = Instantiate(destructionVFX, transform.position, transform.rotation);
        
            // Play it explicitly if "Play On Awake" is turned off
            effectInstance.Play(); 
        
            // Clean it up completely after it finishes playing
            Destroy(effectInstance.gameObject, effectInstance.main.duration + effectInstance.main.startLifetime.constantMax);
        }
        Destroy(gameObject); // Simply delete the sphere from the scene
    }
}