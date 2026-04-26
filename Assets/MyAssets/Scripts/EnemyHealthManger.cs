using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthManger : MonoBehaviour
{
    public Slider healthBar;

    // We can initialize the slider's max limits here
    public void SetupHealthBar(int maxHealth)
    {
        healthBar.minValue = 0;
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;

        healthBar.gameObject.SetActive(false);
    }

    public void UpdateHealthBar(int currentHealth)
    {
        healthBar.value = currentHealth;
    }
}