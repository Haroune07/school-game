using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    private Button btn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
