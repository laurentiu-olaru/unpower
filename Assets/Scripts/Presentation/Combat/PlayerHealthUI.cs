using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
	public HealthView playerHealth;
	public Image fillImage;

	void Start()
	{
		if (playerHealth == null || fillImage == null)
		{
			Debug.LogError("PlayerHealthUI: Missing references");
			return;
		}

		playerHealth.Health.OnHealthChanged += UpdateUI;
	}

	void UpdateUI(int current, int max)
	{
		fillImage.fillAmount = (float)current / max;
	}

	void OnDestroy()
	{
		if (playerHealth != null)
			playerHealth.Health.OnHealthChanged -= UpdateUI;
	}
}
