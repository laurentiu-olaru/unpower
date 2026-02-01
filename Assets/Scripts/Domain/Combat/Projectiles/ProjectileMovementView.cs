using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileMovementView : MonoBehaviour
{
	private Rigidbody2D rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	public void SetVelocity(Vector2 dir, float speed)
	{
		rb.linearVelocity = dir.normalized * speed;
	}
}
