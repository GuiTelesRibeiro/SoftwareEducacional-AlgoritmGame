using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private Button[] actionsButtons; // Bot�es de a��es dispon�veis
    [SerializeField] private SlotActionsButton[] slotButtons; // Refer�ncia para os bot�es de slots
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;

    private int position = 0; // �ndice para o pr�ximo slot
    public string[] listAction; // Lista para armazenar as a��es
    private Dictionary<string, Sprite> actionSpriteMap;

    private void Start()
    {
        // Inicializa o dicion�rio de a��es e sprites
        actionSpriteMap = new Dictionary<string, Sprite>
        {
            { "Up", upSprite },
            { "Down", downSprite },
            { "Left", leftSprite },
            { "Right", rightSprite }
        };

        // Inicializa a lista com o mesmo tamanho dos bot�es de slot
        listAction = new string[slotButtons.Length];
    }

    public void OnClickDirectionButton(string action)
    {
        if (position >= slotButtons.Length)
        {
            Debug.Log("Todos os slots est�o preenchidos!");
            return;
        }

        // Define a a��o no slot correspondente
        slotButtons[position].SetAction(action, GetActionSprite(action));
        listAction[position] = action;
        position++;
    }

    public void OnClickSlotButton(int index)
    {
        // Limpa o slot clicado e reorganiza a lista
        slotButtons[index].ClearActions();
        listAction[index] = null;

        for (int i = index; i < listAction.Length - 1; i++)
        {
            listAction[i] = listAction[i + 1];
            slotButtons[i].SetAction(listAction[i], slotButtons[i + 1].GetComponent<Image>().sprite);
        }

        // Limpa o �ltimo slot
        listAction[listAction.Length - 1] = null;
        slotButtons[slotButtons.Length - 1].ClearActions();
        position = Mathf.Max(0, position - 1);
    }

    private Sprite GetActionSprite(string action)
    {
        if (actionSpriteMap.TryGetValue(action, out Sprite sprite))
        {
            return sprite;
        }

        Debug.LogWarning($"A��o desconhecida: {action}");
        return null;
    }

    public void ToggleButtons(bool state)
    {
        foreach (var button in actionsButtons)
        {
            button.interactable = state; // Ativa ou desativa os bot�es de a��o
        }

        foreach (var slot in slotButtons)
        {
            slot.GetComponent<Button>().interactable = state; // Ativa ou desativa os bot�es de slot
        }
    }
}
