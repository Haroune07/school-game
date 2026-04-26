using UnityEngine;

public class Collectible : MonoBehaviour
{
    public AudioClip collectSoundEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //collision.CompareTag("Player") || 
        if (collision.CompareTag("Projectile") || collision.CompareTag("PlayerAttack")) { 
            Destroy(gameObject);
            ScoreManager.instance.AddScore(1);
        }else if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            ScoreManager.instance.AddScore(1);

            collision.gameObject.GetComponent<AudioSource>().PlayOneShot(collectSoundEffect);
        }

    }
}
