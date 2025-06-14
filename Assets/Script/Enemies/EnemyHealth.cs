using UnityEngine;
using System.Collections; // Required for using Coroutines

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    [Header("Flash Effect")]
    [SerializeField] private float flashDuration = 0.1f;
    
    // A reference to the enemy's sprite renderer and its material.
    private SpriteRenderer spriteRenderer;
    private Material material;

    private void Awake()
    {
        currentHealth = maxHealth;
        
        // Get the renderer and create a unique material instance for it.
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log($"Enemy took {damageAmount} damage. Health is now {currentHealth}");

        // --- Trigger the flash effect ---
        StartCoroutine(FlashEffect());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // This Coroutine handles the flash timing.
    private IEnumerator FlashEffect()
    {
        // 1. Turn the flash on by setting the shader property to 1.
        material.SetFloat("_FlashAmount", 1f);

        // 2. Wait for the specified duration.
        yield return new WaitForSeconds(flashDuration);

        // 3. Turn the flash off by setting the property back to 0.
        material.SetFloat("_FlashAmount", 0f);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}   