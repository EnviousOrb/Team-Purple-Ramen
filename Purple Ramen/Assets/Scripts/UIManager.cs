using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UIManager : MonoBehaviour
{
    public Dictionary<int, IInventory> reservedSpots4Staffs = new();
    [SerializeField] public List<Image> inventoryUISlotLocation = new(); // UI images for inventory slots.
    [SerializeField] private Image mainSlotImage; // UI image for the main slot.
    [SerializeField] private SuperTextMesh descriptionText;
    int StaffSlotIndex = 0;
    public Slider bgmSlider, sfxSlider, npcSlider;

    public static UIManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void UpdateMainSlot(IInventory item)
    {
        mainSlotImage.sprite = item.InventorySprite;
        descriptionText.text = item.InventoryText;
    }

    public void UpdateInventoryUI(List<IInventory> itemList)
    {
        //clears any previous items from the inventory UI (this will probably be used when transitioning from level to hub)
        foreach (var slot in inventoryUISlotLocation)
        {
            slot.sprite = null;
            slot.enabled = false;
        }

        //Creates a copy of the list to track items that don't have a spot assigned to them
        List<IInventory> remainingItems = new(itemList);

        int maxIndex = Math.Min(5, itemList.Count);

        for (int i = 0; i < maxIndex; i++)
        {
            if (itemList[i] is staffElementalStats)
            {
                IInventory currentItem = itemList[i];
                reservedSpots4Staffs[i] = currentItem;
                remainingItems.Remove(currentItem);

                inventoryUISlotLocation[i].sprite = currentItem.InventorySprite;
                inventoryUISlotLocation[i].enabled = true;

                if (inventoryUISlotLocation[i].TryGetComponent<Button>(out var slotButton))
                {
                    slotButton.onClick.RemoveAllListeners();
                    slotButton.onClick.AddListener(() => UpdateMainSlot(currentItem));
                }
            }
        }

        //assign rest of items to generic slots
        foreach (IInventory item in remainingItems)
        {
            //Find first available slot
            int slotIndex = inventoryUISlotLocation.FindIndex(slot => slot.sprite == null && !reservedSpots4Staffs.ContainsKey(inventoryUISlotLocation.IndexOf(slot)));

            //check if spot was found and is a valid index
            if (slotIndex != -1 && slotIndex < inventoryUISlotLocation.Count)
            {
                //if it is, then go through motions
                inventoryUISlotLocation[slotIndex].sprite = item.InventorySprite;
                inventoryUISlotLocation[slotIndex].enabled = true;

                //update the onClick event like before
                if (inventoryUISlotLocation[slotIndex].TryGetComponent<Button>(out var slotButton))
                {
                    slotButton.onClick.RemoveAllListeners();
                    slotButton.onClick.AddListener(() => UpdateMainSlot(item));
                }

                //This whole section is purely for debugging, it allows the user to delete an item
                //with the right click button
                EventTrigger trigger = inventoryUISlotLocation[slotIndex].gameObject.AddComponent<EventTrigger>();
                EventTrigger.Entry entry = new()
                {
                    eventID = EventTriggerType.PointerClick
                };
                entry.callback.AddListener((eventData) => {
                    var pointerData = (PointerEventData)eventData;
                    if (pointerData.button == PointerEventData.InputButton.Right)
                    {
                        inventoryUISlotLocation[slotIndex].sprite = null;
                        inventoryUISlotLocation[slotIndex].enabled = false;
                        slotButton.onClick.RemoveAllListeners();
                        slotButton.onClick.AddListener(() => UpdateMainSlot(null));

                    }
                });
                trigger.triggers.Add(entry);
            }
        }
    }

    public void ReserveSpot(int slotIndex, IInventory item)
    {
        if (reservedSpots4Staffs.ContainsKey(slotIndex))
        {
            Debug.LogError("Slot is already reserved!");
            return;
        }
        reservedSpots4Staffs[slotIndex] = item;
    }

    public void toggleBGM()
    {
        AudioManager.instance.toggleBGM();
    }

    public void toggleSFX()
    {
        AudioManager.instance.toggleSFX();
    }

    public void toggleNPC()
    {
        AudioManager.instance.toggleNPC();
    }

    public void BGMVolume()
    {
        AudioManager.instance.bgmVolume(bgmSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.instance.bgmVolume(sfxSlider.value);
    }

    public void NPCVolume()
    {
        AudioManager.instance.NPCVolume(npcSlider.value);
    }
}

