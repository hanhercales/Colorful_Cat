using UnityEngine;

public class PlayerSpriteRenderer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PlayerMovement movement;

    public Sprite idle;
    public Sprite jump;
    public AnimatedSprite walk;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
        walk.enabled = false;
    }
    
    private void LateUpdate()
    {
        walk.enabled = movement.walking;
        
        if (movement.jumping)
        {
            spriteRenderer.sprite = jump;
        } else if (!movement.walking)
        {
            spriteRenderer.sprite = idle;
        }
    }
    
}