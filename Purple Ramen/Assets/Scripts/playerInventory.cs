using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class playerInventory : MonoBehaviour
{
    [Header("General")]
    public List<itemType> inventoryList;
    public int playerReach;
    [SerializeField] Camera cam;
    [SerializeField] GameObject throwItem;
    [SerializeField] GameObject pickUpItem;

    [Space(20)]
    [Header("Keys")]
    [SerializeField] KeyCode throwItemKey;
    [SerializeField] KeyCode pickItemKey;

    public int selectedItem = 0;

    [Space(20)]
    [Header("Item GameObjects")]
    [SerializeField] GameObject item1;
    [SerializeField] GameObject item2;
    [SerializeField] GameObject item3;
    [SerializeField] GameObject item4;
    [SerializeField] GameObject item5;
    [SerializeField] GameObject item6;
    [SerializeField] GameObject item7;

    [Space(20)]
    [Header("Item Prefabs")]
    [SerializeField] GameObject item1Prefab;
    [SerializeField] GameObject item2Prefab;
    [SerializeField] GameObject item3Prefab;
    [SerializeField] GameObject item4Prefab;
    [SerializeField] GameObject item5Prefab;
    [SerializeField] GameObject item6Prefab;
    [SerializeField] GameObject item7Prefab;

    [Space(20)]
    [Header("UI")]
    [SerializeField] Image[] inventorySlotImage = new Image[7];
    [SerializeField] Image[] inventoryBackgroundImage = new Image[7];
    [SerializeField] Sprite emptySlotSprite;



    private Dictionary<itemType, GameObject> itemSetActive = new Dictionary<itemType, GameObject>() { };
    private Dictionary<itemType, GameObject>  itemInstantiate = new Dictionary<itemType, GameObject>() { };


    void Start()
    {
        itemSetActive.Add(itemType.Apple, item1);
        itemSetActive.Add(itemType.Banana, item2);
        itemSetActive.Add(itemType.Carrot, item3);
        itemSetActive.Add(itemType.Fish, item4);
        itemSetActive.Add(itemType.Mushroom, item5);
        itemSetActive.Add(itemType.Tomato, item6);
        itemSetActive.Add(itemType.Watermelon, item7);

        itemSetActive.Add(itemType.Apple, item1Prefab);
        itemSetActive.Add(itemType.Banana, item2Prefab);
        itemSetActive.Add(itemType.Carrot, item3Prefab);
        itemSetActive.Add(itemType.Fish, item4Prefab);
        itemSetActive.Add(itemType.Mushroom, item5Prefab);
        itemSetActive.Add(itemType.Tomato, item6Prefab);
        itemSetActive.Add(itemType.Watermelon, item7Prefab);

        NewItemSelected();
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;

        if (Physics.Raycast(ray, out hitinfo, playerReach))
        {
            Ipickable item = hitinfo.collider.GetComponent<Ipickable>();
            if (item != null)
            {
                pickUpItem.SetActive(true);
                if (Input.GetKey(pickItemKey))
                {
                    inventoryList.Add(hitinfo.collider.GetComponent<ItemPickable>().itemScriptableObject.itemType);
                    item.PickItem();
                }
            }
            else
            {
                pickUpItem.SetActive(false);

            }
        }
        else
        {
            pickUpItem.SetActive(true);
        }
        //Item throw
        if (Input.GetKeyDown(throwItemKey)&& inventoryList.Count > 1)
        {
            Instantiate(itemInstantiate[inventoryList[selectedItem]], position: throwItem.transform.position, new Quaternion());
            inventoryList.RemoveAt(selectedItem);

            if(selectedItem != 0)
            {
                selectedItem -= 1;
            }
            NewItemSelected();
        }
        //UI
        for(int i = 0; i < 6; i++)
        {
            if (i < inventoryList.Count)
            {
                inventorySlotImage[i].sprite = itemSetActive[inventoryList[i]].GetComponent<Item>().itemData.itemSprite;
            }
            else
            {
                inventorySlotImage[i].sprite = emptySlotSprite;
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1) && inventoryList.Count > 0)
        {
            selectedItem = 0;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && inventoryList.Count >12)
        {
            selectedItem = 1;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && inventoryList.Count > 2)
        {
            selectedItem = 2;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && inventoryList.Count > 3)
        {
            selectedItem = 3;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && inventoryList.Count > 4)
        {
            selectedItem = 4;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && inventoryList.Count > 5)
        {
            selectedItem = 5;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && inventoryList.Count > 6)
        {
            selectedItem = 6;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && inventoryList.Count > 7)
        {
            selectedItem = 7;
            NewItemSelected();
        }
    }

    private void NewItemSelected()
    {
        item1.SetActive(false);
        item2.SetActive(false);
        item3.SetActive(false);
        item4.SetActive(false);
        item5.SetActive(false);
        item6.SetActive(false);
        item7.SetActive(false);

        GameObject selectedItemGameObject = itemSetActive[inventoryList[selectedItem]];
        selectedItemGameObject.SetActive(true);
    }
}

public interface Ipickable
{
    void PickItem();
}