using System;

public interface IHealth
{
    int CurrentHp { get; }
    int MaxHp { get; }

    event Action<int> Damaged; //amount
    event Action Died;
}
