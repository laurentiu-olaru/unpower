using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
	public int maxHP = 100;
	public int currentHP;

	public UnityEvent onDeath;

	void Awake()
	{
		currentHP = maxHP;
	}

	public void TakeDamage(int amount)
	{
		currentHP -= amount;
		currentHP = Mathf.Clamp(currentHP, 0, maxHP);

		if (currentHP <= 0)
			Die();
	}

	void Die()
	{
		onDeath?.Invoke();
		Destroy(gameObject);
	}
}
