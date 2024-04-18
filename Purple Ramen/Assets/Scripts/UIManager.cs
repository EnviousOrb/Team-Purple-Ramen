using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Dictionary<int, ItemData> reservedSpots4Items = new();
    [SerializeField] public List<ItemData> Weapons;
    [SerializeField] public List<Image> inventoryUISlotLocation = new(); // UI images for inventory slots.
    [SerializeField] private Image mainSlotImage; // UI image for the main slot.
    [SerializeField] private SuperTextMesh descriptionText;
    public Slider bgmSlider /*sfxSlider*/;

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
        for (var i = 0; i < Weapons.Count; i++)
        {
            /*reservedSpots4Items.Add(1, Weapons[i]);
            reservedSpots4Items.Add(2, Weapons[i]);
            reservedSpots4Items.Add(3, Weapons[i]);
            reservedSpots4Items.Add(4, Weapons[i]);
            reservedSpots4Items.Add(5, Weapons[i]);*/
        }
    }

    public void UpdateMainSlot(ItemData item)
    {
        mainSlotImage.sprite = item.itemSprite;
        descriptionText.text = item.itemDescription;
    }

    public void UpdateInventoryUI(List<ItemData> itemList)
    {
        //clears any previous items from the inventory UI (this will probably be used when transitioning from level to hub)
        foreach (var slot in inventoryUISlotLocation)
        {
            slot.sprite = null;
            slot.enabled = false;
        }

        //Creates a copy of the list to track items that don't have a spot assigned to them
        List<ItemData> remainingItems = new List<ItemData>(itemList);
        //Assigns items to their reserved spot, if they have one
        foreach (var reservedSpot in reservedSpots4Items)
        {
            //Get item to go in spot
            ItemData reservedItem = reservedSpot.Value;
            //check if item is on reserved list
            if (remainingItems.Contains(reservedItem))
            {
                //go through motions of assigning item
                inventoryUISlotLocation[reservedSpot.Key].sprite = reservedItem.itemSprite;
                inventoryUISlotLocation[reservedSpot.Key].enabled = true;

                //updates the onClick event
                if (inventoryUISlotLocation[reservedSpot.Key].TryGetComponent<Button>(out var slotButton))
                {
                    slotButton.onClick.RemoveAllListeners();
                    slotButton.onClick.AddListener(() => UpdateMainSlot(reservedItem));
                }
                //remove item from list
                remainingItems.Remove(reservedItem);
            }
        }

        //assign rest of items to generic slots
        foreach (ItemData item in remainingItems)
        {
            //Find first available slot
            int slotIndex = inventoryUISlotLocation.FindIndex(slot => slot.sprite == null);

            //check if spot was found and is a valid index
            if (slotIndex != -1 && slotIndex < inventoryUISlotLocation.Count)
            {
                //if it is, then go through motions
                inventoryUISlotLocation[slotIndex].sprite = item.itemSprite;
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

    public void toggleBGM()
    {
        AudioManager.instance.toggleBGM();
    }

    /*public void toggleSFX()
    {
        AudioManager.instance.toggleSFX();
    }*/

    public void BGMVolume()
    {
        AudioManager.instance.bgmVolume(bgmSlider.value);
    }

    /*public void SFXVolume()
    {
        AudioManager.instance.bgmVolume(sfxSlider.value);
    }*/
}

