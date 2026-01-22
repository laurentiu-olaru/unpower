using UnityEngine;

public class PlacementController : MonoBehaviour, IPlacementRequestor
{
    [Header("Optional: manual test mode (press B)")]
    [SerializeField] private BuildingDefinitionSO[] debugCycleDefinitions;
    [SerializeField] private KeyCode debugKey = KeyCode.B;

    [Header("Placement Rules")]
    [SerializeField] private LayerMask buildingLayer;
    [SerializeField] private Vector2 overlapBoxSize = new Vector2(2.8f, 2.8f);
    [SerializeField] private float gridSnap = 1f;

    private BuildingDefinitionSO activeDefinition;
    private GameObject ghost;
    private bool isPlacing;
    private bool canPlace;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    // ====== SHOP ENTRY POINT ======
    public void BeginPlacement(IBuildingDefinition definition)
    {
        if (definition is not BuildingDefinitionSO def)
        {
            Debug.LogWarning("Placement requested with non-Unity BuildingDefinitionSO.");
            return;
        }

        BeginPlacement(def);
    }

    // Overload for direct Unity calls (optional)
    public void BeginPlacement(BuildingDefinitionSO definition)
    {
        if (definition == null)
        {
            Debug.LogWarning("BeginPlacement called with null definition.");
            return;
        }

        activeDefinition = definition;
        EnterPlacementMode();
    }

    void Update()
    {
        // ===== Optional debug cycling with B =====
        if (Input.GetKeyDown(debugKey) && debugCycleDefinitions != null && debugCycleDefinitions.Length > 0)
        {
            if (!isPlacing)
            {
                BeginPlacement(debugCycleDefinitions[0]);
            }
            else
            {
                CycleDebugDefinition();
            }
        }

        // Cancel placement
        if (isPlacing && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPlacementMode();
            return;
        }

        if (!isPlacing || ghost == null) return;

        MoveGhostToMouse();
        UpdatePlacementValidityVisual();

        if (Input.GetMouseButtonDown(0) && canPlace)
        {
            ConfirmPlacement();
        }
    }

    private void EnterPlacementMode()
    {
        isPlacing = true;
        CreateGhost();
        MoveGhostToMouse();
        UpdatePlacementValidityVisual();
    }

    private void ExitPlacementMode()
    {
        isPlacing = false;
        activeDefinition = null;

        if (ghost != null)
            Destroy(ghost);

        ghost = null;
        canPlace = false;
    }

    private void CycleDebugDefinition()
    {
        if (debugCycleDefinitions == null || debugCycleDefinitions.Length == 0) return;

        int idx = 0;
        for (int i = 0; i < debugCycleDefinitions.Length; i++)
        {
            if (debugCycleDefinitions[i] == activeDefinition)
            {
                idx = (i + 1) % debugCycleDefinitions.Length;
                break;
            }
        }

        activeDefinition = debugCycleDefinitions[idx];
        RecreateGhost();
    }

    private void RecreateGhost()
    {
        if (ghost != null) Destroy(ghost);
        CreateGhost();
    }

    private void CreateGhost()
    {
        if (activeDefinition == null)
        {
            Debug.LogWarning("CreateGhost called with no active definition.");
            return;
        }

        if (activeDefinition.prefab == null)
        {
            Debug.LogWarning($"BuildingDefinitionSO '{activeDefinition.name}' has no prefab assigned.");
            return;
        }

        ghost = Instantiate(activeDefinition.prefab);

        // Disable collider(s) so the ghost doesn't block itself or interact
        var col = ghost.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // Set ghost visual
        var sr = ghost.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = new Color(0f, 0.5f, 1f, 0.5f);
        }
    }

    private void MoveGhostToMouse()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        float snappedX = Mathf.Round(mousePos.x / gridSnap) * gridSnap;
        float snappedY = Mathf.Round(mousePos.y / gridSnap) * gridSnap;

        ghost.transform.position = new Vector3(snappedX, snappedY, 0f);
    }

    private void UpdatePlacementValidityVisual()
    {
        if (ghost == null) return;

        // Overlap check
        Collider2D hit = Physics2D.OverlapBox(ghost.transform.position, overlapBoxSize, 0f, buildingLayer);

        canPlace = (hit == null);

        var sr = ghost.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = canPlace
                ? new Color(0f, 0.5f, 1f, 0.5f)   // Blue
                : new Color(1f, 0f, 0f, 0.5f);    // Red
        }
    }

    private void ConfirmPlacement()
    {
        if (activeDefinition == null || activeDefinition.prefab == null || ghost == null) return;

        var realBuilding = Instantiate(activeDefinition.prefab, ghost.transform.position, Quaternion.identity);

        // Activate building (your existing behavior)
        foreach (var building in realBuilding.GetComponents<MonoBehaviour>())
        {
            if (building is IBuilding placeable)
                placeable.OnPlaced();
        }

        // Either exit placement or keep placing the same building (you choose)
        ExitPlacementMode();
    }

    // Helpful gizmo for the overlap box
    void OnDrawGizmosSelected()
    {
        if (!isPlacing || ghost == null) return;

        Gizmos.matrix = Matrix4x4.TRS(ghost.transform.position, Quaternion.identity, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, overlapBoxSize);
    }
}
