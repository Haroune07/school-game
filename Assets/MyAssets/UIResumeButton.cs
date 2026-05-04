using UnityEngine;
using UnityEngine.UI;

public class UIResumeButton : MonoBehaviour
{
    public PauseManager pauseManager;
    private Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(pauseManager.Resume);
    }
}
