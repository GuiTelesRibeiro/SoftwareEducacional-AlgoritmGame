using UnityEngine;
using UnityEngine.UI;

// Gerencia o invent�rio e suas opera��es
public class Inventory : MonoBehaviour
{
    // Singleton para f�cil acesso ao invent�rio
    public static Inventory Singleton;

    [SerializeField] private SelectedItemPanel selectedItemPanel;

    // Array de slots do invent�rio
    [SerializeField] private InventorySlot[] inventorySlots;
    // Array de slots de equipamento
    [SerializeField] private InventorySlot[] equipmentSlots;

    // Bot�o para gerar itens
    [SerializeField] private Button giveItmBtn;
    // Prefab usado para criar novos itens
    [SerializeField] private InventoryItem itemPrefab;
    // Lista de itens dispon�veis para spawn
    [SerializeField] private Item[] items;

    // Item atualmente selecionado pelo jogador
    private InventoryItem selectedItem;

    // Inicializa o Singleton e configura o bot�o
    private void Awake()
    {
        Singleton = this; // Define esta inst�ncia como o Singleton
        selectedItemPanel.UpdatePanel(null); // Atualiza o painel
        giveItmBtn.onClick.AddListener(delegate { SpawnInventoryItem(); });
    }

    // Propriedade para obter o item selecionado
    public InventoryItem SelectedItem => selectedItem;

    // Seleciona ou troca um item
    public void SelectItem(InventoryItem item)
    {
        if (selectedItem != null)
        {
            if (selectedItem == item)
            {
                DeselectItem();
                return;
            }

            SwapItems(selectedItem, item);
            DeselectItem();
        }
        else
        {
            selectedItem = item;
            selectedItemPanel.UpdatePanel(selectedItem.myItem);
        }

        // Atualiza as cores ap�s a sele��o
        UpdateCollorSelectedItem();
    }

    // Deseleciona o item atual
    public void DeselectItem()
    {
        if (selectedItem != null)
        {
            selectedItem = null;
            selectedItemPanel.UpdatePanel(null);
        }

        // Atualiza as cores ap�s a desele��o
        UpdateCollorSelectedItem();
    }

    // Atualiza as cores dos slots com base no item selecionado
    public void UpdateCollorSelectedItem()
    {
        // Atualiza todos os slots do invent�rio para cinza
        foreach (var slot in inventorySlots)
        {
            slot.UpdateCollor(false); // Define todos os slots como cinza
        }

        // Atualiza todos os slots de equipamento para cinza
        foreach (var slot in equipmentSlots)
        {
            slot.UpdateCollor(false); // Define todos os slots como cinza
        }

        // Se houver um item selecionado, destaca o slot ativo
        if (selectedItem != null && selectedItem.activeSlot != null)
        {
            selectedItem.activeSlot.UpdateCollor(true); // Define como verde
        }
    }

    public bool TagVerify(InventoryItem ItemOrigem, InventorySlot slotDestino)
    {
        if (slotDestino.myTag == SlotTag.None || ItemOrigem.myItem.itemTag == slotDestino.myTag)
        {
            return true;
        }
        return false;
    }


    private void SwapItems(InventoryItem item1, InventoryItem item2)
    {
        if (TagVerify(item1, item2.activeSlot) && TagVerify(item2, item1.activeSlot))
        {
            InventorySlot slot1 = item1.activeSlot;
            InventorySlot slot2 = item2.activeSlot;

            slot1.SetItem(item2);
            slot2.SetItem(item1);
            DeselectItem();
            return;
        }
        Debug.Log("Troca de itens nao permitida. Tags incompativeis");
        DeselectItem();
    }

    public void SpawnInventoryItem(Item item = null)
    {
        Item _item = item ?? PickRandomItem();

        foreach (var slot in inventorySlots)
        {
            if (slot.myItem == null)
            {
                Instantiate(itemPrefab, slot.transform).Initialize(_item, slot);
                break;
            }
        }
    }

    private Item PickRandomItem()
    {
        return items[Random.Range(0, items.Length)];
    }
}
