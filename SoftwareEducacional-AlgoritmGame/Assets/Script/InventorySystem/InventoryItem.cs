using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Classe que representa um item de inventário
public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    // Referência para o ícone do item
    Image itemIcon;

    // Referência para o destaque visual de seleção
    Image selectionHighlight;

    // Dados do item (exemplo: nome, sprite, etc.)
    public Item myItem { get; set; }

    // Slot atual onde o item está localizado
    public InventorySlot activeSlot { get; set; }

    // Método chamado ao iniciar o componente
    void Awake()
    {
        // Obtém o componente de imagem para exibir o ícone do item
        itemIcon = GetComponent<Image>();

        // Tenta localizar o elemento visual de destaque dentro do prefab
        selectionHighlight = transform.Find("SelectionHighlight")?.GetComponent<Image>();

        // Se o destaque foi encontrado, o desativa por padrão
        if (selectionHighlight != null)
        {
            selectionHighlight.gameObject.SetActive(false);
        }
    }

    // Inicializa o item no slot específico
    public void Initialize(Item item, InventorySlot parent)
    {
        // Configura o slot atual do item
        activeSlot = parent;
        activeSlot.myItem = this;

        // Atribui os dados do item
        myItem = item;

        // Define o sprite do item no ícone, se disponível
        if (item.sprite != null)
        {
            itemIcon.sprite = item.sprite;
        }
        else
        {
            // Exibe um aviso no console se o sprite do item está ausente
            Debug.LogWarning("Sprite do item está faltando.");
        }
    }

    // Método chamado ao clicar no item
    public void OnPointerClick(PointerEventData eventData)
    {
        // Verifica se o botão esquerdo foi clicado
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Notifica o inventário para selecionar este item
            Inventory.Singleton.SelectItem(this);
        }
    }

    // Altera o estado visual do item quando ele é selecionado ou desmarcado
    public void SetSelected(bool isSelected)
    {
        // Ativa ou desativa o destaque visual com base no estado
        if (selectionHighlight != null)
        {
            selectionHighlight.gameObject.SetActive(isSelected);
        }
    }
}
