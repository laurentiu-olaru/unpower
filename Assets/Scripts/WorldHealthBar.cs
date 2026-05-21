using UnityEngine;
using UnityEngine.UI;

public class WorldHealthBar : MonoBehaviour
{
	public HealthView target;
	public Image fillImage;
	public Vector3 offset = new Vector3(0, 1f, 0);

	[Tooltip("Z offset applied to the health bar's world position. " +
	         "If your Canvas uses Screen Space - Camera and you see a 'Screen position out of " +
	         "view frustum' warning, set this to a small negative value (e.g. -0.5) so the bar " +
	         "sits slightly in front of the camera's near clip plane. " +
	         "No change needed if the Canvas is Screen Space - Overlay.")]
	public float zOffset = 0f;

	void LateUpdate()
	{
		if (target == null) return;

		Vector3 pos = target.transform.position + offset;
		pos.z += zOffset;
		transform.position = pos;
		// Cast to float before dividing. Without the cast, C# performs integer
		// division (e.g. 75 / 100 = 0), which made the health bar snap instantly
		// between full and empty with no intermediate states.
		fillImage.fillAmount = (float)target.Health.CurrentHP / target.maxHP;
	}
}
