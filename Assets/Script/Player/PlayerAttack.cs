using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private AudioClip shootingSound;
    
    public FormManager formManager;
    
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.J) && cooldownTimer > attackCooldown && playerMovement.canAttack())
        {
            Attack();
        }
        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        cooldownTimer = 0f;
        
        int bulletIndex = FindBullet();
    
        // 2. Get a reference to that specific bullet GameObject.
        GameObject bullet = formManager.projectiles[bulletIndex];
        
        GetComponent<AudioSource>().PlayOneShot(shootingSound);

        // 3. Set its position and fire it.
        bullet.transform.position = firePoint.position;
        bullet.GetComponent<PlayerProjectiles>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindBullet()
    {
        for (int i = 0; i < formManager.projectiles.Length; i++)
        {
            if (!formManager.projectiles[i].activeInHierarchy)
            {
                return i;
            }
        }

        return 0;
    }
}
