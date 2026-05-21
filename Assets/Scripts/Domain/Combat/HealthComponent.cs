using System;
using UnityEngine;

/// <summary>
/// Pure C# (non-MonoBehaviour) model for a health pool.
/// Tracks CurrentHP and MaxHP, fires events on damage, death, and heal,
/// and can be embedded inside any MonoBehaviour (HealthBehaviour, HealthView,
/// AllyHealthView, EnemyHealthView, etc.) without inheriting from MonoBehaviour itself.
///
/// Event summary:
///   OnDied          — fires once when CurrentHP reaches 0
///   OnHealthChanged — fires on every HP change: (currentHP, maxHP)
///   Damaged         — fires on every TakeDamage call: (amount)
///                     Used by HitFlash to trigger the sprite flash animation.
/// </summary>
public class HealthComponent
{
    /// <summary>The maximum HP ceiling. Can be raised via IncreaseMaxHp.</summary>
    public int MaxHP { get; private set; }

    /// <summary>Current hit points. Clamped to [0, MaxHP]. Never goes negative.</summary>
    public int CurrentHP { get; private set; }

    /// <summary>Fires once when CurrentHP drops to 0. Does NOT fire on subsequent calls.</summary>
    public event Action OnDied;

    /// <summary>Fires after every HP change. Args: (currentHP, maxHP).</summary>
    public event Action<int, int> OnHealthChanged;

    /// <summary>Fires on every TakeDamage call (even if it doesn't kill). Args: (damageAmount).</summary>
    public event Action<int> Damaged;

    /// <summary>Creates a new HealthComponent at full health.</summary>
    public HealthComponent(int maxHP)
    {
        MaxHP = maxHP;
        CurrentHP = maxHP;
    }

    /// <summary>
    /// Reduces CurrentHP by <paramref name="amount"/>, clamped to 0.
    /// No-ops if already dead. Fires Damaged, OnHealthChanged, and OnDied as appropriate.
    /// </summary>
    public void TakeDamage(int amount)
    {
        // Ignore damage on an already-dead entity to prevent double-death events
        if (CurrentHP <= 0) return;

        CurrentHP = Math.Max(0, CurrentHP - amount);
        OnHealthChanged?.Invoke(CurrentHP, MaxHP);
        Damaged?.Invoke(amount);

        // Fire death event exactly once, when HP first hits 0
        if (CurrentHP == 0)
            OnDied?.Invoke();
    }

    /// <summary>
    /// Restores <paramref name="amount"/> HP, capped at MaxHP.
    /// No-ops on dead entities to prevent resurrection.
    /// </summary>
    public void Heal(int amount)
    {
        if (CurrentHP <= 0) return;

        CurrentHP = Math.Min(MaxHP, CurrentHP + amount);
        OnHealthChanged?.Invoke(CurrentHP, MaxHP);
    }

    /// <summary>
    /// Permanently increases MaxHP. Optionally heals to the new maximum.
    /// If not healing to full, CurrentHP is unchanged unless it already exceeded the new max.
    /// </summary>
    /// <param name="amount">How much to add to MaxHP. Ignored if &lt;= 0.</param>
    /// <param name="healToFull">If true, sets CurrentHP = new MaxHP after the increase.</param>
    public void IncreaseMaxHp(int amount, bool healToFull)
    {
        if (amount <= 0) return;

        MaxHP += amount;

        if (healToFull)
            CurrentHP = MaxHP;
        else
            CurrentHP = Math.Min(CurrentHP, MaxHP); // keep CurrentHP valid

        OnHealthChanged?.Invoke(CurrentHP, MaxHP);
    }
}
