using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class FlyingEnemy : MonoBehaviour
{
    [Header("Behavior Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float detectionRadius = 8f;
    
    [Header("Targeting & Damage")]
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private LayerMask playerLayer;

    private Rigidbody2D rb;
    private Transform playerTransform;
    private bool isChasing = false;
    private bool isFacingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    private void FixedUpdate()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            SearchForPlayer();
        }
    }

    private void SearchForPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        
        if (playerCollider != null)
        {
            Debug.Log("Player detected! Starting chase.");
            playerTransform = playerCollider.transform;
            isChasing = true;
        }
    }

    private void ChasePlayer()
    {
        if (playerTransform == null)
        {
            isChasing = false;
            rb.velocity = Vector2.zero; // Stop moving
            return;
        }
        
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
        FlipTowardsPlayer();
    }

    private void FlipTowardsPlayer()
    {
        if (playerTransform.position.x > transform.position.x && !isFacingRight)
        {
            Flip();
        }
        else if (playerTransform.position.x < transform.position.x && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }

    // Draw the detection radius in the editor for easy setup.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}