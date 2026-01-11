using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthView : MonoBehaviour, IDamageable
{
	[Header("Stats")]
	public int maxHP = 100;

	[Header("UI")]
	public Image fillImage;

	[Header("Loot")]
	public GameObject coinPrefab;
	[Range(0, 1)] public float dropChance = 1f;

	private HealthComponent health;

	void Awake()
	{
		health = new HealthComponent(maxHP);
		health.OnHealthChanged += UpdateUI;
		health.OnDied += Die;
	}

	public void TakeDamage(int amount)
	{
		health.TakeDamage(amount);
	}

	void UpdateUI(int current, int max)
	{
		if (fillImage != null)
			fillImage.fillAmount = (float)current / max;
	}

	void Die()
	{
		if (coinPrefab && Random.value <= dropChance)
			Instantiate(coinPrefab, transform.position, Quaternion.identity);

		Destroy(gameObject);
	}
}
