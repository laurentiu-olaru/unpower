using UnityEngine;

public class PlayerRangedDamageModifier : MonoBehaviour
{
    public int BonusDamage { get; private set; }

    public void AddBonus(int amount)
    {
        BonusDamage += amount;
    }
}
