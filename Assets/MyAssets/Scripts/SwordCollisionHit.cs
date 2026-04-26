using UnityEngine;

public class SwordCollisionHit : MonoBehaviour
{
    public float stepBackForce = 20;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.GetComponent<EnemyCombat>();

        if (enemy != null)
        {
            Vector2 dir = (collision.transform.position - transform.position).normalized;
            enemy.TakeHit(dir, stepBackForce);
        }
    }
}
