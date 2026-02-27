using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float gridSize = 1f;
    private Dictionary<Vector3Int, GameObject> occupiedCells = new Dictionary<Vector3Int, GameObject>();

    public Vector3Int WorldToGridPos(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / gridSize);
        int y = Mathf.FloorToInt(worldPosition.y / gridSize);
        int z = Mathf.FloorToInt(worldPosition.z / gridSize);

        return new Vector3Int(x, y, z);
    }

    public bool isCellOccupied(Vector3Int gridPosition)
    {
        return occupiedCells.ContainsKey(gridPosition);
    }

    public void PlaceObject(GameObject objectPrefab, Vector3Int gridPosition)
    {
        if (!isCellOccupied(gridPosition))
        {
            Vector3 worldPosition = new Vector3(gridPosition.x, gridPosition.y, gridPosition.z);
            GameObject newObject = Instantiate(objectPrefab, worldPosition, Quaternion.identity);

            occupiedCells.Add(gridPosition, newObject);
            Debug.Log("Placed object at: {gridPosition}");
        }
        else
        {
            Debug.Log("Cell is already occupied.");
        }
    }
}
