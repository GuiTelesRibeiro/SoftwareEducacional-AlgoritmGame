using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// Gerencia o inventário e suas operações
public class Inventory : MonoBehaviour
{
    // Singleton para fácil acesso ao inventário
    public static Inventory Singleton;

    [SerializeField] private SelectedItemPanel selectedItemPanel;

    // Array de slots do inventário
    [SerializeField] private InventorySlot[] inventorySlots;
    // Array de slots de equipamento
    [SerializeField] private InventorySlot[] equipmentSlots;

    // Prefab usado para criar novos itens
    [SerializeField] private InventoryItem itemPrefab;

    // Lista de itens disponíveis para spawn
    [SerializeField] private Item[] allItemsList;

    public int[] itemIds;

    // Item atualmente selecionado pelo jogador
    private InventoryItem selectedItem;

    // Inicializa o Singleton e configura o botão
    private void Awake()
    {
        Singleton = this; // Define esta instância como o Singleton
        selectedItemPanel.UpdatePanel(null); // Atualiza o painel
    }

    private void Update()
    {
        GetInventoryItemIds();
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

        // Atualiza as cores após a seleção
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

        // Atualiza as cores após a deseleção
        UpdateCollorSelectedItem();
    }

    // Atualiza as cores dos slots com base no item selecionado
    public void UpdateCollorSelectedItem()
    {
        // Atualiza todos os slots do inventário para cinza
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
    private void GetInventoryItemIds()
    {
        // Inicializa o vetor com o tamanho do número de slots de inventário
        itemIds = new int[inventorySlots.Length];

        // Itera sobre cada slot do inventário
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            // Verifica se o slot contém um item
            if (inventorySlots[i].myItem != null)
            {
                // Armazena o ID do item no vetor
                itemIds[i] = inventorySlots[i].myItem.myItem.itemID; 
            }
            else
            {
                // Se não houver item, coloca 0
                itemIds[i] = 0;
            }
        }
    }
    public void SpawnInventoryItemById(int itemId)
    {
        // Busca o item com base no ID fornecido
        Item itemToSpawn = FindItemById(itemId);

        // Verifica se o item foi encontrado
        if (itemToSpawn == null)
        {
            Debug.LogError($"Item com ID {itemId} não encontrado. Certifique-se de que o ID seja válido.");
            return;
        }

        // Itera pelos slots para encontrar o primeiro slot vazio
        foreach (var slot in inventorySlots)
        {
            if (slot.myItem == null)
            {
                Instantiate(itemPrefab, slot.transform).Initialize(itemToSpawn, slot);
                return;
            }
        }

        Debug.LogWarning("Inventário cheio. Não foi possível adicionar o item.");
    }

    // Função para encontrar um item pelo ID na lista
    private Item FindItemById(int itemId)
    {
        foreach (var item in allItemsList)
        {
            if (item.itemID == itemId)
            {
                return item;
            }
        }
        return null; // Retorna nulo se o item não for encontrado
    }

}
