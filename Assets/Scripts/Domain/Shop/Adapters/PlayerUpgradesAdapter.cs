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

        // TODO: HealthComponent.MaxHP is currently read-only. To properly support a
        // max-HP upgrade you need to add a SetMaxHP(int) or IncreaseMaxHP(int) method
        // to HealthComponent. Until then, we heal by 'amount' as a stopgap so the
        // upgrade at least has a positive effect.
        // Previously this healed by (amount + 100), inflating the heal amount by a
        // hardcoded 100 that was leftover from a test and never removed.
        Debug.LogWarning("MaxHP upgrade requested, but HealthComponent.MaxHP is readonly. Healing instead as a temporary workaround.");
        healthView.Heal(amount);
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
