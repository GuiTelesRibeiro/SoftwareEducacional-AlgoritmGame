using UnityEngine;
using UnityEngine.UI;

// Gerencia o invent�rio e suas opera��es
public class Inventory : MonoBehaviour
{
    // Singleton para f�cil acesso ao invent�rio
    public static Inventory Singleton;

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
        // Adiciona evento ao bot�o para gerar itens
        giveItmBtn.onClick.AddListener(delegate { SpawnInventoryItem(); });
    }

    // Propriedade para obter o item selecionado
    public InventoryItem SelectedItem => selectedItem;

    // Seleciona ou troca um item
    public void SelectItem(InventoryItem item)
    {
        // Se j� houver um item selecionado
        if (selectedItem != null)
        {
            // Se o mesmo item foi clicado, deseleciona
            if (selectedItem == item)
            {
                DeselectItem();
                return;
            }

            // Caso contr�rio, troca os itens entre slots
            SwapItems(selectedItem, item);
            // Deseleciona ap�s a troca
            DeselectItem();
        }
        else
        {
            // Se nenhum item est� selecionado, seleciona o clicado
            selectedItem = item;
            selectedItem.SetSelected(true); // Destaca o item
        }
    }

    // Deseleciona o item atual
    public void DeselectItem()
    {
        // Se houver um item selecionado
        if (selectedItem != null)
        {
            selectedItem.SetSelected(false); // Remove o destaque
            selectedItem = null; // Limpa a refer�ncia ao item selecionado
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

    // Troca dois itens de posi��o
    private void SwapItems(InventoryItem item1, InventoryItem item2)
    {
        if (TagVerify(item1 , item2.activeSlot) && TagVerify(item2, item1.activeSlot))
        {
            // Obt�m os slots dos dois itens
            InventorySlot slot1 = item1.activeSlot;
            InventorySlot slot2 = item2.activeSlot;

            // Define o item2 no slot do item1
            slot1.SetItem(item2);
            // Define o item1 no slot do item2
            slot2.SetItem(item1);
            DeselectItem();
            return;
        }
        Debug.Log("Troca de itens nao permitida. Tags incompativeis");
        DeselectItem();
    }

    // Cria um novo item no invent�rio
    public void SpawnInventoryItem(Item item = null)
    {
        // Se nenhum item for especificado, escolhe um aleat�rio
        Item _item = item ?? PickRandomItem();

        // Procura o primeiro slot vazio no invent�rio
        foreach (var slot in inventorySlots)
        {
            if (slot.myItem == null) // Se o slot estiver vazio
            {
                // Instancia o item no slot e inicializa
                Instantiate(itemPrefab, slot.transform).Initialize(_item, slot);
                break; // Para ap�s criar o item
            }
        }
    }

    // Retorna um item aleat�rio da lista
    private Item PickRandomItem()
    {
        // Escolhe um �ndice aleat�rio e retorna o item correspondente
        return items[Random.Range(0, items.Length)];
    }
}
