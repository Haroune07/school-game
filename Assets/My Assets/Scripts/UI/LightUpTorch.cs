using UnityEngine;

public class LightUpTorch : MonoBehaviour, IInteractable
{
    public GameObject player;
    public float detectDistance = 1f;

    private Animator anim;
    private AudioSource audioSource;

    private bool isLit = false;

    public bool IsLit => isLit;

    public GameObject fireLight;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        var p = FindFirstObjectByType<PlayerHealth>();
        if (p != null)
            player = p.gameObject;

        audioSource.volume = 0;

        StatueManager.Instance.Register(this);
    }

    void Update()
    {
        if (isLit)
        {
            audioSource.volume = Mathf.Lerp(
                audioSource.volume,
                1f,
                2f * Time.deltaTime
            );

            return;
        }

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
        if (isLit) return;

        isLit = true;

        audioSource.Play();

        anim.SetTrigger("Begin");

        fireLight.SetActive(true);

        InteractionManager.Instance.ClearCurrent();
    }

    public string GetPrompt()
    {
        return "Light Torch";
    }
}