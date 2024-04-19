using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item", order = 1)]
public class ItemData : ScriptableObject, IInventory
{
    [SerializeField]
    private Sprite inventorySprite;
    public Sprite InventorySprite
    {
        get { return inventorySprite; }
        set { inventorySprite = value; }
    }

    [SerializeField]
    private string inventoryText;
    public string InventoryText
    {
        get { return inventoryText; }
        set { inventoryText = value; }
    }
}