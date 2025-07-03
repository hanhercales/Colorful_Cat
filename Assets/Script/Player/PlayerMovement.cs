using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;
    private bool isFacingRight = true; 
    
    public float moveSpeed = 8f;
    public float jumpForce = 16f;
    
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    
    public Animator animator;
    public Key keyFollower;
    public PlayerAttack playerAttack;
    
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Input is checked in Update() for responsiveness, as it runs every frame.
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Ground status and jump input are also checked here.
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // We set the velocity directly. The actual movement happens in FixedUpdate.
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            audioSource.PlayOneShot(jumpSound);
        }
        
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        
        //Set animation for object
        animator.SetFloat("Speed", rb.velocity.magnitude);
        if(!isGrounded)
        {
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Jump", true);
        }
        else
        {
            animator.SetBool("Jump", false);
        }
        
        FormChange();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        if ((horizontalInput > 0 && !isFacingRight) || (horizontalInput < 0 && isFacingRight))
        {
            Flip();
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    private void FormChange()
    {
        for (int i = 0; i < playerAttack.formManager.formList.Count; i++)
        {
            if(Input.GetKeyDown(playerAttack.formManager.formList[i].activationKey))
                playerAttack.formManager.ChangeForm(i);
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        
        Vector3 newScale = transform.localScale;
        newScale.x = (isFacingRight ? 1 : -1 ) * Mathf.Abs(transform.localScale.x);
        transform.localScale = newScale;
        
        if (keyFollower != null)
        {
            keyFollower.horizontalOffset *= -1;
        }
    }

    public bool canAttack()
    {
        return true;
    }
}