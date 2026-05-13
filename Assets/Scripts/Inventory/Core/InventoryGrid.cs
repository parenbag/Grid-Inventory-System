using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGrid : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 8;

    public int GridWidth => gridWidth;
    public int GridHeight => gridHeight;

    public Action OnInventoryChanged;

    private InventoryItem[,] grid;

    public List<InventoryItem> items = new();

    private void Awake()
    {
        grid = new InventoryItem[gridWidth, gridHeight];
    }

    public bool TryAddItem(InventoryItem item)
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (CanPlaceItem(item, x, y))
                {
                    PlaceItem(item, x, y);
                    return true;
                }
            }
        }

        return false;
    }

    public bool CanPlaceItem(InventoryItem item, int startX, int startY)
    {
        if (startX < 0 || startY < 0)
            return false;

        if (startX + item.Width > gridWidth)
            return false;

        if (startY + item.Height > gridHeight)
            return false;

        for (int y = 0; y < item.Height; y++)
        {
            for (int x = 0; x < item.Width; x++)
            {
                if (grid[startX + x, startY + y] != null)
                    return false;
            }
        }

        return true;
    }

    public void PlaceItem(InventoryItem item, int startX, int startY)
    {
        item.x = startX;
        item.y = startY;

        for (int y = 0; y < item.Height; y++)
        {
            for (int x = 0; x < item.Width; x++)
            {
                grid[startX + x, startY + y] = item;
            }
        }

        if (!items.Contains(item))
            items.Add(item);

        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(InventoryItem item)
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (grid[x, y] == item)
                    grid[x, y] = null;
            }
        }

        items.Remove(item);


    }

    public InventoryItem GetItem(int x, int y)
    {
        if (x < 0 || y < 0 ||
            x >= gridWidth || y >= gridHeight)
        {
            return null;
        }

        return grid[x, y];
    }
}