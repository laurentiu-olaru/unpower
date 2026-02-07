using UnityEngine;
using UnityEngine.UI;

public class AllyHealthView : MonoBehaviour, IDamageable, IHealable
{
    [Header("Stats")]
    [SerializeField] private int maxHP = 50;

    [Header("UI")]
    [SerializeField] private Image healthBarFill;

    public HealthComponent Health => health;

    private HealthComponent health;

    void Awake()
    {
        health = new HealthComponent(maxHP);
        health.OnHealthChanged += UpdateUI;
        health.OnDied += Die;

        // init UI
        UpdateUI(health.CurrentHP, health.MaxHP);
    }

    public void TakeDamage(int amount)
    {
        health.TakeDamage(amount);
    }

    public void HealToFull()
    {
        // Heal by the gap
        int missing = health.MaxHP - health.CurrentHP;
        if (missing > 0)
            health.Heal(missing);
    }

    /// <summary>
    /// Increase max HP for this ally (and optionally heal to full).
    /// NOTE: requires HealthComponent to support increasing MaxHP; see Step 2.
    /// </summary>
    public void AddMaxHp(int amount, bool healToFull)
    {
        health.IncreaseMaxHp(amount, healToFull);
    }


    private void UpdateUI(int current, int max)
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = (float)current / max;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
