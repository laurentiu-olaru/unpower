using System;

public class HealthComponent
{
    public int MaxHP { get; }
    public int CurrentHP { get; private set; }

    public event Action OnDied;
    public event Action<int, int> OnHealthChanged;

    public HealthComponent(int maxHP)
    {
        MaxHP = maxHP;
        CurrentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        if (CurrentHP <= 0) return;

        CurrentHP = Math.Max(0, CurrentHP - amount);
        OnHealthChanged?.Invoke(CurrentHP, MaxHP);

        if (CurrentHP == 0)
            OnDied?.Invoke();
    }

    public void Heal(int amount)
    {
        if (CurrentHP <= 0) return;

        CurrentHP = Math.Min(MaxHP, CurrentHP + amount);
        OnHealthChanged?.Invoke(CurrentHP, MaxHP);
    }
}
