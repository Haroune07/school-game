using UnityEngine;

public class HorizontalBackAndForth : MonoBehaviour
{
    public float speed = 1;
    public float range = 4;

    private Vector2 initialPos;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        initialPos = rb.position;
    }

    void FixedUpdate()
    {
        float x = Mathf.Sin(Time.time * speed) * range;

        Vector2 targetPos = initialPos + Vector2.right * x;

        rb.MovePosition(targetPos);
    }
}