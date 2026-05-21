using UnityEngine;

/// <summary>
/// ScriptableObject that controls how tough bosses are, independently of normal enemy difficulty.
/// Create via: Assets > Create > Game > Bosses > Boss Difficulty Profile
///
/// Multipliers are applied ON TOP of the boss prefab's base stats via EnemyDifficultyApplierView.
/// The base multipliers give the boss a flat boost, while the AnimationCurves allow the
/// boss to scale up further as the player reaches higher wave numbers.
///
/// Example: wave 20, BaseHpMultiplier = 5, HpCurve at wave 20 = 1.5
///   → final HP multiplier = 5 × 1.5 = 7.5× the prefab's base HP
///
/// Tip: keep the default AnimationCurves flat (value = 1) if you just want a
/// constant difficulty boost — the curve only matters for scaling across many waves.
/// </summary>
[CreateAssetMenu(menuName = "Game/Bosses/Boss Difficulty Profile")]
public class BossDifficultyProfileSO : ScriptableObject
{
	[Header("Base multipliers (applied always, regardless of wave)")]
	[Tooltip("Multiplies the boss prefab's base HP. 5 = 5× the normal HP.")]
	[Min(0f)] public float BaseHpMultiplier = 5f;

	[Tooltip("Multiplies the boss's movement speed.")]
	[Min(0f)] public float BaseSpeedMultiplier = 1f;

	[Tooltip("Multiplies the boss's attack damage.")]
	[Min(0f)] public float BaseDamageMultiplier = 2f;

	[Header("Optional per-wave scaling (x-axis = wave number, y-axis = extra multiplier)")]
	[Tooltip("Multiplied with BaseHpMultiplier. Default flat curve (y=1) = no scaling.")]
	public AnimationCurve HpCurve = AnimationCurve.Linear(1, 1, 100, 1);

	[Tooltip("Multiplied with BaseSpeedMultiplier. Default flat curve (y=1) = no scaling.")]
	public AnimationCurve SpeedCurve = AnimationCurve.Linear(1, 1, 100, 1);

	[Tooltip("Multiplied with BaseDamageMultiplier. Default flat curve (y=1) = no scaling.")]
	public AnimationCurve DamageCurve = AnimationCurve.Linear(1, 1, 100, 1);

	/// <summary>
	/// Returns the final (hp, speed, damage) multipliers for a given wave number.
	/// Called by BossWaveCoordinatorView just before spawning a boss.
	/// </summary>
	public (float hp, float speed, float damage) Evaluate(int waveNumber)
	{
		float hp  = BaseHpMultiplier     * HpCurve.Evaluate(waveNumber);
		float sp  = BaseSpeedMultiplier  * SpeedCurve.Evaluate(waveNumber);
		float dmg = BaseDamageMultiplier * DamageCurve.Evaluate(waveNumber);
		return (hp, sp, dmg);
	}
}
