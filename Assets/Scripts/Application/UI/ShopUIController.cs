using UnityEngine;

public class ShopUIController : MonoBehaviour
{
    public static ShopUIController Instance { get; private set; }

    [SerializeField] private GameObject shopPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (shopPanel != null)
            shopPanel.SetActive(false);
    }

    public void ToggleShop()
    {
        if (shopPanel == null)
        {
            Debug.LogError("[ShopUIController] shopPanel not assigned.");
            return;
        }

        shopPanel.SetActive(!shopPanel.activeSelf);
    }

    public void CloseShop()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);
    }
}
