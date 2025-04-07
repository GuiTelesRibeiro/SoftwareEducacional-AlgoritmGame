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
    [SerializeField] int IdLevel;
    [SerializeField] bool playTutorial;
    public int Move_To_Complete;
    [SerializeField] public Image imageItem;
    [SerializeField] Item[] allListItem;
    [SerializeField] ItemSpawnCache itemSpawnCache;
    [SerializeField] AudioSource Sucess;
    [SerializeField] AudioSource Error;

    int IdToItemToRecive;

    // Start is called before the first frame update
    private void Awake()
    {
        GetLevelById(IdLevel);
        
        //Tentativas = 0;
        //BancoDeDados bancoDeDados = new BancoDeDados();
        //if (!bancoDeDados.VerificarPlayerMissaoExiste(IdPlayer, IdLevel))
        //{
        //    Debug.Log("PlayerMissao Nao existe ainda");
        //    bancoDeDados.CriarPlayerMissao(IdPlayer, IdLevel);
        //}
        imageItem.sprite = allListItem[IdToItemToRecive - 1].sprite;


        // (Nao Feito )pegar informações do Level por meio do id Missao: (Usando dicionario pra pegar logo todas as informaçoes e alocando em uma variacel externa)
        // (Nao feito)iniciar Attempt
        // (Nao feito)definir Id da tentativa atual
        // (Nao Feito)Iniciar a contagem e atualizaçao dos seus dados tentativa atual
        // (Nao feito )ao errar salvar os dados, criar nova tentativa,
        // (Nao feito )Ao acertar salvar os dados, definir id tentativa como -1
    }
    void Start()
    {
        if (!playTutorial) {
            DefaultPanels();
            return;
        }

        OpenTutorialPanel();
    }
    public void SaveTentativas()
    {
        BancoDeDados bancoDeDados = new BancoDeDados();
        int tempTentativas = Tentativas;
        Tentativas = 0;
        tempTentativas += bancoDeDados.GetNumber_Attempts(IdPlayer, IdLevel);
        bancoDeDados.SetNumber_Attempts(IdPlayer,IdLevel, tempTentativas);
    }

    void GetLevelById(int id_level){
        BancoDeDados bancoDeDados = new BancoDeDados();
       
        var levelData = bancoDeDados.GetLevelById(id_level);

        if (levelData.Count > 0)
        {
            Debug.Log("ID: " + levelData["id_level"]);
            Debug.Log("Item recebido: " + levelData["id_item_to_recive"]);
            Debug.Log("Descrição: " + levelData["level_description"]);

            IdToItemToRecive = (int)levelData["id_item_to_recive"];

        }
        else
        {
            Debug.Log("Nenhum nível encontrado com esse ID.");
        }
    }

    public void SaveMissionComplete()
    {
        BancoDeDados bancoDeDados = new BancoDeDados();
        Tentativas += 1;
        SaveTentativas();
        //Debug.Log("SveMission");
        if (bancoDeDados.GetIsMissionComplete(IdPlayer,IdLevel)==1)
        {
            if (Move_To_Complete < bancoDeDados.GetMove_To_Complete(IdPlayer,IdLevel))
            {
                bancoDeDados.SetMove_To_Complete(IdPlayer, IdLevel, Move_To_Complete);
                Debug.Log($"{Move_To_Complete}");
            }
                return;
        }
        itemSpawnCache.itemSpawnId = bancoDeDados.GetMissaoIdItem(IdLevel);
        //Debug.Log($"{Move_To_Complete}");
        bancoDeDados.SetIsMissionComplete(IdPlayer, IdLevel, 1);
        bancoDeDados.SetMove_To_Complete(IdPlayer,IdLevel, Move_To_Complete);

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
        Sucess.Play();
        ResetPanels();
        SaveMissionComplete();
        victoryPanel.SetActive(true);
    }
    public void OpenLosePanel()
    {
        Error.Play();
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
