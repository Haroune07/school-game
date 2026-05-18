using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public Canvas gameOverMenu;
    public PlayerHealth playerHealth;

    [Header("UI References")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI subtitleText;

    [Header("Messages")]
    public string defeatTitle = "Game Over";
    public string defeatSubtitle = "You have been defeated...";
    public string victoryTitle = "Congratulations!";
    public string victorySubtitle = "You have conquered the dungeon!";

    private bool isGameOver = false;

    public static GameOverManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!isGameOver && playerHealth.currentHealth <= 0)
        {
            TriggerGameOver(success: false);
        }
    }

    public void TriggerGameOver(bool success)
    {
        isGameOver = true;

        titleText.text = success ? victoryTitle : defeatTitle;
        subtitleText.text = success ? victorySubtitle : defeatSubtitle;

        Time.timeScale = 0f;
        gameOverMenu.gameObject.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Time.timeScale = 1f;

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}