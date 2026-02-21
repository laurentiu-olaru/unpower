using UnityEngine;

public class StoneGolemAnimDriverView : MonoBehaviour
{
	[SerializeField] private Animator animator;
	[SerializeField] private Rigidbody2D rb;

	private static readonly int Speed = Animator.StringToHash("Speed");

	private void Reset()
	{
		animator = GetComponentInChildren<Animator>();
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (animator == null) return;

		// If you use Rigidbody2D movement later, this works.
		// If you move via transform, rb may be null -> fallback to 0.
		float speed = rb != null ? rb.linearVelocity.magnitude : 0f;
		animator.SetFloat(Speed, speed);
	}
}
