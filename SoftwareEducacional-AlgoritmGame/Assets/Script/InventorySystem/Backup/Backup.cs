/*
 using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem;

    [SerializeField] InventorySlot[] inventorySlots;
    [SerializeField] InventorySlot[] equipmentSlots;

    [SerializeField] Transform draggablesTransform;
    [SerializeField] InventoryItem itemPrefab;
    [SerializeField] Item[] items;

    [SerializeField] Button giveItmBtn;

    private void Awake()
    {
        Singleton = this;
        giveItmBtn.onClick.AddListener(delegate { SpawnIventoryItem(); });


    }

    private void Update()
    {
        if (carriedItem == null)
        {
            return;
        }
        carriedItem.transform.positionF1 = Input.mousePosition;
    }

    public void SetCarriedItem(InventoryItem item)
    {
        if (carriedItem != null)
        {
            if (item.activeSlot.myTag != SlotTag.None && item.activeSlot.myTag != carriedItem.myItem.itemTag)
            {
                return;
            }
            item.activeSlot.SetItem(carriedItem);
        }
        if (item.activeSlot.myTag != SlotTag.None)
        {
            EquipEquipment(item.activeSlot.myTag, null);
        }
        carriedItem = item;
        carriedItem.canvasGroup.blocksRaycasts = false;
        item.transform.SetParent(draggablesTransform);
    }

    public void EquipEquipment(SlotTag tag, InventoryItem item = null)
    {
        switch (tag)
        {
            case SlotTag.Head:
                if (item == null)
                {
                    Debug.Log("Removeu um item da tag Head");
                }
                else
                {
                    Debug.Log("Equipou um item da tag Head");
                }
                break;
            case SlotTag.Chest:
                break;
            case SlotTag.Legs:
                break;
            case SlotTag.Feet:
                break;
        }
    }


    public void SpawnIventoryItem(Item item = null)
    {
        Debug.Log("Botao Pressionado_1");
        Item _item = item;
        if (_item == null)
        {
            _item = PickRandomItem();

        }
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].myItem == null)
            {
                Instantiate(itemPrefab, inventorySlots[i].transform).Initialize(_item, inventorySlots[i]);
                break;
            }
        }
    }

    Item PickRandomItem()
    {
        int random = Random.Range(0, items.Length);
        return items[random];
    }



}
--------------------------------------------------------------
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{

    public InventoryItem myItem { get; set; }
    public SlotTag myTag;



    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Inventory.carriedItem == null)
            {
                return;
            }
            if (myTag != SlotTag.None && Inventory.carriedItem.myItem.itemTag != myTag)
            {
                return;
            }
            SetItem(Inventory.carriedItem);
        }
    }
    public void SetItem(InventoryItem item)
    {
        Inventory.carriedItem = null;
        item.activeSlot.myItem = null;
        myItem = item;
        myItem.activeSlot = this;
        myItem.transform.SetParent(transform);
        myItem.canvasGroup.blocksRaycasts = true;
        if (myTag != SlotTag.None)
        {
            Inventory.Singleton.EquipEquipment(myTag, myItem);
        }
    }
}
-------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    Image itemIcom;
    public CanvasGroup canvasGroup { get; private set; }
    public Item myItem { get; set; }
    public InventorySlot activeSlot { get; set; }// onde o item está



    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        itemIcom = GetComponent<Image>();

    }

    public void Initialize(Item item, InventorySlot parent)
    {
        activeSlot = parent;
        activeSlot.myItem = this;
        myItem = item;
        itemIcom.sprite = item.sprite;

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Inventory.Singleton.SetCarriedItem(this);
        }
    }
}
 
 */