using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class PortalController : MonoBehaviour
{
    [SerializeField] private float activationDistance = 5f;
    [SerializeField] private float bufferZone = 1f;
    [SerializeField] private float revealSpeed = 1.5f;

    private Transform keyTransform;
    private Collider2D portalCollider;
    private Material portalMaterial;
    
    private bool isPortalActive = false;

    void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        portalMaterial = spriteRenderer.material;
        portalCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        GameObject keyObject = GameObject.FindWithTag("Key");
        if (keyObject != null) {
            keyTransform = keyObject.transform;
        } else {
            Debug.LogError("Portal couldn't find an object with the 'Key' tag.");
        }
        
        portalCollider.enabled = false;
        portalMaterial.SetFloat("_WipeAmount", 0f);
    }

    void Update()
    {
        if (keyTransform == null) return;

        float distanceToKey = Vector3.Distance(transform.position, keyTransform.position);
        
        if (!isPortalActive && distanceToKey <= activationDistance)
        {
            isPortalActive = true;
        }

        else if (isPortalActive && distanceToKey > activationDistance + bufferZone)
        {
            isPortalActive = false;
        }
        float targetWipe = isPortalActive ? 1f : 0f;
        
        float currentWipe = portalMaterial.GetFloat("_WipeAmount");
        float newWipe = Mathf.MoveTowards(currentWipe, targetWipe, revealSpeed * Time.deltaTime);
        portalMaterial.SetFloat("_WipeAmount", newWipe);
        
        portalCollider.enabled = (newWipe >= 0.99f);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, activationDistance);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, activationDistance + bufferZone);
    }
}