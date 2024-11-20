using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedItemPanel : MonoBehaviour
{
    [SerializeField] private Image itemIcon;            // Ícone do item
    [SerializeField] private TMP_Text itemName;         // Nome do item
    [SerializeField] private TMP_Text itemDescription;  // Descrição do item
    [SerializeField] private TMP_Text itemTag;          // Tag do item
    [SerializeField] private TMP_Text noItemMensage;          // Tag do item

    // Método para atualizar o painel com os dados do item
    public void UpdatePanel(Item item)
    {
        if (item != null)
        {
            itemIcon.sprite = item.sprite;
            itemIcon.color = new Color(itemIcon.color.r, itemIcon.color.g, itemIcon.color.b, 1f); // Ícone 100% opaco
            itemName.text = item.nomeItem;                     // Atualiza o nome
            itemDescription.text = item.descricao;             // Atualiza a descrição
            itemTag.text = $"Tag: {item.itemTag.ToString()}";   // Atualiza a tag
            noItemMensage.text = "";
            gameObject.SetActive(true);                        // Exibe o painel
        }
        else
        {
            //gameObject.SetActive(false); // Oculta o painel se não houver item
            itemIcon.color = new Color(itemIcon.color.r, itemIcon.color.g, itemIcon.color.b, 0f); // Ícone 100% transparente
            itemName.text = "";                     // Atualiza o nome
            itemDescription.text = "";             // Atualiza a descrição
            itemTag.text = "";   // Atualiza a tag
            noItemMensage.text = "Selecione um item para ver suas informações"; 
            gameObject.SetActive(true);                        // Exibe o painel
        }
    }
}
