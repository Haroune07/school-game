using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int score = 0;
    private TextMeshProUGUI text;
    public GameObject textMesh;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        text = textMesh.GetComponent<TextMeshProUGUI>();
    }

    public void AddScore(int value)
    {
        score += value;
        text.text = $"Player Score {score}";
    }

    public void DecreaseScore(int value)
    {
        score -= value;
        text.text = $"Score {score}";
    }
}