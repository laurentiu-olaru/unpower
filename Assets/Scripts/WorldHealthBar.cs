using UnityEngine;
using UnityEngine.UI;

public class WorldHealthBar : MonoBehaviour
{
	public HealthView target;
	public Image fillImage;
	public Vector3 offset = new Vector3(0, 1f, 0);

	void LateUpdate()
	{
		if (target == null) return;

		transform.position = target.transform.position + offset;
		// Cast to float before dividing. Without the cast, C# performs integer
		// division (e.g. 75 / 100 = 0), which made the health bar snap instantly
		// between full and empty with no intermediate states.
		fillImage.fillAmount = (float)target.Health.CurrentHP / target.maxHP;
	}
}
