using System;
using UnityEngine;


public class HealthView : MonoBehaviour, IDamageable
{

    public event Action<int> Damaged;

    public int maxHP = 100;
	public GameObject gameOverUI;

	public HealthComponent Health => health;

	private HealthComponent health;


    [Header("Audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip hitClip;
    [SerializeField, Range(0f, 1f)] private float hitVolume = 1f;

    [Header("Death Audio")]
    [SerializeField] private AudioClip deathClip;
    [SerializeField, Range(0f, 1f)] private float deathVolume = 0.8f;


    void Awake()
	{
		health = new HealthComponent(maxHP);
		health.OnDied += HandleDeath;
	}

    public void TakeDamage(int amount)
    {
        health.TakeDamage(amount);
        Damaged?.Invoke(amount);
        if (hitClip != null)
        {
            if (sfxSource != null)
                sfxSource.PlayOneShot(hitClip, hitVolume);
            else
                AudioSource.PlayClipAtPoint(hitClip, transform.position, hitVolume);
        }
    }


    public void Heal(int amount) => health.Heal(amount);

	void HandleDeath()
	{
        if (deathClip != null)
            AudioSource.PlayClipAtPoint(deathClip, transform.position, deathVolume);

        if (gameOverUI != null)
			gameOverUI.SetActive(true);

		Time.timeScale = 0.005f;//was 0 before

		if (TryGetComponent(out SpriteRenderer sr))
			sr.enabled = false;

		if (TryGetComponent(out PlayerController pc))
			pc.enabled = false;

		if (TryGetComponent(out Collider2D col))
			col.enabled = false;
	}
}
