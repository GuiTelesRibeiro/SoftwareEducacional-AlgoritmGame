using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePainel;
    public TextMeshProUGUI dialogueText;
    private string[] dialogue; // Armazena o diálogo do NPC atual
    private int index;

    public GameObject contButton;
    public float wordSpeed;
    //private bool playerIsClose;
    private bool isDialogueActive; // Indica se um diálogo está ativo

    private NPC currentNPC; // NPC com o qual o jogador está interagindo

    public void OpenDialogue()
    {

        if (currentNPC != null && !isDialogueActive)
        {
            dialogue = currentNPC.dialogue; // Pega o diálogo do NPC atual
            dialoguePainel.SetActive(true);
            isDialogueActive = true; // Marca o diálogo como ativo
            StartCoroutine(Typing());
        }

        
    }

    public void ZeroText()
    {
        dialogueText.text = "";
        index = 0;
        if (dialoguePainel)
        {
            dialoguePainel.SetActive(false);
        }
        else
        {
            Debug.LogError("dialoguePainel n existe");
        }
        
        contButton.SetActive(false); // Desativa o botão de continuar ao fechar o diálogo
        dialogue = null; // Reseta o diálogo para evitar referências pendentes
        isDialogueActive = false; // Marca o diálogo como inativo
    }

    IEnumerator Typing()
    {
        dialogueText.text = ""; // Garante que o texto seja limpo antes de começar a exibir
        contButton.SetActive(false); // Desativa o botão enquanto o texto está sendo exibido

        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }

        contButton.SetActive(true); // Ativa o botão de continuar após exibir todo o texto
    }

    public void NextLine()
    {
        contButton.SetActive(false); // Desativa o botão antes de exibir a próxima linha

        if (index < dialogue.Length - 1)
        {
            index++;
            StartCoroutine(Typing());
        }
        else
        {
            ZeroText();
            CanvasController.Singleton.DefaultPainels();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            currentNPC = collision.GetComponent<NPC>(); // Pega o script NPC do objeto com o qual o jogador está em contato
            //playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            //playerIsClose = false;
            currentNPC = null; // Libera a referência ao NPC atual
            ZeroText(); // Fecha o painel quando o jogador sai do alcance
        }
    }
}
