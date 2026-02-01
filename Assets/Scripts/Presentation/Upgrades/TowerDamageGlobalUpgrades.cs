using UnityEngine;

public class TowerDamageGlobalUpgrades : MonoBehaviour
{
	public static TowerDamageGlobalUpgrades Instance { get; private set; }

	public int BonusDamage { get; private set; }

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
	}

	public void AddBonusDamage(int amount)
	{
		BonusDamage += amount;
	}
}
