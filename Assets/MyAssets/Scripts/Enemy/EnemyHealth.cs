using UnityEngine;

// EnemyHealth.cs — replaces EnemyCombat health + old EnemyHealthManager
public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 30;
    private int currentHealth;

    private Animator anim;
    private EnemyController controller;
    private EnemyAI ai;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<EnemyController>();
        ai = GetComponent<EnemyAI>();
        currentHealth = maxHealth;
    }

    public void TakeHit(Vector2 hitDir, float force)
    {
        GetComponent<EnemyFlash>()?.TriggerFlash();
        HitStopUtils.TriggerHitStop();

        currentHealth--;
        anim.SetTrigger("Hurt");
        controller.ApplyKnockback(hitDir * force, 0.2f);
        ai.OnKnockback();

        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        anim.SetBool("IsDead", true);
        anim.SetTrigger("Die");
        Destroy(gameObject, .75f);
    }
}