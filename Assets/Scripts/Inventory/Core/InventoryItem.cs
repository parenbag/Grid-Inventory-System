using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public ItemData data;

    public int x;
    public int y;

    public int amount = 1;

    public int Width => data.width;
    public int Height => data.height;
}