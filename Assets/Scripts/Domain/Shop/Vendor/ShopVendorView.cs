using UnityEngine;

public class ShopVendorView : MonoBehaviour
{
    [SerializeField] private GameObject promptObject;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool playerInRange;

    private void Awake()
    {
        if (promptObject != null)
            promptObject.SetActive(false);
    }

    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(interactKey))
        {
            Debug.Log("[ShopVendor] E pressed in range.");

            if (ShopUIController.Instance == null)
            {
                Debug.LogError("[ShopVendor] ShopUIController.Instance is null. Did you add it to the scene?");
                return;
            }

            ShopUIController.Instance.ToggleShop();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        if (promptObject != null) promptObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        if (promptObject != null) promptObject.SetActive(false);

        if (ShopUIController.Instance != null)
            ShopUIController.Instance.CloseShop();
    }
}




//using UnityEngine;

//public class ShopVendorView : MonoBehaviour
//{
//    [Header("UI (runtime resolved)")]
//    [SerializeField] private string shopPanelName = "ShopPanel";

//    [Header("Prompt")]
//    [SerializeField] private GameObject promptObject;

//    [Header("Input")]
//    [SerializeField] private KeyCode interactKey = KeyCode.E;

//    private GameObject shopPanel;
//    private bool playerInRange;
//    private bool shopOpen;

//    private void Start()
//    {
//        // Find the panel in the scene
//        shopPanel = GameObject.Find(shopPanelName);

//        if (shopPanel == null)
//        {
//            Debug.LogError($"[ShopVendor] Could not find panel '{shopPanelName}' in scene.");
//            return;
//        }

//        shopPanel.SetActive(false);

//        if (promptObject != null)
//            promptObject.SetActive(false);
//    }

//    private void Update()
//    {
//        if (!playerInRange || shopPanel == null) return;

//        if (Input.GetKeyDown(interactKey))
//        {
//            shopOpen = !shopOpen;
//            shopPanel.SetActive(shopOpen);

//            Debug.Log($"[ShopVendor] Toggle shop: {shopOpen}");
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (!other.CompareTag("Player")) return;

//        playerInRange = true;
//        if (promptObject != null) promptObject.SetActive(true);
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (!other.CompareTag("Player")) return;

//        playerInRange = false;
//        if (promptObject != null) promptObject.SetActive(false);
//    }
//}


//using UnityEngine;

//public class ShopVendorView : MonoBehaviour
//{
//    [Header("UI")]
//    [SerializeField] private GameObject shopPanel;
//    [SerializeField] private GameObject promptObject;

//    [Header("Input")]
//    [SerializeField] private KeyCode interactKey = KeyCode.E;

//    private bool playerInRange;
//    private bool shopOpen;

//    private void Awake()
//    {
//        if (shopPanel != null)
//            shopPanel.SetActive(false);

//        if (promptObject != null)
//            promptObject.SetActive(false);
//    }

//    private void Update()
//    {
//        if (!playerInRange) return;

//        if (Input.GetKeyDown(interactKey))
//        {
//            shopOpen = !shopOpen;

//            Debug.Log($"[ShopVendor] Toggle shop: {shopOpen}");

//            if (shopPanel != null)
//                shopPanel.SetActive(shopOpen);
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (!other.CompareTag("Player")) return;

//        Debug.Log("[ShopVendor] Player entered shop range");

//        playerInRange = true;

//        if (promptObject != null)
//            promptObject.SetActive(true);
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (!other.CompareTag("Player")) return;

//        Debug.Log("[ShopVendor] Player exited shop range");

//        playerInRange = false;

//        if (promptObject != null)
//            promptObject.SetActive(false);

//        // DO NOT auto-close the shop here
//        // shopPanel.SetActive(false);
//        // shopOpen = false;
//    }
//}
