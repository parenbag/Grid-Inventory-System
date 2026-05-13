using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string itemID;
    public string itemName;

    [TextArea]
    public string description;

    [Header("Visual")]
    public Sprite icon;

    [Header("Grid Size")]
    public int width = 1;
    public int height = 1;

    [Header("Stack")]
    public bool stackable;
    public int maxStack = 1;
}