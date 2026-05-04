// EnemyController.cs — your existing one + facing direction + anim tweak
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector3 initialScale;
    private bool isKnocked = false;

    public string isWalkingString = "IsWalking";

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        initialScale = transform.localScale;
    }

    public void Move(Vector2 dir)
    {
        if (isKnocked) return;

        rb.linearVelocity = new Vector2(dir.x * moveSpeed, rb.linearVelocity.y);

        // Facing
        if (dir.x != 0)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(dir.x) * Mathf.Abs(initialScale.x),
                initialScale.y,
                initialScale.z);
        }

        anim.SetBool(isWalkingString, Mathf.Abs(dir.x) > 0.05f);
    }

    public void Stop()
    {
        if (isKnocked) return;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        anim.SetBool(isWalkingString, false);
    }

    public void ApplyKnockback(Vector2 force, float duration)
    {
        isKnocked = true;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);
        Invoke(nameof(EndKnockback), duration);
    }

    private void EndKnockback() => isKnocked = false;
}