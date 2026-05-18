using UnityEngine;

public class InstantDeath : MonoBehaviour
{
    public Transform respawnPoint;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        var p = collider.gameObject.GetComponent<PlayerHealth>();
        var e = collider.gameObject.GetComponent<EnemyAI>();
        if (p != null)
        {
            //make him die
            p.IncreaseOrDecreaseHealth(p.maxHealth);

            p.transform.position = respawnPoint.position;
        }

        if(e != null)
        {
            Destroy(e.gameObject);
        }
    }
}
