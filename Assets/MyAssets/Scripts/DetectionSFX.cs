using UnityEngine;

public class DetectionSFX : MonoBehaviour
{
    private bool isInside = false;
    private AudioSource source;

    private float targetVolume = 0;

    public float fadeSpeed = 2f;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        targetVolume = isInside ? .6f : 0f;

        source.volume = Mathf.Lerp(
            source.volume,
            targetVolume,
            fadeSpeed * Time.deltaTime
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerHealth>() != null)
        {
            isInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerHealth>() != null)
        {
            isInside = false;
        }
    }
}