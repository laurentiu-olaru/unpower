using UnityEngine;
using TMPro;

public class BuildMenuController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode toggleKey = KeyCode.B;
    [SerializeField] private KeyCode closeKey = KeyCode.Escape;

    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text feedbackText;

    [Header("Dependencies")]
    [SerializeField] private PlayerScoreView scoreView;
    [SerializeField] private ShopView shopView;

    [Header("Optional gating (set to 0 to disable)")]
    [SerializeField] private int minScoreToOpen = 0; //to change for gating

    private void Awake()
    {
        if (panel != null)
            panel.SetActive(false);

        if (shopView != null)
            shopView.OnPurchaseSucceeded += Close;
    }

    private void OnDestroy()
    {
        if (shopView != null)
            shopView.OnPurchaseSucceeded -= Close;
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            Toggle();
        }

        if (Input.GetKeyDown(closeKey) && panel != null && panel.activeSelf)
        {
            Close();
        }
    }

    public void Toggle()
    {
        if (panel == null) return;

        if (!panel.activeSelf)
        {
            if (!CanOpen())
                return;

            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
        }
    }

    public void Close()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    private bool CanOpen()
    {
        if (minScoreToOpen <= 0) return true;

        if (scoreView == null || scoreView.Score == null)
        {
            if (feedbackText != null)
                feedbackText.text = "Build menu missing ScoreView reference.";
            return false;
        }

        if (scoreView.Score.Score < minScoreToOpen)
        {
            if (feedbackText != null)
                feedbackText.text = $"Need at least {minScoreToOpen} score to open build menu.";
            return false;
        }

        return true;
    }
}
