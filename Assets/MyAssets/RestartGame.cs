using UnityEngine;
using UnityEngine.UI;

public class RestartGame : MonoBehaviour
{

    public GameOverManager gom;
    private Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(gom.Restart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
