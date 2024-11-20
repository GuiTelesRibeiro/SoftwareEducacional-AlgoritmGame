using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedItemPanel : MonoBehaviour
{
    [SerializeField] private Image itemIcon;            // �cone do item
    [SerializeField] private TMP_Text itemName;         // Nome do item
    [SerializeField] private TMP_Text itemDescription;  // Descri��o do item
    [SerializeField] private TMP_Text itemTag;          // Tag do item
    [SerializeField] private TMP_Text noItemMensage;          // Tag do item

    // M�todo para atualizar o painel com os dados do item
    public void UpdatePanel(Item item)
    {
        if (item != null)
        {
            itemIcon.sprite = item.sprite;
            itemIcon.color = new Color(itemIcon.color.r, itemIcon.color.g, itemIcon.color.b, 1f); // �cone 100% opaco
            itemName.text = item.nomeItem;                     // Atualiza o nome
            itemDescription.text = item.descricao;             // Atualiza a descri��o
            itemTag.text = $"Tag: {item.itemTag.ToString()}";   // Atualiza a tag
            noItemMensage.text = "";
            gameObject.SetActive(true);                        // Exibe o painel
        }
        else
        {
            //gameObject.SetActive(false); // Oculta o painel se n�o houver item
            itemIcon.color = new Color(itemIcon.color.r, itemIcon.color.g, itemIcon.color.b, 0f); // �cone 100% transparente
            itemName.text = "";                     // Atualiza o nome
            itemDescription.text = "";             // Atualiza a descri��o
            itemTag.text = "";   // Atualiza a tag
            noItemMensage.text = "Selecione um item para ver suas informa��es"; 
            gameObject.SetActive(true);                        // Exibe o painel
        }
    }
}
