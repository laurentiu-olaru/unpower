using UnityEngine;
using TMPro;

public class BuildMenuController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode toggleKey = KeyCode.B;

    [Header("Gating")]
    [SerializeField] private int minScoreToOpen = 1; // set to your Shop building cost, e.g. 50

    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text feedbackText;

    [Header("Currency")]
    [SerializeField] private PlayerScoreView scoreView;

    private void Awake()
    {
        if (panel != null) panel.SetActive(false);
    }

    private void Update()
    {
        if (!Input.GetKeyDown(toggleKey)) return;

        if (panel == null || scoreView == null || scoreView.Score == null) return;

        if (scoreView.Score.Score < minScoreToOpen)
        {
            if (feedbackText != null)
                feedbackText.text = $"Need at least {minScoreToOpen} score to open build menu.";
            return;
        }

        panel.SetActive(!panel.activeSelf);
    }
}
