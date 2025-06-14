using System;
using UnityEngine;

public class PlayerProjectiles : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float speed = 10f;
    
    private float direction;
    private bool hit;
    private float lifetime;

    private BoxCollider2D boxCollider;
    private PlayerMovement player;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);
        
        lifetime += Time.deltaTime;
        if (lifetime > 2) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ground"))
        {
            hit = true;
            boxCollider.enabled = false;
            if (collision.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
            {
                if(enemyHealth.weaknessName == player.playerAttack.formManager.currentForm.name ||
                   player.playerAttack.formManager.currentForm.name == "RainbowForm")
                    enemyHealth.TakeDamage(damage);
                else if(enemyHealth.weaknessName == "BlankForm" && EasterEggUnlocked())
                    enemyHealth.TakeDamage(damage * 2);
            }
            Deactivate();
        }
    }

    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;
        
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
        {
            localScaleX = -localScaleX;
        }
        
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private bool EasterEggUnlocked()
    {
        foreach (Form form in player.playerAttack.formManager.formList)
        {
            if (form.name == "RainbowForm")
                return true;
        }
        return false;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
