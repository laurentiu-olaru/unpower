using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public HealthView playerHealth;
    public Image fillImage;

    void OnEnable()
    {
        playerHealth.Health.OnHealthChanged += UpdateBar;
    }

    void OnDisable()
    {
        playerHealth.Health.OnHealthChanged -= UpdateBar;
    }

    void UpdateBar(int current, int max)
    {
        fillImage.fillAmount = (float)current / max;
    }
}
