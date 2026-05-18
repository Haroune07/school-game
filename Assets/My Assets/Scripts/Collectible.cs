using UnityEngine;

public class Collectible : MonoBehaviour
{
    public AudioClip collectSoundEffect;
    public int healthIncrease = 2;

    public bool IncreaseLife = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var p = collision.gameObject.GetComponent<PlayerHealth>();
            if (IncreaseLife)
            {
                p.IncreaseLife();
            }

            p.IncreaseOrDecreaseHealth(healthIncrease);
            collision.gameObject.GetComponent<AudioSource>().PlayOneShot(collectSoundEffect);
            Destroy(gameObject);
        }
    }
}
