using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public Canvas gameOverMenu;
    public PlayerHealth playerHealth;

    private bool isGameOver = false;

    void Update()
    {
        if (!isGameOver && playerHealth.currentHealth <= 0)
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        isGameOver = true;

        Time.timeScale = 0f;
        gameOverMenu.gameObject.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}