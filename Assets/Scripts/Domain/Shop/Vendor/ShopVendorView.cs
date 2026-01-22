using UnityEngine;

public class ShopVendorView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject shopPanel;    // a Canvas panel in your scene
    [SerializeField] private GameObject promptObject; // optional "Press E" world text

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool playerInRange;

    void Awake()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);

        if (promptObject != null)
            promptObject.SetActive(false);
    }

    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(interactKey))
        {
            if (shopPanel != null)
                shopPanel.SetActive(!shopPanel.activeSelf);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        if (promptObject != null) promptObject.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        if (promptObject != null) promptObject.SetActive(false);

        if (shopPanel != null) shopPanel.SetActive(false);
    }
}
