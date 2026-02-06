using UnityEngine;
using System;

public class BarracksGlobalUpgrades : MonoBehaviour
{
    public static BarracksGlobalUpgrades Instance { get; private set; }

    // number of times the upgrade was purchased
    public int PurchaseCount { get; private set; }

    public event Action OnChanged;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddLevel(int amount = 1)
    {
        PurchaseCount += Mathf.Max(0, amount);
        Debug.Log($"[BarracksGlobalUpgrades] AddLevel -> PurchaseCount={PurchaseCount}");
        OnChanged?.Invoke();
    }


    // 0 = extra ally, 1 = hp+heal, 2 = damage
    public int CycleIndex => PurchaseCount % 3;

    // How many times did we hit the "extra ally" step?
    public int ExtraMaxAllies => (PurchaseCount + 2) / 3; // +1 at 1st, +2 at 4th, +3 at 7th...

    // These activate when you’ve bought enough to reach their cycle step at least once
    public bool HpUnlocked => PurchaseCount >= 2;
    public bool DamageUnlocked => PurchaseCount >= 3;

    // Tune values here
    public int AllyHpBonus => HpUnlocked ? 50 : 0;
    public int AllyDamageBonus => DamageUnlocked ? 20 : 0;

    // Heal survivors each wave once HP step unlocked
    public bool HealAlliesEachWave => HpUnlocked;
}
