using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class NormalEnemy : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 2f;
    protected bool isFacingRight = true;
    
    [SerializeField] protected int damageAmount = 1;
    [SerializeField] protected LayerMask groundLayer;

    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected Transform groundCheck;
    
    [SerializeField] protected float groundCheckRadius = 0.2f;
    [SerializeField] protected float wallCheckDistance = 0.2f;
    
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        float horizontalVelocity = isFacingRight ? moveSpeed : -moveSpeed;
        rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);

        CheckForTurn();
    }

    protected virtual void CheckForTurn()
    {
        bool wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, groundLayer);
        bool groundDetected = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (wallDetected || !groundDetected)
        {
            Flip();
        }
    }

    protected virtual void Flip()
    {
        isFacingRight = !isFacingRight;
        
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        // Wall Check line
        Gizmos.color = Color.red;
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (transform.right * wallCheckDistance));

        // --- Ground Check circle ---
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

}
