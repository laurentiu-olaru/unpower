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
		return Mathf.Max(0, baseProjectileDamage + BonusProjectileDamage);
	}
}
