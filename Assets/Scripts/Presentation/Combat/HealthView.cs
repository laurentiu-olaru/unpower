using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour, IDamageable
{
    public int maxHP = 100;
    public GameObject gameOverUI;
    public Image fillImage;
    public HealthComponent Health => health;

    private HealthComponent health;

    void Awake()
    {
        health = new HealthComponent(maxHP);
        health.OnDied += HandleDeath;
        health.OnHealthChanged += HandleHealthChanged;
    }

    public void TakeDamage(int amount) => health.TakeDamage(amount);
    public void Heal(int amount) => health.Heal(amount);

    void UpdateUI(int current)
    {
        if (fillImage != null)
            fillImage.fillAmount = (float)current / health.MaxHP;
    }
    void HandleHealthChanged(int current, int max)
    {
        // UI will listen separately later
    }

    void HandleDeath()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        Time.timeScale = 0f;

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PlayerController>().enabled = false;
        if (GetComponent<Collider2D>())
            GetComponent<Collider2D>().enabled = false;
    }
}
