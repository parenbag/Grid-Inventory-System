using UnityEngine;

public class PlayerInventoryInteractor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private InventoryGrid inventoryGrid;

    [Header("Pickup")]
    [SerializeField] private float pickupDistance = 3f;
    [SerializeField] private LayerMask itemMask;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryPickup();
        }
    }

    private void TryPickup()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance, itemMask))
        {
            WorldItem worldItem = hit.collider.GetComponent<WorldItem>();

            if (worldItem == null)
                return;

            InventoryItem item = new();

            item.data = worldItem.itemData;
            item.amount = worldItem.amount;

            bool added = inventoryGrid.TryAddItem(item);

            if (added)
            {
                Destroy(worldItem.gameObject);
            }
        }
    }
}