using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    // The amount of damage the barrier will deal.
    private int barrierDamage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(barrierDamage);
            playerHealth.RespawnAtStartPoint();
        }
    }
}