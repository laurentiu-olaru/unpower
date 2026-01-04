using UnityEngine;

public class HeartItem : MonoBehaviour
{
	public int healAmount = 30;

	void OnTriggerEnter2D(Collider2D other)
	{
		// Check if the object that touched the heart is the Player
		if (other.CompareTag("Player"))
		{
			Health playerHealth = other.GetComponent<Health>();

			if (playerHealth != null)
			{
				// Call the heal function we just made
				playerHealth.Heal(healAmount);

				// Destroy the heart so it can't be picked up again
				Destroy(gameObject);
			}
		}
	}
	void Update()
	{
		// Floating effect
		float newY = Mathf.Sin(Time.time * 5f) * 0.1f;
		transform.position = new Vector3(transform.position.x, transform.position.y + newY * Time.deltaTime, transform.position.z);
	}
}