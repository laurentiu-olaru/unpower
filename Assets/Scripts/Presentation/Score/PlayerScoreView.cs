using UnityEngine;
using TMPro;

public class PlayerScoreView : MonoBehaviour
{
    public TMP_Text scoreText;
    public ScoreComponent Score { get; private set; }

    void Awake()
    {
        Score = new ScoreComponent();
        Score.OnScoreChanged += UpdateUI;
        UpdateUI(0);
    }

    void UpdateUI(int value)
    {
        scoreText.text = $"Score: {value}";
    }
}
