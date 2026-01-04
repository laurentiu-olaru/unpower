using UnityEngine.Tilemaps;
using UnityEngine;

public class GridManager : MonoBehaviour
{
	public Tilemap obstaclesTilemap;
	public Vector3Int originOffset = Vector3Int.zero;
	public int width = 100;
	public int height = 100;

	private int[,] logicGrid;

	void Start()
	{
		if (obstaclesTilemap == null)
		{
			Debug.LogError("Obstacles Tilemap is not assigned!");
			return;
		}

		GenerateLogicGrid();
	}

	void GenerateLogicGrid()
	{
		logicGrid = new int[width, height];
		int wallCount = 0;

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Vector3Int tilePos = new Vector3Int(x + originOffset.x, y + originOffset.y, 0);

				if (obstaclesTilemap.HasTile(tilePos))
				{
					logicGrid[x, y] = 1;
					wallCount++;
				}
				else
				{
					logicGrid[x, y] = 0;
				}
			}
		}

		Debug.Log($"LogicGrid built. Walls found: {wallCount}");
	}

	public bool IsCellWalkable(int worldX, int worldY)
	{
		int arrayX = worldX - originOffset.x;
		int arrayY = worldY - originOffset.y;

		if (arrayX < 0 || arrayX >= width || arrayY < 0 || arrayY >= height) return false;

		return logicGrid[arrayX, arrayY] == 0;
	}

	public bool IsWorldPositionWalkable(Vector3 worldPosition)
	{
		Vector3Int cell = obstaclesTilemap.WorldToCell(worldPosition);
		return IsCellWalkable(cell.x, cell.y);
	}
}
