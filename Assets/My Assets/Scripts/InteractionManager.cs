using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance;

    public InputActionReference interactAction;

    public GameObject panel;
    public TextMeshProUGUI promptText;

    private IInteractable current;

    public IInteractable Current => current;

    void Awake()
    {
        Instance = this;

        panel.SetActive(false);
    }

    void Update()
    {
        if (current != null)
        {
            if (interactAction.action.WasPressedThisFrame())
            {
                current.Interact();
            }
        }
    }

    public void SetCurrent(IInteractable interactable)
    {
        current = interactable;

        panel.SetActive(true);

        promptText.text = interactable.GetPrompt();
    }

    public void ClearCurrent()
    {
        current = null;

        panel.SetActive(false);
    }
}