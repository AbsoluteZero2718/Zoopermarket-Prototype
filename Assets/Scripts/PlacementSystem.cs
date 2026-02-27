using Unity.VisualScripting;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public GameObject objectToPlace;
    public InputManager inputManager;
    public Camera mainCam;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;

            Vector3Int gridPosition = inputManager.WorldToGridPos(mouseWorldPosition);

            inputManager.PlaceObject(objectToPlace, gridPosition);
        }
    }
}
