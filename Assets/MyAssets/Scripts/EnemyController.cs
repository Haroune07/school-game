using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Rigidbody2D rb;
    Animator anim;

    private bool isKnocked = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        anim = GetComponentInChildren<Animator>();
    }

    public void Move(Vector2 dir)
    {
        if (isKnocked) return;

        rb.linearVelocity = new Vector2(dir.x * moveSpeed, rb.linearVelocity.y);

        if (anim != null)
            anim.SetFloat("Speed", Mathf.Abs(dir.x));
    }

    public void Stop()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    public void ApplyKnockback(Vector2 force, float duration)
    {
        Stop();
        isKnocked = true;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);

        Invoke(nameof(EndKnockback), duration);
    }

    void EndKnockback()
    {
        isKnocked = false;
    }
}