using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public int maxHealth = 30;
    private int currentHealth;

    private EnemyController controller;
    public EnemyHealthManger healthManager;


    void Awake()
    {
        controller = GetComponent<EnemyController>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        
        if (healthManager != null)
        {
            healthManager.SetupHealthBar(maxHealth);
        }
    }

    public void TakeHit(Vector2 hitDir, float force)
    {
        GetComponent<EnemyFlash>().TriggerFlash();
        HitStopUtils.TriggerHitStop();
        currentHealth--;

        if (healthManager != null)
        {
            healthManager.UpdateHealthBar(currentHealth);
        }

        controller.ApplyKnockback(hitDir * force, 0.2f);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}