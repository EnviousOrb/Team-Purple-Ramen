using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
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
    }

    public void UpdateMainSlot(ItemData item)
    {
        mainSlotImage.sprite = item.itemSprite;
        descriptionText.text = item.itemDescription;
    }

    public void UpdateInventoryUI(List<ItemData> itemList)
    {

        for (int i = 0; i < inventoryUISlotLocation.Count; i++)
        {
            if (i < itemList.Count)
            {
                ItemData currentItem = itemList[i];
                inventoryUISlotLocation[i].sprite = itemList[i].itemSprite;
                inventoryUISlotLocation[i].enabled = true;

                Button slotButton = inventoryUISlotLocation[i].GetComponent<Button>();
                if (slotButton != null)
                {
                    slotButton.onClick.RemoveAllListeners();

                    slotButton.onClick.AddListener(() => UpdateMainSlot(currentItem));
                }
            }
            else if (i < inventoryUISlotLocation.Count)
            {
                inventoryUISlotLocation[i].enabled = false;
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

