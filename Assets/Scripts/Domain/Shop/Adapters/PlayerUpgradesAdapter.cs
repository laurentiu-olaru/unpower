using UnityEngine;

public class PlayerUpgradesAdapter : MonoBehaviour, IPlayerUpgrades
{
    [Header("References")]
    public HealthView healthView;

    [Header("Serialized stats for easy change")]
    public int damageBonus = 20;
    public float fireRateMultiplier = 1.2f;

	[SerializeField] private PlayerRangedDamageModifier rangedDamage;
	[SerializeField] private PlayerMoveSpeedModifier moveSpeed;
	void Awake()
	{
		if (rangedDamage == null) rangedDamage = GetComponent<PlayerRangedDamageModifier>();
		if (moveSpeed == null) moveSpeed = GetComponent<PlayerMoveSpeedModifier>();
	}

	public void AddArrowDamage(int amount)
	{
		var stats = GetComponent<PlayerAttackStatsView>();
		if (stats == null)
		{
			Debug.LogWarning("[Upgrades] Missing PlayerAttackStatsView on Player.");
			return;
		}

		stats.AddProjectileDamage(amount);
		Debug.Log($"[Upgrades] Arrow Damage +{amount} (new total dmg={stats.GetProjectileDamage()})");
	}


	public void AddMoveSpeed(float amount)
	{
		if (moveSpeed == null)
		{
			Debug.LogWarning("[Upgrades] Missing PlayerMoveSpeedModifier on Player.");
			return;
		}
		moveSpeed.AddBonus(amount);
		Debug.Log($"[Upgrades] Move Speed +{amount}");
	}
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
	public void AddTowerDamage(int amount)
	{
		if (TowerDamageGlobalUpgrades.Instance == null)
		{
			Debug.LogWarning("[Upgrades] TowerDamageGlobalUpgrades missing in scene.");
			return;
		}

		TowerDamageGlobalUpgrades.Instance.AddBonusDamage(amount);
		Debug.Log($"[Upgrades] Tower Damage +{amount}");
	}
    public void AddBarracksUpgradeLevel(int amount)
    {
        Debug.Log($"[UpgradesAdapter] AddBarracksUpgradeLevel({amount}) called");

        if (BarracksGlobalUpgrades.Instance == null)
        {
            Debug.LogWarning("[Upgrades] BarracksGlobalUpgrades missing in scene.");
            return;
        }

        BarracksGlobalUpgrades.Instance.AddLevel(amount);
        Debug.Log($"[Upgrades] Barracks PurchaseCount={BarracksGlobalUpgrades.Instance.PurchaseCount}");
    }



}
