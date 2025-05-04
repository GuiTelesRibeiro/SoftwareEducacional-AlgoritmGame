using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private Button[] actionsButtons; // Botões de ações disponíveis
    [SerializeField] private SlotActionsButton[] slotButtonsF1; // Referência para os botões de slots
    [SerializeField] private SlotActionsButton[] slotButtonsF2; // Referência para os botões de slots
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;
    [SerializeField] private Sprite F1Sprite;
    [SerializeField] private Sprite F2Sprite;
    [SerializeField] private Sprite Clockwise;
    [SerializeField] private Sprite Counterclockwise;
    [SerializeField] private Sprite forward;

    private int positionF1 = 0; // Índice para o próximo slot F1
    private int positionF2 = 0; // Índice para o próximo slot
    public string[] listActionF1; // Lista para armazenar as ações da funcao 1
    public string[] listActionF2; // Lista para armazenar as ações da funcao 2
    public bool ThisLevelUsingF2;
    public int numberFunctionIsSelected = 1;

    [SerializeField] private int maxIterations = 1000; // Limite máximo de iterações totais
    private Dictionary<string, Sprite> actionSpriteMap;

    private void Start()
    {
        // Inicializa o dicionário de ações e sprites
        actionSpriteMap = new Dictionary<string, Sprite>
        {
            { "Up", upSprite },
            { "Down", downSprite },
            { "Left", leftSprite },
            { "Right", rightSprite },
            { "F1", F1Sprite },
            { "F2", F2Sprite },
            { "Clockwise", Clockwise },
            { "Counterclockwise", Counterclockwise },
            { "forward", forward }
        };

        // Inicializa a lista com o mesmo tamanho dos botões de slot
        listActionF1 = new string[slotButtonsF1.Length];
        listActionF2 = new string[slotButtonsF2.Length];
    }

    public string[] listAllActions()
    {
        List<string> finalActions = new List<string>();
        int iterationCount = 0;
        int f1Index = 0;
        int f2Index = 0;
        bool processingF1 = true; // Começa processando F1

        // Pilha para controlar qual função estamos processando
        Stack<bool> functionStack = new Stack<bool>();
        functionStack.Push(true); // Começa com F1

        while (functionStack.Count > 0 && iterationCount < maxIterations)
        {
            iterationCount++;
            bool currentIsF1 = functionStack.Peek();
            string[] currentFunction = currentIsF1 ? listActionF1 : listActionF2;
            int currentIndex = currentIsF1 ? f1Index : f2Index;

            // Se chegou ao final da função atual
            if (currentIndex >= currentFunction.Length || string.IsNullOrEmpty(currentFunction[currentIndex]))
            {
                functionStack.Pop(); // Remove da pilha
                if (currentIsF1) f1Index = 0; else f2Index = 0; // Reseta o índice
                continue;
            }

            string action = currentFunction[currentIndex];

            // Atualiza o índice para a próxima ação
            if (currentIsF1) f1Index++; else f2Index++;

            if (action == "F1")
            {
                functionStack.Push(true); // Empilha F1
                f1Index = 0; // Reseta o índice de F1
            }
            else if (action == "F2" && ThisLevelUsingF2)
            {
                functionStack.Push(false); // Empilha F2
                f2Index = 0; // Reseta o índice de F2
            }
            else
            {
                finalActions.Add(action);
                // Limita a lista final a 30 comandos
                if (finalActions.Count >= 30)
                {
                    break;
                }
            }
        }

        if (iterationCount >= maxIterations)
        {
            Debug.LogWarning("Limite de iterações excedido - possível loop infinito!");
            return finalActions.ToArray(); // Retorna o que foi possível gerar
        }

        return finalActions.ToArray();
    }

    public void OnClickDirectionButton(string action)
    {
        if (numberFunctionIsSelected == 1)
        {
            if (positionF1 >= slotButtonsF1.Length)
            {
                Debug.Log("Todos os slots estão preenchidos!");
                return;
            }

            // Define a ação no slot correspondente
            slotButtonsF1[positionF1].SetAction(action, GetActionSprite(action));
            listActionF1[positionF1] = action;
            positionF1++;
        }
        else if(numberFunctionIsSelected == 2) {
            if (positionF2 >= slotButtonsF2.Length)
            {
                Debug.Log("Todos os slots estão preenchidos!");
                return;
            }

            // Define a ação no slot correspondente
            slotButtonsF2[positionF2].SetAction(action, GetActionSprite(action));
            listActionF2[positionF2] = action;
            positionF2++;
        }
    }

    public void OnClickSlotButtonF1(int index)
    {
            // Limpa o slot clicado e reorganiza a lista
            slotButtonsF1[index].ClearActions();
            listActionF1[index] = null;

            for (int i = index; i < listActionF1.Length - 1; i++)
            {
                listActionF1[i] = listActionF1[i + 1];
                slotButtonsF1[i].SetAction(listActionF1[i], slotButtonsF1[i + 1].GetComponent<Image>().sprite);
            }

            // Limpa o último slot
            listActionF1[listActionF1.Length - 1] = null;
            slotButtonsF1[slotButtonsF1.Length - 1].ClearActions();
            positionF1 = Mathf.Max(0, positionF1 - 1);
    }
    public void OnClickSlotButtonF2(int index)
    {
        // Limpa o slot clicado e reorganiza a lista
        slotButtonsF2[index].ClearActions();
        listActionF2[index] = null;

        for (int i = index; i < listActionF2.Length - 1; i++)
        {
            listActionF2[i] = listActionF2[i + 1];
            slotButtonsF2[i].SetAction(listActionF2[i], slotButtonsF2[i + 1].GetComponent<Image>().sprite);
        }

        // Limpa o último slot
        listActionF2[listActionF2.Length - 1] = null;
        slotButtonsF2[slotButtonsF2.Length - 1].ClearActions();
        positionF2 = Mathf.Max(0, positionF2 - 1);
    }

    public void OnClickSwitchButton()
    {
        numberFunctionIsSelected = (numberFunctionIsSelected == 1) ? 2 : 1;
    }
    private Sprite GetActionSprite(string action)
    {
        if (actionSpriteMap.TryGetValue(action, out Sprite sprite))
        {
            return sprite;
        }

        Debug.LogWarning($"Ação desconhecida: {action}");
        return null;
    }

    public void ToggleButtons(bool state)
    {
        foreach (var button in actionsButtons)
        {
            button.interactable = state; // Ativa ou desativa os botões de ação
        }

        foreach (var slot in slotButtonsF1)
        {
            slot.GetComponent<Button>().interactable = state; // Ativa ou desativa os botões de slot
        }
    }
}
