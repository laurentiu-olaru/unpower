using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GridManager gridManager; // Drag the GridManager object here in Inspector

    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        // Start at current position
        targetPosition = transform.position;
    }

    void Update()
    {
        // If we are already moving, just keep sliding toward the target
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
            return;
        }

        // Get Input (WASD)
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float vertical = Input.GetAxisRaw("Vertical");     // W/S or Up/Down

        // Prevent diagonal movement for now to keep it simple
        if (horizontal != 0) vertical = 0;

        if (horizontal != 0 || vertical != 0)
        {
            Vector3 direction = new Vector3(horizontal, vertical, 0);
            Vector3 potentialTarget = targetPosition + direction;

            // Check our Logic Grid before moving
            if (CanMove(potentialTarget))
            {
                targetPosition = potentialTarget;
                isMoving = true;
            }
        }
    }

    bool CanMove(Vector3 target)
    {
        // Convert world position to grid coordinates (assuming 1 unit = 1 tile)
        int gridX = Mathf.RoundToInt(target.x);
        int gridY = Mathf.RoundToInt(target.y);

        return gridManager.IsCellWalkable(gridX, gridY);
    }
}