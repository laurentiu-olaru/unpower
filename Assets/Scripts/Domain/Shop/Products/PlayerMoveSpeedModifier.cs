using UnityEngine;

public class PlayerMoveSpeedModifier : MonoBehaviour
{
	public float BonusSpeed { get; private set; }

	public void AddBonus(float amount)
	{
		BonusSpeed += amount;
	}
}
