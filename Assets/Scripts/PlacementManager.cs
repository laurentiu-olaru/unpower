using UnityEngine;
using System.Collections.Generic;

public class PlacementManager : MonoBehaviour
{
	public List<GameObject> buildingPrefabs; // Assign your prefabs in the Inspector
	private int currentBuildingIndex = 0;

	private GameObject ghostBuilding; // The "Blue Ghost"
	private bool isBuildingMode = false;

	public LayerMask buildingLayer; // Set this to "Buildings" in the Inspector
	private bool canPlace = true;

	void CheckPlacementValidity()
	{
		// Check an area slightly larger than the building (e.g., 2.8x2.8 for a 1x1 building)
		// This creates the "1 unit offset" feel
		Collider2D hit = Physics2D.OverlapBox(ghostBuilding.transform.position, new Vector2(2.8f, 2.8f), 0, buildingLayer);

		SpriteRenderer sr = ghostBuilding.GetComponent<SpriteRenderer>();

		if (hit == null)
		{
			canPlace = true;
			sr.color = new Color(0, 0.5f, 1f, 0.5f); // Blue (Valid)
		}
		else
		{
			canPlace = false;
			sr.color = new Color(1f, 0f, 0f, 0.5f); // Red (Invalid)
		}
	}

	void Update()
	{
		// 1. Toggle Building Mode
		if (Input.GetKeyDown(KeyCode.B))
		{
			if (!isBuildingMode) EnterBuildingMode();
			else CycleBuilding();
		}

		// 2. Cancel Building Mode
		if (Input.GetKeyDown(KeyCode.Escape) && isBuildingMode)
		{
			ExitBuildingMode();
		}

		if (isBuildingMode && ghostBuilding != null)
		{
			MoveGhostToMouse();
			CheckPlacementValidity(); // New function

			if (Input.GetMouseButtonDown(0) && canPlace)
			{
				PlaceBuilding();
			}
		}
	}

	void EnterBuildingMode()
	{
		isBuildingMode = true;
		CreateGhost();
	}

	void CycleBuilding()
	{
		currentBuildingIndex = (currentBuildingIndex + 1) % buildingPrefabs.Count;
		Destroy(ghostBuilding); // Remove old ghost
		CreateGhost(); // Create new ghost for the new building type
	}

	void CreateGhost()
	{
		// Create the ghost based on the current prefab
		ghostBuilding = Instantiate(buildingPrefabs[currentBuildingIndex]);

		// Disable components so the ghost doesn't "act" like a building yet
		if (ghostBuilding.GetComponent<Collider2D>())
			ghostBuilding.GetComponent<Collider2D>().enabled = false;

		// Make it Blue and Transparent
		SpriteRenderer sr = ghostBuilding.GetComponent<SpriteRenderer>();
		if (sr != null)
		{
			sr.color = new Color(0, 0.5f, 1f, 0.5f); // Blue with 50% transparency
		}
	}

	void MoveGhostToMouse()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;

		// OPTIONAL: Grid Snapping (Change 1f to 0.5f if your tiles are smaller)
		float snappedX = Mathf.Round(mousePos.x);
		float snappedY = Mathf.Round(mousePos.y);

		ghostBuilding.transform.position = new Vector3(snappedX, snappedY, 0);
	}


	void PlaceBuilding()
	{
		//Create the real building
		GameObject realBuilding = Instantiate(buildingPrefabs[currentBuildingIndex], ghostBuilding.transform.position, Quaternion.identity);

		// Check if it's a Barracks
		Barracks barracksScript = realBuilding.GetComponent<Barracks>();
		if (barracksScript != null)
		{
			barracksScript.InitializeBarracks();
		}

		//Tell the tower it is now allowed to shoot
		TowerBehavior towerScript = realBuilding.GetComponent<TowerBehavior>();
		if (towerScript != null)
		{
			towerScript.isPlaced = true;
		}
	}

	void ExitBuildingMode()
	{
		isBuildingMode = false;
		if (ghostBuilding != null) Destroy(ghostBuilding);
	}
}