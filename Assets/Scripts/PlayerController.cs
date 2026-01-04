using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

	public float speed = 5f;

	private Rigidbody2D rb;
	public GameObject projectilePrefab;
	public Transform firePoint;
	public float maxHealth = 100f;
	private float currentHealth;

	// Use your existing PlayerHealthUI script to update the bar
	private PlayerHealthUI healthUI;

	void Start()
	{
		currentHealth = maxHealth;
		healthUI = GetComponentInParent<PlayerHealthUI>();
	}

	public void TakeDamage(float damage)
	{
		currentHealth -= damage;

		// Update the UI bar you created earlier
		if (healthUI != null)
		{
			// Assuming your PlayerHealthUI has a method like this
			// or just update the fill directly if you prefer
		}

		if (currentHealth <= 0)
		{
			Debug.Log("Player Dead!");
			// Add Game Over logic here
		}
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
			Shoot();
	}

	void Shoot()
	{
		Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouse.z = 0;

		Vector2 dir = (mouse - firePoint.position).normalized;

		GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
		proj.GetComponent<Projectile>().Launch(dir);
	}

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		Vector2 dir = new Vector2(h, v);
		if (Mathf.Abs(h) > 0 && Mathf.Abs(v) > 0)
			dir *= 0.7071f; // normalize diagonals
		rb.linearVelocity = dir * speed;

	}
}
