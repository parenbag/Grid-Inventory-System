using System.Collections.Generic;
using UnityEngine;

public class InventoryGridUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventoryGrid inventoryGrid;

    [SerializeField] private RectTransform cellsRoot;
    [SerializeField] private RectTransform itemsRoot;

    [Header("Prefabs")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private InventoryItemUI itemPrefab;

    [Header("Grid Settings")]
    [SerializeField] private Vector2 cellSize = new Vector2(64, 64);

    [SerializeField] private float spacing = 4f;

    private readonly List<InventoryItemUI> itemUIs = new();

    public InventoryGrid InventoryGrid => inventoryGrid;
    public Vector2 CellSize => cellSize;
    public float Spacing => spacing;
    public RectTransform ItemsRoot => itemsRoot;

    private InventoryCellUI[,] cells;

    private void Start()
    {
        BuildGrid();

        inventoryGrid.OnInventoryChanged += Refresh;

        Refresh();
    }

    public void BuildGrid()
    {
        foreach (Transform child in cellsRoot)
        {
            Destroy(child.gameObject);
        }

        for (int y = 0; y < inventoryGrid.GridHeight; y++)
        {
            for (int x = 0; x < inventoryGrid.GridWidth; x++)
            {
                GameObject cell =
                    Instantiate(cellPrefab, cellsRoot);

                if (cells == null)
                {
                    cells = new InventoryCellUI[
                        inventoryGrid.GridWidth,
                        inventoryGrid.GridHeight];
                }

                cells[x, y] =
                    cell.GetComponent<InventoryCellUI>();

                RectTransform rect =
                    cell.GetComponent<RectTransform>();

                rect.anchorMin = new Vector2(0, 1);
                rect.anchorMax = new Vector2(0, 1);
                rect.pivot = new Vector2(0, 1);

                rect.sizeDelta = cellSize;

                rect.anchoredPosition = new Vector2(
                    x * (cellSize.x + spacing),
                    -y * (cellSize.y + spacing)
                );
            }
        }

        float width =
            inventoryGrid.GridWidth * (cellSize.x + spacing);

        float height =
            inventoryGrid.GridHeight * (cellSize.y + spacing);

        cellsRoot.sizeDelta = new Vector2(width, height);
        itemsRoot.sizeDelta = new Vector2(width, height);
    }

    public void Refresh()
    {
        foreach (InventoryItemUI itemUI in itemUIs)
        {
            if (itemUI != null)
                Destroy(itemUI.gameObject);
        }

        itemUIs.Clear();
        UpdateOccupiedCells();

        foreach (InventoryItem item in inventoryGrid.items)
        {
            InventoryItemUI spawned =
                Instantiate(itemPrefab, itemsRoot);

            spawned.Setup(item, this);

            itemUIs.Add(spawned);
        }
        UpdateOccupiedCells();
    }

    public Vector2Int GetGridPosition(Vector2 mousePosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            itemsRoot,
            mousePosition,
            null,
            out Vector2 localPoint);

        localPoint.x += itemsRoot.rect.width * 0.5f;
        localPoint.y = Mathf.Abs(
            localPoint.y - (itemsRoot.rect.height * 0.5f));

        int x = Mathf.FloorToInt(
            localPoint.x / (cellSize.x + spacing));

        int y = Mathf.FloorToInt(
            localPoint.y / (cellSize.y + spacing));

        return new Vector2Int(x, y);
    }

    public void ResetCellColors()
    {
        for (int y = 0; y < inventoryGrid.GridHeight; y++)
        {
            for (int x = 0; x < inventoryGrid.GridWidth; x++)
            {
                cells[x, y].ResetColor();
            }
        }
    }
    public void UpdateOccupiedCells()
    {
        ResetCellColors();

        foreach (InventoryItem item in inventoryGrid.items)
        {
            ColorCells(
                item.x,
                item.y,
                item.Width,
                item.Height,
                new Color(0.15f, 0.15f, 0.15f, 1f));
        }
    }

    public void ColorCells(
        int startX,
        int startY,
        int width,
        int height,
        Color color)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int cellX = startX + x;
                int cellY = startY + y;

                if (cellX < 0 || cellY < 0 ||
                    cellX >= inventoryGrid.GridWidth ||
                    cellY >= inventoryGrid.GridHeight)
                {
                    continue;
                }

                cells[cellX, cellY].SetColor(color);
            }
        }
    }
}