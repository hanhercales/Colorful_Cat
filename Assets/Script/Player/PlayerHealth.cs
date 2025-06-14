using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;
    
    [Header("Invincibility")]
    [SerializeField] private float invincibilityDuration = 1.0f;
    [SerializeField] private float invincibilityFlashDelay = 0.15f;
    private bool isInvincible = false;

    private Vector3 respawnPoint;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private PlayerMovement playerMovement;

    public static event Action OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
        respawnPoint = transform.position;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;

    public void TakeDamage(int damageAmount)
    {
        if (isInvincible)
        {
            return;
        }

        GetComponent<AudioSource>().PlayOneShot(hurtSound);
        
        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
        OnHealthChanged?.Invoke();
        
        StartCoroutine(InvincibilityCoroutine());

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        
        for (float i = 0; i < invincibilityDuration; i += invincibilityFlashDelay)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(invincibilityFlashDelay);
        }
        
        spriteRenderer.enabled = true;
        isInvincible = false;
    }

    public void RespawnAtStartPoint()
    {
        transform.position = respawnPoint;
        rb.velocity = Vector2.zero;
        GetComponent<PlayerAttack>().formManager.LoadForms();
        if(playerMovement.keyFollower != null)
            playerMovement.keyFollower.GetComponent<Key>().DropKey();
        Debug.Log("Player respawned at start.");
    }

    private void Die()
    {
        Debug.Log("Player has run out of health! Game Over.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}