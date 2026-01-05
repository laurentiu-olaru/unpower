using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
	public int maxHP = 100;
	public int currentHP;

	public UnityEvent onDeath;
	public PlayerHealthUI healthUIScript;

    [Header("Game Over Settings")]
    public GameObject gameOverUI; // Drag your GameOverPanel here

    void Start()
    {
        currentHP = maxHP;
        // Ensure game is running at normal speed when starting
        Time.timeScale = 1f;
    }

    void Awake()
	{
		currentHP = maxHP;
	}

	public void TakeDamage(int amount)
	{
		currentHP -= amount;
		currentHP = Mathf.Clamp(currentHP, 0, maxHP);
		if (healthUIScript != null)
		{
			float currentFill = currentHP / maxHP;
		}
		if (currentHP <= 0)
		Die();
	}
	public void Heal(int amount)
	{
		currentHP += amount;

		// Clamp health so it doesn't go over the maximum
		if (currentHP > maxHP)
		{
			currentHP = maxHP;
		}

		// Update the UI bar so we see the heal happen
		if (healthUIScript != null)
		{
			float currentFill = currentHP / maxHP;
			//healthUIScript.UpdateHealthBar(currentFill);
		}

		Debug.Log("Healed! Current HP: " + currentHP);
	}

	void Die()
	{
		onDeath?.Invoke();
        Debug.Log("Player Died!");

        // 1. Show the Game Over Screen
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        // 2. Freeze the game
        Time.timeScale = 0f;

        // 3. INSTEAD OF DESTROY: Disable visuals and controls
        // This hides the player but keeps the script running
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PlayerController>().enabled = false;

        // Optional: If you have a collider, disable it so enemies stop bumping into a ghost
        if (GetComponent<Collider2D>() != null)
            GetComponent<Collider2D>().enabled = false;

    }

    public void RestartGame()
    {
        // Important: Reset time scale so the next game actually moves!
        Time.timeScale = 1f;

        // Reloads the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
