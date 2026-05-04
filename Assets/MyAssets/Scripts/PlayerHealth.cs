// PlayerHealth.cs — no MovementController dependency
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth { get; private set; }

    private Animator anim;
    private Rigidbody2D rb;

    public Slider healthBar;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        if (healthBar)
        {
            healthBar.minValue = 0;
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeHit(Vector2 hitDir, float force)
    {
        currentHealth--;
        anim.SetTrigger("Hurt");
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(hitDir * force, ForceMode2D.Impulse);

        if (currentHealth <= 0) Die();
        if (healthBar) healthBar.value = currentHealth;
    }

    void Die()
    {
        anim.SetTrigger("Die");
    }
}