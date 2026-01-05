using UnityEngine;
using TMPro; // Crucial: This allows the script to talk to TextMeshPro

public class PlayerScore : MonoBehaviour
{
    private int score = 0;

    // Drag your ScoreText object here in the Inspector
    public TMP_Text scoreTextUI;

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
        Debug.Log("Score Updated: " + score);
    }

    void UpdateScoreUI()
    {
        if (scoreTextUI != null)
        {
            scoreTextUI.text = "Score: " + score.ToString();
        }
    }
}