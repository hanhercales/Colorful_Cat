using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeartsDisplay : MonoBehaviour
{
    [Header("Heart Sprites")]
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;

    [Header("UI Setup")] public GameObject heartPrefab;
    
    private List<Image> hearts = new List<Image>();
    
    private void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateHearts;
    }
    
    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateHearts;
    }

    private void Start()
    {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            CreateHeartIcons(playerHealth.GetMaxHealth());
            UpdateHearts();
        }
    }
    
    void CreateHeartIcons(int maxHealth)
    {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        hearts.Clear();

        // Create a heart icon for each point of max health.
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, transform);
            hearts.Add(newHeart.GetComponent<Image>());
        }
    }
    
    void UpdateHearts()
    {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth == null) return; // Safety check

        int currentHealth = playerHealth.GetCurrentHealth();

        // Loop through all the heart icons we've created.
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeartSprite;
            }
            else
            {
                hearts[i].sprite = emptyHeartSprite;
            }
        }
    }
}