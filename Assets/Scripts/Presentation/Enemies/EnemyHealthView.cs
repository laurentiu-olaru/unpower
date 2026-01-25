using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthView : MonoBehaviour, IDamageable, IHealthInitializable
{
    [Header("Stats")]
    public int maxHP = 100;

    [Header("UI")]
    public Image fillImage;

    [Header("Loot")]
    public GameObject coinPrefab;
    [Range(0, 1)] public float dropChance = 1f;

    private HealthComponent health;
    private bool initialized;

    // Called by difficulty system BEFORE init
    public void SetMaxHp(int newMaxHp)
    {
        if (initialized) return;
        maxHP = Mathf.Max(1, newMaxHp);
    }

    private void Start()
    {
        EnsureInitialized();
    }

    public void TakeDamage(int amount)
    {
        EnsureInitialized();              // makes sure health exists
        health.TakeDamage(amount);        // triggers UI + death via events
    }

    private void EnsureInitialized()
    {
        if (initialized) return;

        health = new HealthComponent(maxHP);
        health.OnHealthChanged += UpdateUI;
        health.OnDied += Die;

        initialized = true;

        // set bar to full immediately
        UpdateUI(health.CurrentHP, health.MaxHP);
    }

    private void UpdateUI(int current, int max)
    {
        if (fillImage != null)
            fillImage.fillAmount = (float)current / max;
    }

    private void Die()
    {
        if (coinPrefab != null && Random.value <= dropChance)
            Instantiate(coinPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
