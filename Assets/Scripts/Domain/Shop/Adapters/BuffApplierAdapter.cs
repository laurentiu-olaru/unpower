using System.Collections;
using UnityEngine;

public class BuffApplierAdapter : MonoBehaviour, IBuffApplier
{
    public PlayerUpgradesAdapter upgrades;

    public void Apply(string buffId, float durationSeconds)
    {
        if (upgrades == null) return;

        StartCoroutine(ApplyRoutine(buffId, durationSeconds));
    }

    private IEnumerator ApplyRoutine(string buffId, float durationSeconds)
    {
        // Example: temporary fire rate boost
        if (buffId == "buff_firerate_2x")
        {
            upgrades.AddFireRateMultiplier(2f);
            yield return new WaitForSeconds(durationSeconds);
            upgrades.AddFireRateMultiplier(0.5f);
            yield break;
        }

        Debug.LogWarning($"Unknown buff id: {buffId}");
    }
}
