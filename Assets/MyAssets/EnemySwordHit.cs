using UnityEngine;

public class EnemySwordHit : MonoBehaviour
{
    private int counter;
    public float knockbackForce = 8f;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log("Collision");
    //    var player = collision.GetComponent<PlayerHealth>();

    //    if (player != null)
    //    {
    //        Vector2 dir = (collision.transform.position - transform.position).normalized;
    //        player.TakeHit(dir, knockbackForce);
    //    }
    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        counter++;
        Debug.Log($"Collision detected! {counter}");
        var player = collision.GetComponent<PlayerHealth>();

        if (player != null)
        {
            Vector2 dir = (collision.transform.position - transform.position).normalized;
            player.TakeHit(dir, knockbackForce);
            gameObject.SetActive(false);
        }

        
    }
}
