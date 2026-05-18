// PlayerHealth.cs — no MovementController dependency
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth { get; private set; }

    public int maxLives = 3;
    public int currentLives { get; private set; }

    private Animator anim;
    private Rigidbody2D rb;

    public Slider healthBar;
    public Slider lifeBar;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        currentLives = maxLives;

        // HEALTH BAR
        if (healthBar)
        {
            healthBar.minValue = 0;
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        // LIFE BAR
        if (lifeBar)
        {
            lifeBar.minValue = 0;
            lifeBar.maxValue = maxLives;
            lifeBar.value = currentLives;
        }
    }

    public void TakeHit(Vector2 hitDir, float force)
    {
        currentHealth--;
        anim.SetTrigger("Hurt");

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(hitDir * force, ForceMode2D.Impulse);

        if (currentHealth <= 0)
            LoseLife();

        UpdateUI();
    }

    public void IncreaseOrDecreaseHealth(int by)
    {
        currentHealth = Mathf.Clamp(currentHealth + by, 0, maxHealth);
        UpdateUI();
    }

    public void IncreaseLife()
    {
        currentLives = Mathf.Clamp(currentLives + 1, 0, maxLives);
        UpdateUI();
    }

    void LoseLife()
    {
        currentLives--;

        if (currentLives <= 0)
        {
            Die();
            return;
        }

        currentHealth = maxHealth;
    }

    void UpdateUI()
    {
        if (healthBar)
            healthBar.value = currentHealth;

        if (lifeBar)
            lifeBar.value = currentLives;
    }

    void Die()
    {
        anim.SetTrigger("Die");

        GameOverManager.Instance.TriggerGameOver(false);
    }
}