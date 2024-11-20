using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedItemPanel : MonoBehaviour
{
    [SerializeField] private Image itemIcon;            // �cone do item
    [SerializeField] private Sprite noItemIcon;            // �cone de quando n�o houver item
    [SerializeField] private TMP_Text itemName;         // Nome do item
    [SerializeField] private TMP_Text itemDescription;  // Descri��o do item
    [SerializeField] private TMP_Text itemTag;          // Tag do item

    // M�todo para atualizar o painel com os dados do item
    public void UpdatePanel(Item item)
    {
        if (item != null)
        {
            itemIcon.sprite = item.sprite;                     // Atualiza o �cone
            itemName.text = item.nomeItem;                     // Atualiza o nome
            itemDescription.text = item.descricao;             // Atualiza a descri��o
            itemTag.text = $"Tag: {item.itemTag.ToString()}";   // Atualiza a tag
            gameObject.SetActive(true);                        // Exibe o painel
        }
        else
        {
            //gameObject.SetActive(false); // Oculta o painel se n�o houver item
            itemIcon.sprite = noItemIcon;                     // Atualiza o �cone
            itemName.text = "Nenhum item por hora";                     // Atualiza o nome
            itemDescription.text = "Caso voc� tente selecionar um item, � bem provavel que aqui apare�a sua descri��o, voc� deve tentar";             // Atualiza a descri��o
            itemTag.text = "Sem item, sem tag!";   // Atualiza a tag
            gameObject.SetActive(true);                        // Exibe o painel
        }
    }
}
