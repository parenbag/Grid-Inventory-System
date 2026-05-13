using UnityEngine;
using UnityEngine.UI;

public class InventoryCellUI : MonoBehaviour
{
    [SerializeField] private Image image;

    private Color defaultColor;

    private void Awake()
    {
        defaultColor = image.color;
    }

    public void SetColor(Color color)
    {
        image.color = color;
    }

    public void ResetColor()
    {
        image.color = defaultColor;
    }
}