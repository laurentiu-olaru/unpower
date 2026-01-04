using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float speed = 10f;
	public int damage = 10;
	public float lifetime = 5f;

	void Start()
	{
		Destroy(gameObject, lifetime);
	}

	public void Launch(Vector2 dir)
	{
		GetComponent<Rigidbody2D>().linearVelocity = dir * speed;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		// CHANGED: Look for 'EnemyHealth' instead of 'Health'
		EnemyHealth h = other.GetComponent<EnemyHealth>();

		if (h != null)
		{
			h.TakeDamage(damage);
			Destroy(gameObject); // Destroy the arrow
		}
	}
}
