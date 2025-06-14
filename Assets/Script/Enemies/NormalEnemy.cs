using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class NormalEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    private bool isFacingRight = true;
    
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform groundCheck;
    
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private float wallCheckDistance = 0.2f;
    
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float horizontalVelocity = isFacingRight ? moveSpeed : -moveSpeed;
        rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);

        CheckForTurn();
    }

    private void CheckForTurn()
    {
        bool wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, groundLayer);
        bool groundDetected = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (wallDetected || !groundDetected)
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

    private void OnDrawGizmosSelected()
    {
        // Wall Check line
        Gizmos.color = Color.red;
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (transform.right * wallCheckDistance));

        // --- Ground Check circle ---
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

}
