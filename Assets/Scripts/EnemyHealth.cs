using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
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
		// Add particle effects or score logic here later
		Destroy(gameObject);
	}
}