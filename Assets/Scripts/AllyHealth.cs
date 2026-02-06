using UnityEngine;
using UnityEngine.UI;

public class AllyHealth : MonoBehaviour, IDamageable, IHealable
{
	public float maxHealth = 50f;
	private float currentHealth;

	[Header("UI Settings")]
	public Image healthBarFill;

	void Start()
	{
		currentHealth = maxHealth;
		UpdateHealthUI();
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

	public void HealToFull()
	{
		currentHealth = maxHealth;
		UpdateHealthUI();
	}
	void UpdateHealthUI()
	{
		if (healthBarFill != null)
		{
			// This changes the width of the bar based on percentage (0 to 1)
			healthBarFill.fillAmount = currentHealth / maxHealth;
		}
	}

    public void AddMaxHealth(float amount, bool healToFull = true)
    {
        maxHealth += amount;
        if (healToFull)
            currentHealth = maxHealth;

        UpdateHealthUI();
    }

    public void ApplyMaxHealthBonusOnce(float desiredBonusTotal, AllyUpgradeStamp stamp)
    {
        float toApply = desiredBonusTotal - stamp.AppliedHpBonus;
        if (toApply <= 0f) return;

        maxHealth += toApply;
        currentHealth = Mathf.Min(currentHealth + toApply, maxHealth); // keeps % roughly, or just clamp
        stamp.AppliedHpBonus += Mathf.RoundToInt(toApply);

        UpdateHealthUI();
    }


    void Die()
	{
		// You could add a death particle effect here
		Destroy(gameObject);
	}
}