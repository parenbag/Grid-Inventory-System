using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    [SerializeField] private InventoryGrid inventoryGrid;

    [SerializeField] private ItemData pistol;
    [SerializeField] private ItemData rifle;

    private void Start()
    {
        InventoryItem item1 = new();
        item1.data = pistol;

        InventoryItem item2 = new();
        item2.data = rifle;

        inventoryGrid.TryAddItem(item1);
        inventoryGrid.TryAddItem(item2);
    }
}