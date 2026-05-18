using UnityEngine;

public class StatueInteract : MonoBehaviour, IInteractable
{
    public GameObject player;
    public float detectDistance = 1.5f;
    private AudioSource audioSource;

    public Transform teleportPoint;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        var p = FindFirstObjectByType<PlayerHealth>();
        if (p != null)
            player = p.gameObject;
    }

    public void PlaySound()
    {
        Debug.Log(" try Sound!");
        if (audioSource != null){
            audioSource.Play();
            Debug.Log("Sound! should play!");
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(
            transform.position,
            player.transform.position
        );

        if (distance <= detectDistance)
        {
            InteractionManager.Instance.SetCurrent(this);
        }
        else if (InteractionManager.Instance.Current == this)
        {
            InteractionManager.Instance.ClearCurrent();
        }
    }

    public void Interact()
    {
        InteractionManager.Instance.ClearCurrent();

        player.transform.position = teleportPoint.position;
    }

    public string GetPrompt()
    {
        return "Inspect Statue";
    }
}