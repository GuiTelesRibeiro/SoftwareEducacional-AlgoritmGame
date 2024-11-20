using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedItemPanel : MonoBehaviour
{
    [SerializeField] private Image itemIcon;            // Ícone do item
    [SerializeField] private Sprite noItemIcon;            // Ícone de quando não houver item
    [SerializeField] private TMP_Text itemName;         // Nome do item
    [SerializeField] private TMP_Text itemDescription;  // Descrição do item
    [SerializeField] private TMP_Text itemTag;          // Tag do item

    // Método para atualizar o painel com os dados do item
    public void UpdatePanel(Item item)
    {
        if (item != null)
        {
            itemIcon.sprite = item.sprite;                     // Atualiza o ícone
            itemName.text = item.nomeItem;                     // Atualiza o nome
            itemDescription.text = item.descricao;             // Atualiza a descrição
            itemTag.text = $"Tag: {item.itemTag.ToString()}";   // Atualiza a tag
            gameObject.SetActive(true);                        // Exibe o painel
        }
        else
        {
            //gameObject.SetActive(false); // Oculta o painel se não houver item
            itemIcon.sprite = noItemIcon;                     // Atualiza o ícone
            itemName.text = "Nenhum item por hora";                     // Atualiza o nome
            itemDescription.text = "Caso você tente selecionar um item, é bem provavel que aqui apareça sua descrição, você deve tentar";             // Atualiza a descrição
            itemTag.text = "Sem item, sem tag!";   // Atualiza a tag
            gameObject.SetActive(true);                        // Exibe o painel
        }
    }
}
