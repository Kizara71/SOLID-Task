using System;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour, IDamageable 
{
    // EVENTS: This is Dependency Inversion in action. 
    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth); 
    }

    // IDamageable method
    public void TakeDamage(float damageAmount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damageAmount;
        
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            OnDeath?.Invoke(); 
        }
    }
}