using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player;

    [Header("Follow Target")]
    public Transform target;

    [Header("Camera Smoothing")]
    public float smoothSpeed = 0.1f;

    [Header("Vertical Movement")]
    public float verticalDeadZone = 1.5f;
    
    private float initialXPosition;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        if (target != null) {
            player = target;
        } else {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null) {
                player = playerObject.transform;
            } else {
                Debug.LogError("Camera cannot find a target or an object with the 'Player' tag!");
            }
        }
        initialXPosition = transform.position.x;
    }

    private void LateUpdate()
    {
        if (player == null) return;
        float targetX = Mathf.Max(initialXPosition, player.position.x);
        float targetY = transform.position.y;
        
        float yDistance = player.position.y - transform.position.y;
        
        if (yDistance > verticalDeadZone)
        {
            targetY = player.position.y - verticalDeadZone;
        }
        else if (yDistance < -verticalDeadZone)
        {
            targetY = player.position.y + verticalDeadZone;
        }
        
        Vector3 desiredPosition = new Vector3(targetX, targetY, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
    }
    
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        
        float screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        Vector3 deadZoneSize = new Vector3(screenWidth, verticalDeadZone * 2, 0);
        
        Gizmos.color = new Color(0, 1, 1, 0.25f); // Cyan
        Gizmos.DrawWireCube(transform.position, deadZoneSize);
    }
}