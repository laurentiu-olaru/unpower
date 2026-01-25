using System;
using UnityEngine;


public class HealthView : MonoBehaviour, IDamageable
{

    public event Action<int> Damaged;

    public int maxHP = 100;
	public GameObject gameOverUI;

	public HealthComponent Health => health;

	private HealthComponent health;

	void Awake()
	{
		health = new HealthComponent(maxHP);
		health.OnDied += HandleDeath;
	}

    public void TakeDamage(int amount)
    {
        health.TakeDamage(amount);
        Damaged?.Invoke(amount); 
    }

    public void Heal(int amount) => health.Heal(amount);

	void HandleDeath()
	{
		if (gameOverUI != null)
			gameOverUI.SetActive(true);

		Time.timeScale = 0f;

		if (TryGetComponent(out SpriteRenderer sr))
			sr.enabled = false;

		if (TryGetComponent(out PlayerController pc))
			pc.enabled = false;

		if (TryGetComponent(out Collider2D col))
			col.enabled = false;
	}
}
