using UnityEngine;
using UnityEngine.UI;

public class WorldHealthBar : MonoBehaviour
{
	public Health target;
	public Image fillImage;
	public Vector3 offset = new Vector3(0, 1f, 0);

	void LateUpdate()
	{
		if (target == null) return;

		transform.position = target.transform.position + offset;
		fillImage.fillAmount = (float)target.currentHP / target.maxHP;
	}
}
