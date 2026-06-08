using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private EnemyHealthController enemyHealthController;
    [SerializeField] private Slider healthBarSlider;

    // Subscribe to events when enabled
    private void OnEnable()
    {
        if (enemyHealthController != null)
        {
            enemyHealthController.OnHealthChanged += UpdateHealthBar;
        }
    }

    // This method matches the signature of the Action<float, float> event
    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
        }
    }

    // Unsubscribe when disabled
    private void OnDisable()
    {
        if (enemyHealthController != null)
        {
            enemyHealthController.OnHealthChanged -= UpdateHealthBar;
        }
    }
}