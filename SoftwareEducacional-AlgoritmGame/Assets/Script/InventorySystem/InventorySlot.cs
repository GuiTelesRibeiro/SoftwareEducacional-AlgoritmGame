using UnityEngine;
using UnityEngine.EventSystems;

// Classe que representa um slot de inventário
public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    // Item atualmente armazenado neste slot
    public InventoryItem myItem { get; set; }
    // Tag para categorizar o slot (exemplo: arma, armadura, etc.)
    public SlotTag myTag;

    // Método chamado ao clicar no slot
    public void OnPointerClick(PointerEventData eventData)
    {
        // Verifica se o botão clicado é o esquerdo
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Se o slot está vazio e há um item selecionado, coloca o item no slot
            if (myItem == null && Inventory.Singleton.SelectedItem != null)
            {
                if (!Inventory.Singleton.TagVerify(Inventory.Singleton.SelectedItem, this))
                {
                    Debug.Log("Tag Incompativel");
                    Inventory.Singleton.DeselectItem();
                    return;
                }
                SetItem(Inventory.Singleton.SelectedItem); // Define o item no slot
                Inventory.Singleton.DeselectItem(); // Deseleciona o item
                return; // Sai do método após a ação
            }

            // Se o slot já contém um item, seleciona ou troca
            if (myItem != null)
            {
                Inventory.Singleton.SelectItem(myItem); // Seleciona o item no slot
            }
        }
    }

    // Define um item neste slot
    public void SetItem(InventoryItem item)
    {
        // Se o slot já contém um item, limpa a referência do slot no item
        if (myItem != null)
        {
            myItem.activeSlot = null; // Remove a ligação com este slot
        }

        // Se o item a ser adicionado já está em outro slot, limpa esse slot
        if (item.activeSlot != null)
        {
            item.activeSlot.myItem = null; // Remove o item do slot anterior
        }

        // Atribui o novo item a este slot
        myItem = item;
        // Atualiza o slot ativo no item
        myItem.activeSlot = this;

        // Ajusta a posição do item para ser filho deste slot
        myItem.transform.SetParent(transform, false); // Define o slot como pai
        myItem.transform.localPosition = Vector3.zero; // Centraliza o item no slot
    }
}
