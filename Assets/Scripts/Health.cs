using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
	public int maxHP = 100;
	public int currentHP;

	public UnityEvent onDeath;
	public PlayerHealthUI healthUIScript;

	void Awake()
	{
		currentHP = maxHP;
	}

	public void TakeDamage(int amount)
	{
		currentHP -= amount;
		currentHP = Mathf.Clamp(currentHP, 0, maxHP);
		if (healthUIScript != null)
		{
			float currentFill = currentHP / maxHP;
		}
		if (currentHP <= 0)
		Die();
	}
	public void Heal(int amount)
	{
		currentHP += amount;

		// Clamp health so it doesn't go over the maximum
		if (currentHP > maxHP)
		{
			currentHP = maxHP;
		}

		// Update the UI bar so we see the heal happen
		if (healthUIScript != null)
		{
			float currentFill = currentHP / maxHP;
			//healthUIScript.UpdateHealthBar(currentFill);
		}

		Debug.Log("Healed! Current HP: " + currentHP);
	}

	void Die()
	{
		onDeath?.Invoke();
		Destroy(gameObject);
	}
}
