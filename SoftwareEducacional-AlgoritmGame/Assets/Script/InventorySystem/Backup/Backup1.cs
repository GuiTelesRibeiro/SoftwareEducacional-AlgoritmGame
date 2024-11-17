/*
 using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;// faz a classe ser acessada por qualquer outra, facilita o lance de lao ter que ficar arrastando um onstancia dela na unity:)


    public static InventorySlot selectedSlot = null;



    [SerializeField] InventorySlot[] inventorySlots;
    [SerializeField] InventorySlot[] equipmentSlots;

    [SerializeField] Transform draggablesTransform;// nem uso mais sá poha

    //spawInventory------------------------------------------ ;-;
    [SerializeField] InventoryItem itemPrefab;
    [SerializeField] Item[] items;

    [SerializeField] Button giveItmBtn;

    private void Awake()
    {
        Singleton = this;
        giveItmBtn.onClick.AddListener(delegate { SpawnInventoryItem(); });
    }

    public void ResetInventoryState()
    {

        selectedSlot = null;
        foreach (var slot in inventorySlots)
        {
            if (slot.myItem != null)
            {
                slot.myItem.activeSlot = slot;
            }
        }
        Debug.Log("----------------------------------Estado do inventário resetado----------------------------");
    }

    public void SelectItem(InventoryItem item)
    {
        if (item == null || item.activeSlot == null) return;

        if (selectedSlot == null)
        {
            if (item.activeSlot.myItem != null)
            {
                selectedSlot = item.activeSlot;
                //Debug.Log("Item selecionado no slot " + selectedSlot);
            }
        }
        else
        {
            TrySwapItems(selectedSlot, item.activeSlot);
            ResetInventoryState();  // Reseta o inventário após a troca
        }
    }

    public bool VerifyTag(SlotTag tag, InventoryItem item = null)
    {
        // Verifica se o item é compatível com o slot ou se é uma remoção
        if (tag != SlotTag.None && item != null && item.myItem.itemTag != tag)
        {
            //Debug.Log("Tags incompatíveis. Ação não permitida.");
            return false;
        }

        // Log de feedback
        Debug.Log(item == null
            ? $"Removeu um item da tag {tag}"
            : $"Equipou um item da tag {tag}");
        return true;
    }

    public void TrySwapItems(InventorySlot fromSlot, InventorySlot toSlot)
    {
        if (fromSlot == null || toSlot == null) return;

        // Verifica compatibilidade usando VerifyTag antes de trocar
        if (!VerifyTag(toSlot.myTag, fromSlot.myItem))
        {
            Debug.Log("Itens não são compatíveis. Troca não realizada.");
            return;
        }

        // Verifica se o slot destino está vazio
        if (toSlot.myItem == null)
        {
            Debug.Log("Move o item para o slot vazio");//
            toSlot.SetItem(fromSlot.myItem);
            fromSlot.SetItem(null); // Esvazia o slot de origem
        }
        else
        {
            // Troca os itens entre os slots com verificação de tags
            InventoryItem tempItem = toSlot.myItem;
            toSlot.SetItem(fromSlot.myItem);

            if (!VerifyTag(fromSlot.myTag, tempItem))
            {
                // Se a troca não for válida, desfaz a ação
                Debug.Log("Tags incompatíveis. Ação revertida.");
                toSlot.SetItem(tempItem);
            }
            else
            {
                fromSlot.SetItem(tempItem);
            }
        }

        ResetInventoryState();  // Reseta o inventário após a troca
    }

    public void SpawnInventoryItem(Item item = null)
    {
        //Debug.Log("Botão pressionado");
        Item _item = item ?? PickRandomItem();
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].myItem == null)
            {
                InventoryItem newItem = Instantiate(itemPrefab, inventorySlots[i].transform);
                newItem.Initialize(_item, inventorySlots[i]);
                VerifyTag(inventorySlots[i].myTag, newItem); // Verifica compatibilidade do slot
                break;
            }
        }
        ResetInventoryState();  // Reseta o inventário após spawn de item
    }

    Item PickRandomItem()
    {
        int random = Random.Range(0, items.Length);
        return items[random];
    }
}
------------------------------------------------------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem myItem { get; set; }
    public SlotTag myTag;


    public void OnPointerClick(PointerEventData eventData)
    {
       

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Click Slot");
            if (myItem != null || Inventory.selectedSlot != null)
            {
                //Debug.Log("item no slot ou slot selecionado");
                Inventory.Singleton.SelectItem(myItem);
            }
            else
            {
                // Se um slot está selecionado, mova o item para o slot vazio
                Debug.Log("omaga");
                Debug.Log($"{this}");
                Inventory.Singleton.TrySwapItems(Inventory.selectedSlot, this);
            }
        }
    }

    public void PlaceItemInEmptySlot()
    {
        if (Inventory.selectedSlot == null) return;

        // Coloca o item selecionado neste slot vazio
        SetItem(Inventory.selectedSlot.myItem);

        // Apaga o item do slot antigo
        Inventory.selectedSlot.SetItem(null);

        // Reseta o estado do inventário para limpar a seleção
        Inventory.Singleton.ResetInventoryState();
    }

    public void SetItem(InventoryItem item)
    {
        // Verifica a compatibilidade antes de definir o item no slot
        if (myTag != SlotTag.None && item != null && !Inventory.Singleton.VerifyTag(myTag, item))
        {
            Debug.Log("Item incompatível com o slot. Operação cancelada.");
            return;
        }

        // Remove referência ao slot anterior se houver um item no slot atual
        if (myItem != null)
        {
            myItem.activeSlot = null;
        }

        // Define o novo item no slot
        myItem = item;
        if (myItem != null)
        {
            myItem.activeSlot = this;
            myItem.transform.SetParent(transform);
            myItem.canvasGroup.blocksRaycasts = true;
        }

        // Atualiza status do item equipado (logs via VerifyTag para compatibilidade)
        Inventory.Singleton.VerifyTag(myTag, myItem);
    }
}
------------------------------------------------------------------------------------------------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    Image itemIcon;
    public CanvasGroup canvasGroup;
    public Item myItem { get; set; }
    public InventorySlot activeSlot { get; set; }

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        itemIcon = GetComponent<Image>();
    }

    public void Initialize(Item item, InventorySlot parent)
    {
        activeSlot = parent;
        activeSlot.myItem = this;
        myItem = item;
        itemIcon.sprite = item.sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Click Item");
            Inventory.Singleton.SelectItem(this);
        }
    }
}
------------------------------------------------------------------------------------------------------------------------------------------------------------------
 using UnityEngine;

public enum SlotTag { None, Head, Chest, Legs, Feet }
[CreateAssetMenu(menuName = "RPG 2D/Item")]
public class Item : ScriptableObject
{
    public Sprite sprite;
    public SlotTag itemTag;
}
 
 */
