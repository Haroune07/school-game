using UnityEngine;

public class TurnOnLever : MonoBehaviour, IInteractable
{
    public GameObject player;
    public float detectDistance = 1.5f;

    private AudioSource audioSource;
    public AudioClip leverSound;

    private bool isActivated = false;

    public GameObject obstacleToRemove;

    public bool IsActivated => isActivated;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        var p = FindFirstObjectByType<PlayerHealth>();
        if (p != null)
            player = p.gameObject;
    }

    void Update()
    {
        if (isActivated) return;

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
        if (isActivated) return;

        isActivated = true;

        if (audioSource != null)
            audioSource.PlayOneShot(leverSound);

        InteractionManager.Instance.ClearCurrent();

        OnLeverActivated();
    }

    private void OnLeverActivated()
    {
        transform.localScale = new Vector3( - transform.localScale.x, transform.localScale.y, transform.localScale.z);
        obstacleToRemove.SetActive(false);
    }

    public string GetPrompt()
    {
        return "Pull Lever";
    }
}