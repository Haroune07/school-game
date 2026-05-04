using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PauseManager : MonoBehaviour
{
    public Volume volume;
    private DepthOfField dof;
    private bool isPaused = false;
    public InputActionReference pauseAction;

    public Canvas gameUI;
    public Canvas pauseUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (volume.profile.TryGet(out dof))
        {
            dof.active = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseAction.action.WasPressedThisFrame())
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                Time.timeScale = 0f;
                dof.active = true;
                pauseUI.gameObject.SetActive(true);
                gameUI.gameObject.SetActive(false);
            }
            else
            {
                Resume();
            }
        }
    }

    public void Resume()
    {
        isPaused = false;
        dof.active = false;
        Time.timeScale = 1f;
        pauseUI.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(true);
    }
}
