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
    private bool isInteractionActive; // Indica se uma interaçao está ativa

    private NPC currentNPC; // NPC com o qual o jogador está interagindo
    private Mission currentMission;
    public bool missionPanel= false;




    public void OpenMission()
    {
        if(currentMission != null && !isInteractionActive)
        {
            currentMission.Interaction();
        }
    }
    public void OpenDialogue()
    {

        if (currentNPC != null && !isInteractionActive)
        {
            dialogue = currentNPC.dialogue; // Pega o diálogo do NPC atual
            dialoguePainel.SetActive(true);
            isInteractionActive = true; // Marca o diálogo como ativo
            StartCoroutine(Typing());
        }
    }
    public void OpenMissionPanel()
    {

        if (missionPanel) 
        {
            CanvasController.Singleton.OpenMissionPanel();
        }
    }



    public void ZeroText()
    {
        dialogueText.text = "";
        index = 0;
        if (dialoguePainel)
            dialoguePainel.SetActive(false);
        if(contButton)
            contButton.SetActive(false);
        dialogue = null;
        isInteractionActive = false;
    }

    IEnumerator Typing()
    {
        dialogueText.text = "";
        contButton.SetActive(false);

        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }

        contButton.SetActive(true);
    }

    public void NextLine()
    {
        contButton.SetActive(false);

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
            //Debug.Log("NPC");
            currentNPC = collision.GetComponent<NPC>();
        }
        if (collision.CompareTag("Mission"))
        {
            //Debug.Log("Mission");
            currentMission = collision.GetComponent<Mission>();
        }
        if (collision.CompareTag("MP"))
        {
            missionPanel = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            //Debug.Log("NPC");
            currentNPC = null;
            ZeroText();
        }
        if (collision.CompareTag("Mission"))
        {
            //Debug.Log("Mission");
            currentMission =null;
        }
        if (collision.CompareTag("MP"))
        {
            missionPanel=false;
        }

    }
}
