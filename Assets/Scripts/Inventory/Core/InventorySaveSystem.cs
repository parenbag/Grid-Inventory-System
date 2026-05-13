using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InventorySaveSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventoryGrid inventoryGrid;

    [Header("Item Database")]
    [SerializeField] private List<ItemData> itemDatabase;

    private string SavePath => Path.Combine(Application.persistentDataPath, "inventory.json");

    public void Save()
    {
        InventorySaveData data = new();

        foreach (InventoryItem item in inventoryGrid.items)
        {
            InventoryItemSaveData saveItem = new();

            saveItem.itemID = item.data.itemID;
            saveItem.x = item.x;
            saveItem.y = item.y;
            saveItem.amount = item.amount;

            data.items.Add(saveItem);
        }

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(SavePath, json);

        Debug.Log("Inventory Saved: " + SavePath);
    }

    public void Load()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("Save file not found");
            return;
        }

        string json = File.ReadAllText(SavePath);

        InventorySaveData data = JsonUtility.FromJson<InventorySaveData>(json);

        if (data == null)
        {
            Debug.LogError("Failed to load inventory");
            return;
        }

        ClearInventory();

        foreach (InventoryItemSaveData savedItem in data.items)
        {
            ItemData foundItem = GetItemByID(savedItem.itemID);

            if (foundItem == null)
            {
                Debug.LogWarning("Item not found in database: " + savedItem.itemID);
                continue;
            }

            InventoryItem item = new();

            item.data = foundItem;
            item.amount = savedItem.amount;

            inventoryGrid.PlaceItem(item, savedItem.x, savedItem.y);
        }

        Debug.Log("Inventory Loaded");
    }

    private ItemData GetItemByID(string id)
    {
        foreach (ItemData item in itemDatabase)
        {
            if (item.itemID == id)
                return item;
        }

        return null;
    }

    private void ClearInventory()
    {
        List<InventoryItem> itemsCopy = new(inventoryGrid.items);

        foreach (InventoryItem item in itemsCopy)
        {
            inventoryGrid.RemoveItem(item);
        }
    }
}