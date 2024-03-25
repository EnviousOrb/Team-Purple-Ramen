using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] public List<Image> inventoryUISlotLocation = new List<Image>(); // UI images for inventory slots.

    public static UIManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void UpdateInventoryUI(List<ItemData> itemList)
    {
        for (int i = 0; i < inventoryUISlotLocation.Count; i++)
        {
            if (i < itemList.Count)
            {
                inventoryUISlotLocation[i].sprite = itemList[i].itemSprite;
                inventoryUISlotLocation[i].enabled = true;
            }
            else
            {
                inventoryUISlotLocation[i].enabled = false;
            }
        }
    }
}

