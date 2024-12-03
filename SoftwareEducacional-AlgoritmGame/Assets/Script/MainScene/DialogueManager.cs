using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePainel;
    public TextMeshProUGUI dialogueText;
    private string[] dialogue; // Armazena o di�logo do NPC atual
    private int index;

    public GameObject contButton;
    public float wordSpeed;
    //private bool playerIsClose;
    private bool isDialogueActive; // Indica se um di�logo est� ativo

    private NPC currentNPC; // NPC com o qual o jogador est� interagindo

    public void OpenDialogue()
    {

        if (currentNPC != null && !isDialogueActive)
        {
            dialogue = currentNPC.dialogue; // Pega o di�logo do NPC atual
            dialoguePainel.SetActive(true);
            isDialogueActive = true; // Marca o di�logo como ativo
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
        
        contButton.SetActive(false); // Desativa o bot�o de continuar ao fechar o di�logo
        dialogue = null; // Reseta o di�logo para evitar refer�ncias pendentes
        isDialogueActive = false; // Marca o di�logo como inativo
    }

    IEnumerator Typing()
    {
        dialogueText.text = ""; // Garante que o texto seja limpo antes de come�ar a exibir
        contButton.SetActive(false); // Desativa o bot�o enquanto o texto est� sendo exibido

        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }

        contButton.SetActive(true); // Ativa o bot�o de continuar ap�s exibir todo o texto
    }

    public void NextLine()
    {
        contButton.SetActive(false); // Desativa o bot�o antes de exibir a pr�xima linha

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
            currentNPC = collision.GetComponent<NPC>(); // Pega o script NPC do objeto com o qual o jogador est� em contato
            //playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            //playerIsClose = false;
            currentNPC = null; // Libera a refer�ncia ao NPC atual
            ZeroText(); // Fecha o painel quando o jogador sai do alcance
        }
    }
}
