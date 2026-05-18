using UnityEngine;

public class FinishGame : MonoBehaviour
{

    private EnemyHealth health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health.currentHealth <= 0)
        {
            Invoke(nameof(GameOver), 3);
        }
    }

    void GameOver()
    {
        GameOverManager.Instance.TriggerGameOver(true);
    }

}
