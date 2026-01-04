using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
	public Health playerHealth;
	public Image fillImage;

	void Update()
	{
		fillImage.fillAmount = (float)playerHealth.currentHP / playerHealth.maxHP;
	}
}
