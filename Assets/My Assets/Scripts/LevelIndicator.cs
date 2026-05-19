using UnityEngine;
using TMPro;

public class LevelIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;

    public void UpdateLevel(string levelName)
    {
        levelText.text = levelName;
    }
}