using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PuzzleCanvasController : MonoBehaviour
{

    [SerializeField] GameObject victoryPanel;
    [SerializeField] GameObject losePanel;
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] GameObject puzzlePainel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject UiPanel;
    public int Tentativas;
    [SerializeField] int IdPlayer;
    [SerializeField] int IdMissao;
    [SerializeField] bool playTutorial;
    public int Move_To_Complete;
    [SerializeField] Image imageItem;
    [SerializeField] Item[] allListItem;
    [SerializeField] ItemSpawnCache itemSpawnCache;

    // Start is called before the first frame update
    private void Awake()
    {
        Tentativas = 0;
        BancoDeDados bancoDeDados = new BancoDeDados();
        if (!bancoDeDados.VerificarPlayerMissaoExiste(IdPlayer, IdMissao))
        {
            Debug.Log("PlayerMissao Nao existe ainda");
            bancoDeDados.CriarPlayerMissao(IdPlayer, IdMissao);
        }
        imageItem.sprite = allListItem[bancoDeDados.GetMissaoIdItem(IdMissao)-1].sprite;
    }
    void Start()
    {
        if (!playTutorial) {
            DefaultPanels();
            return;
        }

        OpenTutorialPanel();
    }

    public void ResetPanels()
    {
        SaveTentativas();
        victoryPanel.SetActive(false); 
        losePanel.SetActive(false);
        tutorialPanel.SetActive(false);
        puzzlePainel.SetActive(false);
        optionsPanel.SetActive(false);
        UiPanel.SetActive(false);
    }
    
    public void DefaultPanels()
    {
        ResetPanels();
        puzzlePainel.SetActive(true);
        UiPanel.SetActive(true);
    }

    public void OpenVictoryPanel()
    {

        ResetPanels();
        SaveMissionComplete();
        victoryPanel.SetActive(true);
    }
    public void SaveTentativas()
    {
        BancoDeDados bancoDeDados = new BancoDeDados();
        int tempTentativas = Tentativas;
        Tentativas = 0;
        tempTentativas += bancoDeDados.GetNumber_Attempts(IdPlayer, IdMissao);
        bancoDeDados.SetNumber_Attempts(IdPlayer,IdMissao, tempTentativas);
    }

    public void SaveMissionComplete()
    {
        BancoDeDados bancoDeDados = new BancoDeDados();
        Tentativas += 1;
        SaveTentativas();
        //Debug.Log("SveMission");
        if (bancoDeDados.GetIsMissionComplete(IdPlayer,IdMissao)==1)
        {
            if (Move_To_Complete < bancoDeDados.GetMove_To_Complete(IdPlayer,IdMissao))
            {
                bancoDeDados.SetMove_To_Complete(IdPlayer, IdMissao, Move_To_Complete);
                Debug.Log($"{Move_To_Complete}");
            }
                return;
        }
        itemSpawnCache.itemSpawnId = bancoDeDados.GetMissaoIdItem(IdMissao);
        //Debug.Log($"{Move_To_Complete}");
        bancoDeDados.SetIsMissionComplete(IdPlayer, IdMissao, 1);
        bancoDeDados.SetMove_To_Complete(IdPlayer,IdMissao, Move_To_Complete);

    }
    public void OpenLosePanel()
    {
        ResetPanels();
        losePanel.SetActive(true);
    }
    public void OpenTutorialPanel()
    {
        ResetPanels();
        tutorialPanel.SetActive(true);
    }
    public void OpenOptionsPanel()
    {
        ResetPanels();
        optionsPanel.SetActive(true);
    }


    public void SwitchScennes(string sceneName)
    {
        SaveTentativas();
        if (SceneManager.GetSceneByName(sceneName) != null)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("A cena " + sceneName + " não foi encontrada!");
        }
    }

}
