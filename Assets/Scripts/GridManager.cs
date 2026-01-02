using UnityEngine;
using UnityEngine.Tilemaps; // Add this!

public class GridManager : MonoBehaviour
{
    public Tilemap obstaclesTilemap; // Drag your Obstacles Tilemap here
    public int width = 20;
    public int height = 20;
    public Vector3Int originOffset = new Vector3Int(-10, -10, 0); // Centers the grid

    private int[,] logicGrid;

    void Awake()
    {
        GenerateLogicGrid();
    }

    void GenerateLogicGrid()
    {
        logicGrid = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Translate grid coordinates to Tilemap coordinates
                Vector3Int tilePos = new Vector3Int(x + originOffset.x, y + originOffset.y, 0);

                if (obstaclesTilemap.HasTile(tilePos))
                {
                    logicGrid[x, y] = 1; // It's a wall
                }
                else
                {
                    logicGrid[x, y] = 0; // It's walkable
                }
            }
        }
    }

    public bool IsCellWalkable(int worldX, int worldY)
    {
        // Convert world position back to our array index
        int arrayX = worldX - originOffset.x;
        int arrayY = worldY - originOffset.y;

        if (arrayX < 0 || arrayX >= width || arrayY < 0 || arrayY >= height) return false;

        return logicGrid[arrayX, arrayY] == 0;
    }
}