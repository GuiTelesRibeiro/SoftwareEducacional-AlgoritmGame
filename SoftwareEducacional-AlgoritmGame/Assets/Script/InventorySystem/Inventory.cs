using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// Gerencia o invent�rio e suas opera��es
public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;

    [SerializeField] private SelectedItemPanel selectedItemPanel;

    [SerializeField] private InventorySlot[] inventorySlots;
    [SerializeField] private InventorySlot[] equipmentSlots;
    [SerializeField] private InventoryItem itemPrefab;
    private InventoryItem selectedItem;

    // Lista de itens dispon�veis para spawn
    [SerializeField] public Item[] allItemsList;

    public InventoryItem SelectedItem => selectedItem;
    public int[] getItemIds;// Lista sempre atualizada com os itens que tem no inventario jogador 
    public int[] setItemIds;
    private int itemAReceber;


    private void Awake()
    {

        if (Singleton == null)
        {
            Singleton = this; // Ensure Singleton is properly initialized
        }
        selectedItemPanel.UpdatePanel(null);
        GetInventoryBanco(1);
        UpdateItensIventory();
    }

    private void Update()
    {
        InventoryDataUpdate(1);
        
    }
    public void InventoryDataUpdate(int ID)
    {
        UpdateListInventoryItens();// mantem uma lista atualizada dos itens do inventario
        if (AreArraysEqual(getItemIds, setItemIds))
        {
            return;
        }

        SetInventoryBanco(1);
    }
    private bool AreArraysEqual(int[] array1, int[] array2)
    {
        if (array1 == null || array2 == null)
            return false;

        if (array1.Length != array2.Length)
            return false;

        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i] != array2[i])
                return false;
        }

        return true;
    }

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
        UpdateCollorSelectedItem();
    }
    public void DeselectItem()
    {
        if (selectedItem != null)
        {
            selectedItem = null;
            selectedItemPanel.UpdatePanel(null);
        }
        UpdateCollorSelectedItem();
    }
    public void UpdateCollorSelectedItem()
    {
        foreach (var slot in inventorySlots)
        {
            slot.UpdateCollor(false);
        }
        foreach (var slot in equipmentSlots)
        {
            slot.UpdateCollor(false);
        }

        if (selectedItem != null && selectedItem.activeSlot != null)
        {
            selectedItem.activeSlot.UpdateCollor(true);
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

    //-------------------------------------------------- Itens Do invent�rio---------Modifique apenas coisas a boixo deste ponto-----------------------------------------------------
    public void SpawnInventoryItemById(int itemId)
    {
        Item itemToSpawn = FindItemById(itemId);

        if (itemToSpawn == null)
        {
            Debug.LogError($"Item com ID {itemId} n�o encontrado. Certifique-se de que o ID seja v�lido.");
            return;
        }

        foreach (var slot in inventorySlots)
        {
            if (slot.myItem == null)
            {
                Instantiate(itemPrefab, slot.transform).Initialize(itemToSpawn, slot);
                return;
            }
        }

        Debug.LogWarning("Invent�rio cheio. N�o foi poss�vel adicionar o item.");
    }

    private Item FindItemById(int itemId)
    {
        foreach (var item in allItemsList)
        {
            if (item.itemID == itemId)
            {
                return item;
            }
        }
        return null;
    }

    private int[] UpdateListInventoryItens()
    {
        // Inicializa o vetor com o tamanho do n�mero de slots de invent�rio
        getItemIds = new int[inventorySlots.Length];

        // Itera sobre cada slot do invent�rio
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            // Verifica se o slot cont�m um item
            if (inventorySlots[i].myItem != null)
            {
                // Armazena o ID do item no vetor
                getItemIds[i] = inventorySlots[i].myItem.myItem.itemID; 
            }
            else
            {
                // Se n�o houver item, coloca 0
                getItemIds[i] = 0;
            }
        }
            return getItemIds;
    }


    public void SetInventoryBanco(int ID)
    {

        ID = 1;// por enquanto tem apenas um player
        setItemIds = getItemIds;
        if (!PlayerExist(ID))
        {
            return;
        }
        BancoDeDados bancoDeDados = new BancoDeDados();
        bancoDeDados.SalvarInventario(ID, setItemIds);
        UpdateItensIventory();
    }
    public void GetInventoryBanco(int ID)
    {
        ID = 1;// por enquanto tem apenas um player
        if (!PlayerExist(ID))
        {
            return;
        }
        BancoDeDados bancoDeDados = new BancoDeDados();
        setItemIds = bancoDeDados.LerInventario(ID);
    }


    public void UpdateItensIventory()
    {
        // Itera pelos slots do invent�rio para limpar itens antigos
        foreach (var slot in inventorySlots)
        {
            if (slot.myItem != null)
            {
                Destroy(slot.myItem.gameObject);
                slot.myItem = null;
            }
        }

        // Itera sobre os IDs salvos e os atribui ao respectivo slot
        for (int i = 0; i < setItemIds.Length; i++)
        {
            if (setItemIds[i] > 0 && i < inventorySlots.Length) // Verifica se o ID do item e o �ndice do slot s�o v�lidos
            {
                SpawnItemDirectlyToSlot(setItemIds[i], inventorySlots[i]);
            }
        }
    }

    // Adiciona o item diretamente a um slot espec�fico
    private void SpawnItemDirectlyToSlot(int itemId, InventorySlot slot)
    {
        Item itemToSpawn = FindItemById(itemId);

        if (itemToSpawn == null)
        {
            Debug.LogError($"Item com ID {itemId} n�o encontrado.");
            return;
        }

        if (slot.myItem == null)
        {
            InventoryItem newItem = Instantiate(itemPrefab, slot.transform);
            newItem.Initialize(itemToSpawn, slot);
        }
    }

    public bool PlayerExist(int playerId)
    {
        BancoDeDados bancoDeDados = new BancoDeDados();
        bool isPlayerExist;
        var dados = bancoDeDados.LerPlayer(playerId);

        if (dados.Read())
        {
            Debug.Log("PlayerExiste");
            isPlayerExist = true;
            
        }
        else
        {
            Debug.Log("PlayerN�oExiste");
            isPlayerExist = false;
        }
        dados.Close();
        bancoDeDados.FecharConexao();
        return isPlayerExist;
    }

    public void DeleteItemById(int itemId)
    {
        // Itera pelos slots de invent�rio para encontrar o item com o ID correspondente
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            // Verifica se o slot cont�m um item e se o ID do item corresponde ao ID passado
            if (inventorySlots[i].myItem != null && inventorySlots[i].myItem.myItem.itemID == itemId)
            {
                // Remove o item do slot
                Destroy(inventorySlots[i].myItem.gameObject);
                inventorySlots[i].myItem = null; // Define o slot como vazio
                UpdateListInventoryItens(); // Atualiza a lista de itens do invent�rio
                return; // Termina a fun��o ap�s deletar o item
            }
        }

        // Se o item n�o for encontrado no invent�rio, exibe um erro
        Debug.LogError($"Item com ID {itemId} n�o encontrado no invent�rio.");
    }


}
