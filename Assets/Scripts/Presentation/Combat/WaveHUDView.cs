using System.Collections;
using TMPro;
using UnityEngine;

public class WaveHudView : MonoBehaviour
{
    [Header("Wave UI")]
    [SerializeField] private TMP_Text waveText;

    [Header("Break UI")]
    [SerializeField] private GameObject breakPanel;
    [SerializeField] private TMP_Text breakText;

    private Coroutine breakRoutine;

    public void SetWave(int waveNumber)
    {
        if (waveText != null)
            waveText.text = $"Wave: {waveNumber}";
    }

    public void ShowBreak(int seconds)
    {
        if (breakPanel != null)
            breakPanel.SetActive(true);

        StartBreakCountdown(seconds);
    }

    public void HideBreak()
    {
        if (breakRoutine != null)
        {
            StopCoroutine(breakRoutine);
            breakRoutine = null;
        }

        if (breakPanel != null)
            breakPanel.SetActive(false);
    }

    private void StartBreakCountdown(int seconds)
    {
        if (breakRoutine != null)
            StopCoroutine(breakRoutine);

        breakRoutine = StartCoroutine(BreakCountdownRoutine(seconds));
    }

    private IEnumerator BreakCountdownRoutine(int seconds)
    {
        int remaining = Mathf.Max(0, seconds);

        while (remaining > 0)
        {
            if (breakText != null)
                breakText.text = $"Break: {remaining}";

            yield return new WaitForSeconds(1f);
            remaining--;
        }

        if (breakText != null)
            breakText.text = "Break: 0";

        breakRoutine = null;
    }
}
