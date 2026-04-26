using UnityEngine;

public class MoveProjectile : MonoBehaviour
{
    public float flySpeed = 4;
    private float startX;
    public float startDeath = 6;

    public Vector3 TargetScale = Vector3.one;
    public float smoothTime = .2f;
    Vector3 currentVel = Vector3.zero;

    public float stepBackForce = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * flySpeed * Time.deltaTime;

        if(Mathf.Abs(transform.position.x - startX) >= startDeath)
        {
            Destroy(gameObject);
        }

        if(transform.localScale.x <= TargetScale.x)
        {
            transform.localScale = Vector3.SmoothDamp(transform.localScale, TargetScale, ref currentVel, smoothTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.GetComponent<EnemyCombat>();

        if (enemy != null)
        {
            Vector2 dir = (collision.transform.position - transform.position).normalized;
            enemy.TakeHit(dir, stepBackForce);

            Destroy(gameObject);
        }
    }
}
