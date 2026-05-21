using UnityEngine;

public class BossDeathView : MonoBehaviour
{
	[SerializeField] private EnemyHealthView health;
	[SerializeField] private Animator animator;
	[SerializeField] private Rigidbody2D rb;

	[Header("Disable on death")]
	[SerializeField] private MonoBehaviour[] disableOnDeath;

	[Header("Animator Params")]
	[SerializeField] private string dieTrigger = "Die";

	private bool hasDied;

	private void Reset()
	{
		health = GetComponentInChildren<EnemyHealthView>(true);
		animator = GetComponentInChildren<Animator>(true);
		rb = GetComponent<Rigidbody2D>();
	}

	private void OnEnable()
	{
		if (health != null)
			health.Died += OnDied;
	}

	private void OnDisable()
	{
		if (health != null)
			health.Died -= OnDied;
	}

	private void OnDied(EnemyHealthView _)
	{
		if (hasDied) return;     //prevents double trigger
		hasDied = true;

		// Stop physics movement and freeze the corpse in place.
		// Setting simulated = false removes the rigidbody from the physics
		// simulation entirely, which prevents other colliders (enemies, projectiles)
		// from shoving the dead boss around after death.
		if (rb != null)
		{
			rb.linearVelocity = Vector2.zero;
			rb.angularVelocity = 0f;
			rb.simulated = false;
		}

		// Disable scripts (movement/attack/targeting)
		if (disableOnDeath != null)
		{
			foreach (var mb in disableOnDeath)
				if (mb != null) mb.enabled = false;
		}

		animator?.SetTrigger(dieTrigger);
	}
}