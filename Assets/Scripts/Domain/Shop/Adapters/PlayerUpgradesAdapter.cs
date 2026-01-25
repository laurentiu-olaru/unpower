using UnityEngine;

public class PlayerUpgradesAdapter : MonoBehaviour, IPlayerUpgrades
{
    [Header("References")]
    public HealthView healthView;

    [Header("Serialized stats for easy change")]
    public int damageBonus = 5;
    public float fireRateMultiplier = 1.2f;

    public void AddMaxHp(int amount)
    {
        if (healthView == null) return;

        // Your HealthComponent currently has MaxHP read-only,
        // so we cannot actually increase max HP without upgrading HealthComponent design.
        // For now, we can warn or apply a workaround (NOT recommended).
        Debug.LogWarning("MaxHP upgrade requested, but HealthComponent.MaxHP is readonly. Upgrade HealthComponent to support max HP changes.");
        healthView.Heal(amount+100); // Workaround: heal the player instead of increasing max HP.
    }

    public void AddDamage(int amount)
    {
        damageBonus += amount;
    }

    public void AddFireRateMultiplier(float multiplier)
    {
        if (multiplier <= 0f) return;
        fireRateMultiplier *= multiplier;
    }
}
