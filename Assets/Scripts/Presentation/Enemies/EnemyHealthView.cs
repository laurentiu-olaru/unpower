using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyHealthView : MonoBehaviour, IDamageable, IHealthInitializable
{
    [Header("Stats")]
    public int maxHP = 100;

    [Header("UI")]
    public Image fillImage;

    [Header("Loot")]
    public GameObject coinPrefab;
    [Range(0, 1)] public float dropChance = 1f;

	[SerializeField] private bool destroyOnDie = true;
	[SerializeField] private float destroyDelaySeconds = 0f;


	private HealthComponent health;
    private bool initialized;

    private int coinsToDrop = 1;

    public event Action<int> Damaged; // amount

	public event Action<EnemyHealthView> Died;

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
        Damaged?.Invoke(amount);
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

    public void SetCoinsToDrop(int amount)
    {
        coinsToDrop = Mathf.Max(1, amount);
    }


    private void Die()
    {
		Died?.Invoke(this);

		if (coinPrefab != null && UnityEngine.Random.value <= dropChance)
		{
			for (int i = 0; i < coinsToDrop; i++)
				Instantiate(coinPrefab, transform.position, Quaternion.identity);
		}

		if (destroyOnDie)
		{
			Destroy(gameObject, destroyDelaySeconds);
		}
	}

}
