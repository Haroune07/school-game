using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{


    private Button btn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => { Application.Quit(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
