using System.Collections.Generic;

[System.Serializable]
public class InventorySaveData
{
    public List<InventoryItemSaveData> items = new();
}

[System.Serializable]
public class InventoryItemSaveData
{
    public string itemID;

    public int x;
    public int y;

    public int amount;
}