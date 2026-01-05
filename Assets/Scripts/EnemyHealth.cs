using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Loot")]
    public GameObject coinPrefab; // Drag your coin prefab here
    [Range(0, 1)]
    public float dropChance = 1.0f; // 1.0 means 100% chance

    public float maxHealth = 100f;
	private float currentHealth;

	// Drag the Red "Fill" Image from the child canvas here
	public Image healthBarFill;
	// Drag the whole Canvas object here (so we can hide it if needed)
	public GameObject healthCanvas;

	void Start()
	{
		currentHealth = maxHealth;
		UpdateHealthUI();
	}

	public void TakeDamage(float amount)
	{
		currentHealth -= amount;
		UpdateHealthUI();

		if (currentHealth <= 0)
		{
			Die();
		}
	}

	void UpdateHealthUI()
	{
		// Update the fill amount (0 to 1)
		if (healthBarFill != null)
		{
			healthBarFill.fillAmount = currentHealth / maxHealth;
		}
	}

	void Die()
	{
        // Check if we should spawn a coin
        if (coinPrefab != null && Random.value <= dropChance)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}