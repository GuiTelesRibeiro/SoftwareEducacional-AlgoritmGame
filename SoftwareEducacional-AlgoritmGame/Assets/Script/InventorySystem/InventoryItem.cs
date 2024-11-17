using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Classe que representa um item de invent�rio
public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    // Refer�ncia para o �cone do item
    Image itemIcon;

    // Refer�ncia para o destaque visual de sele��o
    Image selectionHighlight;

    // Dados do item (exemplo: nome, sprite, etc.)
    public Item myItem { get; set; }

    // Slot atual onde o item est� localizado
    public InventorySlot activeSlot { get; set; }

    // M�todo chamado ao iniciar o componente
    void Awake()
    {
        // Obt�m o componente de imagem para exibir o �cone do item
        itemIcon = GetComponent<Image>();

        // Tenta localizar o elemento visual de destaque dentro do prefab
        selectionHighlight = transform.Find("SelectionHighlight")?.GetComponent<Image>();

        // Se o destaque foi encontrado, o desativa por padr�o
        if (selectionHighlight != null)
        {
            selectionHighlight.gameObject.SetActive(false);
        }
    }

    // Inicializa o item no slot espec�fico
    public void Initialize(Item item, InventorySlot parent)
    {
        // Configura o slot atual do item
        activeSlot = parent;
        activeSlot.myItem = this;

        // Atribui os dados do item
        myItem = item;

        // Define o sprite do item no �cone, se dispon�vel
        if (item.sprite != null)
        {
            itemIcon.sprite = item.sprite;
        }
        else
        {
            // Exibe um aviso no console se o sprite do item est� ausente
            Debug.LogWarning("Sprite do item est� faltando.");
        }
    }

    // M�todo chamado ao clicar no item
    public void OnPointerClick(PointerEventData eventData)
    {
        // Verifica se o bot�o esquerdo foi clicado
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Notifica o invent�rio para selecionar este item
            Inventory.Singleton.SelectItem(this);
        }
    }

    // Altera o estado visual do item quando ele � selecionado ou desmarcado
    public void SetSelected(bool isSelected)
    {
        // Ativa ou desativa o destaque visual com base no estado
        if (selectionHighlight != null)
        {
            selectionHighlight.gameObject.SetActive(isSelected);
        }
    }
}
