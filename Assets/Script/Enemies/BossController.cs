using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : NormalEnemy
{
    [System.Serializable]
    private class BossForm
    {
        public string weaknessName;
        public Sprite bossSprite;
    }
    
    [SerializeField] private List<BossForm> formsList = new List<BossForm>();
    [SerializeField] private BossForm currentForm;
    [SerializeField] private EnemyHealth enemyHealth;
    
    public float verticalMoveSpeed = 2f;
    private float currentVerticalDirection = 1f;
    private float currentHorizontalDirection = 1f;
    
    public Transform playerTransform;
    public Transform bulletSpawnPoint;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float projectileBarrageHeight = 10f;
    public int numberOfShots = 2; 
    public float shotDelay = 0.3f; 

    public float chargeSpeed = 15f;
    public float chargeDuration = 1f;
    public float timeBetweenAttacks = 3f; 
    public float attackWindupTime = 0.5f;

    private bool canAttack = true;
    private Coroutine attackCoroutine;
    private bool isCharging = false;
    private Vector2 chargeDirection;
    
    private Vector3 initialBossPosition;

    protected override void Awake()
    {
        base.Awake();
        
        initialBossPosition = transform.position;
        
        ChangeForm();
    }

    protected override void FixedUpdate()
    {
        if (isCharging) return;
        
        rb.velocity = new Vector2(currentHorizontalDirection * moveSpeed, currentVerticalDirection * verticalMoveSpeed);
        
        CheckBossMovementBounds();
        
        if (canAttack && playerTransform != null)
        {
            attackCoroutine = StartCoroutine(AttackSequence());
        }
    }

    protected override void CheckForTurn()
    {
        
    }
    
    private void CheckBossMovementBounds()
    {
        bool wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * currentHorizontalDirection, wallCheckDistance, groundLayer);
        
        bool groundOrCeilingDetected = Physics2D.Raycast(groundCheck.position, Vector2.up * currentVerticalDirection, groundCheckRadius, groundLayer);

        if (wallDetected)
        {
            currentHorizontalDirection *= -1;
            Flip();
            Debug.Log("Boss hit horizontal bound, changing direction.");
        }

        if (groundOrCeilingDetected)
        {
            currentVerticalDirection *= -1;
            Debug.Log("Boss hit vertical bound, changing direction.");
        }
    }

    protected override void Flip()
    {
        base.Flip();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Vector2 collisionNormal = collision.GetContact(0).normal;
            if (Mathf.Abs(collisionNormal.x) > 0.1f)
            {
                currentHorizontalDirection *= -1;
                Flip();
            }
            if (Mathf.Abs(collisionNormal.y) > 0.1f)
            {
                currentVerticalDirection *= -1;
            }
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
    }
    
    IEnumerator AttackSequence()
    {
        canAttack = false;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(attackWindupTime);

        int attackChoice = UnityEngine.Random.Range(0, 2);

        if (attackChoice == 0)
        {
            yield return StartCoroutine(ProjectileBarrageAttack());
        }
        else
        {
            yield return StartCoroutine(ChargeAttack());
        }

        yield return new WaitForSeconds(timeBetweenAttacks - attackWindupTime); 

        ChangeForm();

        canAttack = true;
    }

    IEnumerator ProjectileBarrageAttack()
    {
        if (projectilePrefab == null) { yield break; }
        if (playerTransform == null) { yield break; }

        for (int i = 0; i < numberOfShots; i++)
        {
            Vector3 targetPlayerPos = playerTransform.position;
            Vector3 spawnPos = new Vector3(targetPlayerPos.x, targetPlayerPos.y + projectileBarrageHeight, targetPlayerPos.z);
            
            GameObject bullet = Instantiate(projectilePrefab, spawnPos, projectilePrefab.transform.rotation);
            Vector2 direction = Vector2.down;

            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
            if (rbBullet != null) { rbBullet.velocity = direction * projectileSpeed; }
            else { Debug.LogWarning("Projectile prefab does not have a Rigidbody2D."); }
            
            yield return new WaitForSeconds(shotDelay);
        }
    }

    IEnumerator ChargeAttack()
    {
        if (playerTransform == null || rb == null) { Debug.LogWarning("Player or Rigidbody2D not found."); yield break; }

        isCharging = true;
        rb.velocity = Vector2.zero;

        chargeDirection = (playerTransform.position - transform.position).normalized;

        if ((chargeDirection.x > 0 && !isFacingRight) || (chargeDirection.x < 0 && isFacingRight))
        {
            Flip();
        }

        float startTime = Time.time;
        while (Time.time < startTime + chargeDuration && isCharging)
        {
            rb.velocity = chargeDirection * chargeSpeed;
            yield return null;
        }

        rb.velocity = Vector2.zero;
        isCharging = false;
    }

    void ChangeForm()
    {
        int currentFormIndex = UnityEngine.Random.Range(0, formsList.Count - 1);
        
        if (formsList == null || formsList.Count == 0) return;

        currentForm = formsList[currentFormIndex];

        GetComponent<SpriteRenderer>().sprite = currentForm.bossSprite;
        enemyHealth.weaknessName = currentForm.weaknessName;
    }

    void OnDisable()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }
}