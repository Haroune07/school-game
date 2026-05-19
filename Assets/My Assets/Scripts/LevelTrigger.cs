using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private LevelIndicator levelIndicator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            levelIndicator.UpdateLevel(levelName);
        }
    }
}