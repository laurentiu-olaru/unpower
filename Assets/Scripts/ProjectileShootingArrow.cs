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
        //if (other.CompareTag("Player")) return; // Ignore player
		if (!other.CompareTag("Enemy")) return;

		IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

}
