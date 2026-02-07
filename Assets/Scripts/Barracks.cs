using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Barracks : MonoBehaviour, IBuilding
{
	public GameObject allyPrefab;
	public Transform spawnPoint; // Create an empty child object on the prefab for this
	public int maxAllies = 3;
	public float spawnInterval = 120f; // 2 minutes in seconds

	private List<GameObject> activeAllies = new List<GameObject>();
	private bool isPlaced = false;

    private BarracksGlobalUpgrades upgrades;



    // This is called by your PlacementManager when the building is built
    public void OnPlaced()
    {
        isPlaced = true;

        upgrades = BarracksGlobalUpgrades.Instance;
        if (upgrades != null)
            upgrades.OnChanged += HandleUpgradesChanged;

        // Apply upgrades immediately on placement too (important for newly placed barracks)
        ApplyUpgradesNow();

        SpawnWave();
        StartCoroutine(SpawnRoutine());
    }

	IEnumerator SpawnRoutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(spawnInterval);

			if (isPlaced)
			{
				SpawnWave();
			}
		}
	}

    void SpawnWave()
    {
        activeAllies.RemoveAll(item => item == null);

        var upgrades = BarracksGlobalUpgrades.Instance;

        int effectiveMaxAllies = maxAllies + (upgrades != null ? upgrades.ExtraMaxAllies : 0);

        if (upgrades != null && upgrades.HealAlliesEachWave)
            HealSurvivors();

        int spaceLeft = effectiveMaxAllies - activeAllies.Count;

        for (int i = 0; i < spaceLeft; i++)
            SpawnAlly();
    }


    void HealSurvivors()
	{
		for (int i = 0; i < activeAllies.Count; i++)
		{
			var ally = activeAllies[i];
			if (ally == null) continue;

			if (ally.TryGetComponent(out IHealable healable))
				healable.HealToFull();
		}
	}

    void SpawnAlly()
    {
        GameObject newAlly = Instantiate(allyPrefab, spawnPoint.position, Quaternion.identity);

        var upgrades = BarracksGlobalUpgrades.Instance;

        // Apply ally HP bonus + full heal on spawn (only after HP unlock)
        if (upgrades != null && upgrades.HpUnlocked)
        {
            if (newAlly.TryGetComponent(out AllyHealthView health))
                health.AddMaxHp(upgrades.AllyHpBonus, healToFull: true);

        }

        // Apply ally damage bonus (only after damage unlock)
        if (upgrades != null && upgrades.DamageUnlocked)
        {
            if (newAlly.TryGetComponent(out AllyMeleeAI aiForDamage))
                aiForDamage.attackDamage += upgrades.AllyDamageBonus;
        }

        // Existing homeBase wiring
        AllyMeleeAI ai = newAlly.GetComponent<AllyMeleeAI>();
        if (ai != null)
            ai.homeBase = this.transform;

        var up = BarracksGlobalUpgrades.Instance;
        if (up != null)
        {
            var stamp = newAlly.GetComponent<AllyUpgradeStamp>();
            if (stamp == null) stamp = newAlly.AddComponent<AllyUpgradeStamp>();

            if (up.HpUnlocked && newAlly.TryGetComponent(out AllyHealth health))
            {
                health.ApplyMaxHealthBonusOnce(up.AllyHpBonus, stamp);
                if (up.HealAlliesEachWave) health.HealToFull();
            }

            if (up.DamageUnlocked && newAlly.TryGetComponent(out AllyMeleeAI newAI))
            {
                ApplyDamageBonusOnce(newAI, up.AllyDamageBonus, stamp);
            }
        }


        activeAllies.Add(newAlly);
    }

    void OnDestroy()
    {
        if (upgrades != null)
            upgrades.OnChanged -= HandleUpgradesChanged;
    }
    private void HandleUpgradesChanged()
    {
        if (!isPlaced) return;

        ApplyUpgradesNow();
    }

    private void ApplyUpgradesNow()
    {
        var up = BarracksGlobalUpgrades.Instance;
        if (up == null) return;

        // Clean dead allies from list
        activeAllies.RemoveAll(a => a == null);

        // Apply bonuses to EXISTING alive allies
        ApplyBonusesToExistingAllies(up);

        // Heal existing allies if unlocked
        if (up.HealAlliesEachWave)
            HealSurvivors();

        // Spawn any missing allies immediately to reach new max
        SpawnToMax(up);
    }

    private void ApplyBonusesToExistingAllies(BarracksGlobalUpgrades up)
    {
        for (int i = 0; i < activeAllies.Count; i++)
        {
            var ally = activeAllies[i];
            if (ally == null) continue;

            // HP bonus (apply to existing too)
            if (up.HpUnlocked && ally.TryGetComponent(out AllyHealth health))
            {
                // We need to apply the maxHP bonus once per ally, not every time upgrades change.
                // So we store applied bonuses on the ally (next step).
                var stamp = ally.GetComponent<AllyUpgradeStamp>();
                if (stamp == null) stamp = ally.AddComponent<AllyUpgradeStamp>();

                health.ApplyMaxHealthBonusOnce(up.AllyHpBonus, stamp);
            }

            // Damage bonus (apply to existing too)
            if (up.DamageUnlocked && ally.TryGetComponent(out AllyMeleeAI ai))
            {
                var stamp = ally.GetComponent<AllyUpgradeStamp>();
                if (stamp == null) stamp = ally.AddComponent<AllyUpgradeStamp>();

                ApplyDamageBonusOnce(ai, up.AllyDamageBonus, stamp);
            }
        }
    }

    private void ApplyDamageBonusOnce(AllyMeleeAI ai, int bonus, AllyUpgradeStamp stamp)
    {
        int toApply = bonus - stamp.AppliedDamageBonus;
        if (toApply <= 0) return;

        ai.attackDamage += toApply;
        stamp.AppliedDamageBonus += toApply;
    }

    private void SpawnToMax(BarracksGlobalUpgrades up)
    {
        int effectiveMaxAllies = maxAllies + up.ExtraMaxAllies;
        int spaceLeft = effectiveMaxAllies - activeAllies.Count;

        for (int i = 0; i < spaceLeft; i++)
            SpawnAlly(); // SpawnAlly will also apply bonuses to new ally
    }


}