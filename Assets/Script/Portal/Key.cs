using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Key : MonoBehaviour
{
    private Transform playerTransform;
    private bool isFollowingPlayer = false;
    private Vector3 initialPosition;
    [SerializeField] private Vector3 originalPosition;

    [Header("Idle Floating (Before Pickup)")]
    [Tooltip("How fast the key bobs up and down when idle.")]
    public float idleFloatSpeed = 1.5f;
    [Tooltip("How high and low the key bobs when idle.")]
    public float idleFloatStrength = 0.25f;

    [Header("Following Player (After Pickup)")]
    public float horizontalOffset = -1.2f; 
    public float verticalOffset = 0.5f;
    public float followFloatSpeed = 2f;
    public float followFloatStrength = 0.2f;

    void Start()
    {
        initialPosition = originalPosition = transform.position;
    }

    void Update()
    {
        if (isFollowingPlayer)
        {
            FollowPlayer();
        }
        else
        {
            IdleFloat();
        }
    }
    
    private void IdleFloat()
    {
        float floatMotion = Mathf.Sin(Time.time * idleFloatSpeed) * idleFloatStrength;
        transform.position = initialPosition + new Vector3(0, floatMotion, 0);
    }
    
    private void FollowPlayer()
    {
        if (playerTransform == null) return;

        Vector3 targetPosition = playerTransform.position;
        targetPosition.x += horizontalOffset;
        targetPosition.y += verticalOffset;

        float floatMotion = Mathf.Sin(Time.time * followFloatSpeed) * followFloatStrength;
        targetPosition.y += floatMotion;

        transform.position = targetPosition;
    }
    
    public Vector3 GetOriginalPosition() => originalPosition;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isFollowingPlayer || !other.CompareTag("Player"))
        {
            return;
        }
        
        // Picked up
        Debug.Log("Key picked up by player!");
        isFollowingPlayer = true;

        playerTransform = other.transform;
        
        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.keyFollower = this;
        }
        
        GetComponent<Collider2D>().enabled = false;
    }
    
    public void DropKey()
    {
        if (!isFollowingPlayer)
        {
            Debug.LogWarning("DropKey() called but key is not currently following the player.", this);
            return;
        }

        isFollowingPlayer = false;
        playerTransform = null;

        transform.position = originalPosition;
        initialPosition = originalPosition; 

        GetComponent<Collider2D>().enabled = true;
        
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        if (playerGameObject != null)
        {
            PlayerMovement playerMovement = playerGameObject.GetComponent<PlayerMovement>();
            if (playerMovement != null && playerMovement.keyFollower == this)
            {
                playerMovement.keyFollower = null;
            }
        }
    }
}