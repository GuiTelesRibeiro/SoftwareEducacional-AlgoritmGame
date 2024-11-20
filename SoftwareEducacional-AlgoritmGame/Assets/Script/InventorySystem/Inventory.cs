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

    // Botão para gerar itens
    [SerializeField] private Button giveItmBtn;
    // Prefab usado para criar novos itens
    [SerializeField] private InventoryItem itemPrefab;
    // Lista de itens disponíveis para spawn
    [SerializeField] private Item[] items;

    // Item atualmente selecionado pelo jogador
    private InventoryItem selectedItem;

    // Inicializa o Singleton e configura o botão
    private void Awake()
    {
        Singleton = this; // Define esta instância como o Singleton
        // Adiciona evento ao botão para gerar itens
        selectedItemPanel.UpdatePanel(null); // Atualiza o painel
        giveItmBtn.onClick.AddListener(delegate { SpawnInventoryItem(); });
    }

    // Propriedade para obter o item selecionado
    public InventoryItem SelectedItem => selectedItem;

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
            selectedItem.activeSlot.UpdateCollor(true); // Atualiza a cor para indicar seleção
            selectedItemPanel.UpdatePanel(selectedItem.myItem);
        }
    }

    // Deseleciona o item atual
    public void DeselectItem()
    {
        if (selectedItem != null)
        {
            selectedItem.activeSlot.UpdateCollor(false); // Atualiza a cor para cinza
            selectedItem = null;
            selectedItemPanel.UpdatePanel(null);
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

    // Troca dois itens de posição
    private void SwapItems(InventoryItem item1, InventoryItem item2)
    {
        if (TagVerify(item1 , item2.activeSlot) && TagVerify(item2, item1.activeSlot))
        {
            //SelectedItem.activeSlot.image.color = Color.gray;
            // Obtém os slots dos dois itens
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

    // Cria um novo item no inventário
    public void SpawnInventoryItem(Item item = null)
    {
        // Se nenhum item for especificado, escolhe um aleatório
        Item _item = item ?? PickRandomItem();

        // Procura o primeiro slot vazio no inventário
        foreach (var slot in inventorySlots)
        {
            if (slot.myItem == null) // Se o slot estiver vazio
            {
                // Instancia o item no slot e inicializa
                Instantiate(itemPrefab, slot.transform).Initialize(_item, slot);
                break; // Para após criar o item
            }
        }
    }

    // Retorna um item aleatório da lista
    private Item PickRandomItem()
    {
        // Escolhe um índice aleatório e retorna o item correspondente
        return items[Random.Range(0, items.Length)];
    }
}
