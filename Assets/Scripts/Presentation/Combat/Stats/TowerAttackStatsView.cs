using UnityEngine;

public class TowerAttackStatsView : MonoBehaviour
{
	[SerializeField] private int baseProjectileDamage = 10;
	public int BaseProjectileDamage => baseProjectileDamage;

	public int BonusProjectileDamage { get; private set; }

	public void AddProjectileDamage(int amount)
	{
		BonusProjectileDamage += amount;
	}

	public int GetProjectileDamage()
	{
		int globalBonus = TowerDamageGlobalUpgrades.Instance != null
			? TowerDamageGlobalUpgrades.Instance.BonusDamage
			: 0;

		return Mathf.Max(0, baseProjectileDamage + BonusProjectileDamage + globalBonus);
	}

}
