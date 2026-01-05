using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object is the Player
        if (other.CompareTag("Player"))
        {
            // Find the PlayerScore script on the player
            PlayerScore ps = other.GetComponent<PlayerScore>();

            if (ps != null)
            {
                ps.AddScore(coinValue);
                Destroy(gameObject); // Make the coin disappear
            }
        }
    }
}