using UnityEngine;

[CreateAssetMenu(menuName = "Game/Bosses/Boss Difficulty Profile")]
public class BossDifficultyProfileSO : ScriptableObject
{
	[Header("Base multipliers (applied always)")]
	[Min(0f)] public float BaseHpMultiplier = 5f;
	[Min(0f)] public float BaseSpeedMultiplier = 1f;
	[Min(0f)] public float BaseDamageMultiplier = 2f;

	[Header("Optional per-wave scaling curves (x-axis = wave number)")]
	public AnimationCurve HpCurve = AnimationCurve.Linear(1, 1, 100, 1);
	public AnimationCurve SpeedCurve = AnimationCurve.Linear(1, 1, 100, 1);
	public AnimationCurve DamageCurve = AnimationCurve.Linear(1, 1, 100, 1);

	public (float hp, float speed, float damage) Evaluate(int waveNumber)
	{
		float hp = BaseHpMultiplier * HpCurve.Evaluate(waveNumber);
		float sp = BaseSpeedMultiplier * SpeedCurve.Evaluate(waveNumber);
		float dmg = BaseDamageMultiplier * DamageCurve.Evaluate(waveNumber);
		return (hp, sp, dmg);
	}
}