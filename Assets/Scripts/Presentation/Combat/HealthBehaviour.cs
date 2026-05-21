using UnityEngine;
using System;
using Unity.InferenceEngine;


public class HealthBehaviour : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHP = 100;
    public HealthComponent Model { get; private set; }
    public event Action<int> Damaged;
    public event Action<int> Healed;
    public event Action Died;

    private void Awake()
    {
        Model = new HealthComponent(maxHP);

        // Do NOT forward Model.Damaged here — TakeDamage already fires
        // the outer Damaged event directly. Forwarding it here as well
        // caused every hit to fire Damaged TWICE (once via this delegate,
        // once via the explicit Damaged?.Invoke in TakeDamage), which made
        // HitFlash play the flash animation double per hit.
        //Model.Damaged += (amount) => Damaged?.Invoke(amount);
        //Model.Healed += (amount) => Healed?.Invoke(amount);
        //Model.Died += () => Died?.Invoke();
    }

    public void TakeDamage(int amount)
    {
        Model.TakeDamage(amount);
        // Fire once — this is the single source of truth for the Damaged event.
        Damaged?.Invoke(amount);
    }
    public void Heal(int amount) => Model.Heal(amount);
    public int CurrentHP => Model.CurrentHP;
    public int MaxHP => Model.MaxHP;

}
