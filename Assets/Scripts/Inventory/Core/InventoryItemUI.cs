using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [SerializeField] private Image icon;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private InventoryItem item;
    private InventoryGridUI gridUI;

    private Vector2 dragOffset;

    private Transform startParent;



    [Header("Preview Colors")]
    [SerializeField] private Color canPlaceColor = Color.green;

    [SerializeField] private Color replaceColor = Color.yellow;

    [SerializeField] private Color blockedColor = Color.red;

    [SerializeField] private Color stackColor = Color.blue;

    [SerializeField] private Color occupiedColor = Color.black;


    public void Setup(InventoryItem inventoryItem, InventoryGridUI inventoryGridUI)
    {
        item = inventoryItem;
        gridUI = inventoryGridUI;

        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        icon.sprite = item.data.icon;

        UpdateVisual();
    }

    private void UpdateVisual()
    {
        Vector2 cellSize = gridUI.CellSize;
        float spacing = gridUI.Spacing;

        float width = item.Width * cellSize.x + ((item.Width - 1) * spacing);
        float height = item.Height * cellSize.y + ((item.Height - 1) * spacing);

        rectTransform.sizeDelta = new Vector2(width, height);

        float x = item.x * (cellSize.x + spacing);
        float y = -(item.y * (cellSize.y + spacing));

        rectTransform.anchoredPosition = new Vector2(x, y);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = transform.parent;

        transform.SetParent(canvas.transform);

        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            null,
            out Vector2 localMousePos);

        dragOffset =
            rectTransform.anchoredPosition - localMousePos;

        gridUI.InventoryGrid.RemoveItem(item);
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            null,
            out Vector2 localMousePos);

        rectTransform.anchoredPosition =
            localMousePos + dragOffset;

        UpdatePlacementPreview();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        Vector2Int pos = gridUI.GetGridPosition(Input.mousePosition);

        bool placed = gridUI.InventoryGrid.CanPlaceItem(item, pos.x, pos.y);

        if (placed)
        {
            gridUI.InventoryGrid.PlaceItem(item, pos.x, pos.y);
        }
        else
        {
            gridUI.InventoryGrid.TryAddItem(item);
        }

        transform.SetParent(startParent);

        gridUI.ResetCellColors();

    }


    private void UpdatePlacementPreview()
    {
        gridUI.UpdateOccupiedCells();

        Vector2Int pos =
            gridUI.GetGridPosition(Input.mousePosition);

        bool hasBlockedCells = false;
        bool hasStackableCells = false;
        bool hasReplaceCells = false;

        for (int y = 0; y < item.Height; y++)
        {
            for (int x = 0; x < item.Width; x++)
            {
                int checkX = pos.x + x;
                int checkY = pos.y + y;

                if (checkX < 0 || checkY < 0 ||
                    checkX >= gridUI.InventoryGrid.GridWidth ||
                    checkY >= gridUI.InventoryGrid.GridHeight)
                {
                    hasBlockedCells = true;

                    continue;
                }

                InventoryItem found =
                    gridUI.InventoryGrid.GetItem(checkX, checkY);

                if (found == null)
                {
                    gridUI.ColorCells(
                        checkX,
                        checkY,
                        1,
                        1,
                        canPlaceColor);

                    continue;
                }

                if (found.data == item.data &&
                    found.data.stackable)
                {
                    hasStackableCells = true;

                    gridUI.ColorCells(
                        checkX,
                        checkY,
                        1,
                        1,
                        stackColor);

                    continue;
                }

                hasReplaceCells = true;

                gridUI.ColorCells(
                    checkX,
                    checkY,
                    1,
                    1,
                    occupiedColor);
            }
        }

        if (hasBlockedCells)
        {
            for (int y = 0; y < item.Height; y++)
            {
                for (int x = 0; x < item.Width; x++)
                {
                    int checkX = pos.x + x;
                    int checkY = pos.y + y;

                    if (checkX < 0 || checkY < 0 ||
                        checkX >= gridUI.InventoryGrid.GridWidth ||
                        checkY >= gridUI.InventoryGrid.GridHeight)
                    {
                        continue;
                    }

                    InventoryItem found =
                        gridUI.InventoryGrid.GetItem(checkX, checkY);

                    if (found == null)
                    {
                        gridUI.ColorCells(
                            checkX,
                            checkY,
                            1,
                            1,
                            blockedColor);
                    }
                }
            }
        }

        if (hasReplaceCells)
        {
            for (int y = 0; y < item.Height; y++)
            {
                for (int x = 0; x < item.Width; x++)
                {
                    int checkX = pos.x + x;
                    int checkY = pos.y + y;

                    if (checkX < 0 || checkY < 0 ||
                        checkX >= gridUI.InventoryGrid.GridWidth ||
                        checkY >= gridUI.InventoryGrid.GridHeight)
                    {
                        continue;
                    }

                    InventoryItem found =
                        gridUI.InventoryGrid.GetItem(checkX, checkY);

                    if (found == null)
                    {
                        gridUI.ColorCells(
                            checkX,
                            checkY,
                            1,
                            1,
                            replaceColor);
                    }
                }
            }
        }
    }

}