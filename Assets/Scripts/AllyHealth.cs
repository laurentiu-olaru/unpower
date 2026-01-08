using UnityEngine;
using UnityEngine.UI;

public class AllyHealth : MonoBehaviour, IDamageable
{
	public float maxHealth = 50f;
	private float currentHealth;

	[Header("UI Settings")]
	public Image healthBarFill;

	void Start()
	{
		currentHealth = maxHealth;
	}

	public void TakeDamage(int damage)
	{
		currentHealth -= damage;

		// Update the UI immediately after taking damage
		UpdateHealthUI();

		if (currentHealth <= 0)
		{
			Die();
		}
	}

	void UpdateHealthUI()
	{
		if (healthBarFill != null)
		{
			// This changes the width of the bar based on percentage (0 to 1)
			healthBarFill.fillAmount = currentHealth / maxHealth;
		}
	}


	void Die()
	{
		// You could add a death particle effect here
		Destroy(gameObject);
	}
}