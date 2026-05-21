using UnityEngine;

/// <summary>
/// Drives the Stone Golem's Animator parameters each frame based on physics state.
///
/// Currently drives:
///   "Speed" (float) — magnitude of the Rigidbody2D velocity.
///     Use this in the Animator to blend between Idle and Walk states:
///     Idle threshold: Speed = 0 (or < ~0.1 to avoid idle-walk flicker)
///     Walk threshold: Speed > 0.1
///
/// Add more parameters here as the animation tree grows (e.g. "IsAttacking", "IsDead").
/// </summary>
public class StoneGolemAnimDriverView : MonoBehaviour
{
	[SerializeField] private Animator animator;
	[SerializeField] private Rigidbody2D rb;

	/// <summary>
	/// Cached hash for the "Speed" Animator parameter.
	/// Using a hash instead of a string avoids a dictionary lookup every frame.
	/// </summary>
	private static readonly int Speed = Animator.StringToHash("Speed");

	private void Reset()
	{
		// Auto-wire when the component is first added in the Inspector
		animator = GetComponentInChildren<Animator>();
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (animator == null) return;

		// rb.linearVelocity.magnitude gives us the actual movement speed from physics.
		// Falls back to 0 if no Rigidbody2D is present (e.g. during editor preview).
		float speed = rb != null ? rb.linearVelocity.magnitude : 0f;
		animator.SetFloat(Speed, speed);
	}
}
