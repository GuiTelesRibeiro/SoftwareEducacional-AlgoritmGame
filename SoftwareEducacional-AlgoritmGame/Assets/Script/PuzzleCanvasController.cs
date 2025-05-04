using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PuzzleCanvasController : MonoBehaviour
{

    [SerializeField] PlayerController playerController;

    [SerializeField] GameObject victoryPanel;

    [SerializeField] GameObject losePanel;
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] GameObject puzzlePainel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject UiPanel;

    public int Tentativas;
    [SerializeField] int idPlayer;
    // -------------------------------- Level DATA
    [SerializeField] int idLevel;
    [SerializeField] int id_item_to_recive;
    [SerializeField] int level_description;

    //---------------------------------Attempt DATA
    int lastIdAtempt;
    [SerializeField] bool playTutorial;
    float startTime;
    bool isResetTime;
    int secondsToSolve;

    public int numberOfCommands;
    [SerializeField] public Image imageItem;
    [SerializeField] Item[] allListItem;
    [SerializeField] ItemSpawnCache itemSpawnCache;
    [SerializeField] AudioSource Sucess;
    [SerializeField] AudioSource Error;

    int IdToItemToRecive;

    // Start is called before the first frame update
    private void Awake()
    {
        GetLevelById(idLevel);
        
        //Tentativas = 0;
        BancoDeDados bancoDeDados = new BancoDeDados();
        bancoDeDados.InitializeDatabase();
        //if (!bancoDeDados.VerificarPlayerMissaoExiste(idPlayer, idLevel))
        //{
        //    Debug.Log("PlayerMissao Nao existe ainda");
        //    bancoDeDados.CriarPlayerMissao(idPlayer, idLevel);
        //}
        imageItem.sprite = allListItem[IdToItemToRecive - 1].sprite;
        // pegar informações do Level por meio do id Missao: (Usando dicionario pra pegar logo todas as informaçoes e alocando em uma variacvel externa)
        // iniciar Attempt
        // (definir Id da tentativa atual
        // Iniciar a contagem e atualizaçao dos seus dados tentativa atual
        // ao errar salvar os dados, criar nova tentativa,
        // Ao acertar salvar os dados, definir id tentativa como -1
    }
    void Start()
    {
        if (!playTutorial) {
            DefaultPanels();
            return;
        }

        OpenTutorialPanel();
    }

    void GetLevelById(int id_level){
        BancoDeDados bancoDeDados = new BancoDeDados();
       bancoDeDados.InitializeDatabase();
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

    public void SaveAttempt(bool is_failed_attempts)
    {
        BancoDeDados bancoDeDados = new BancoDeDados();
        numberOfCommands = playerController.numberOfCommands();
        lastIdAtempt = bancoDeDados.SetAttemptData(idPlayer, idLevel, numberOfCommands, secondsToSolve, is_failed_attempts);
        if (bancoDeDados.IsFirstComplete(idPlayer, idLevel))
        {
            itemSpawnCache.itemSpawnId = IdToItemToRecive;
        }
    }
    public void StartTimeCount()
    {
        if (isResetTime == true)
        {
            startTime = Time.time; // Marca o tempo inicial em segundos desde o início do jogo
            isResetTime = false;
        }
    }

    public void StopTimeCount()
    {
        float tempoTotal = Time.time - startTime; // Tempo decorrido em segundos
        secondsToSolve = Mathf.RoundToInt(tempoTotal); // Armazena como inteiro (segundos)
        isResetTime = true;
    }


    public void ResetPanels()
    {
        victoryPanel.SetActive(false); 
        losePanel.SetActive(false);
        tutorialPanel.SetActive(false);
        puzzlePainel.SetActive(false);
        optionsPanel.SetActive(false);
        UiPanel.SetActive(false);
    }
    
    public void DefaultPanels()
    {
        StartTimeCount();
        ResetPanels();
        puzzlePainel.SetActive(true);
        UiPanel.SetActive(true);
    }

    public void OpenVictoryPanel()
    {
        Sucess.Play();
        ResetPanels();
        SaveAttempt( false);
        victoryPanel.SetActive(true);
    }
    public void OpenLosePanel()
    {
        Error.Play();
        ResetPanels();
        SaveAttempt( true);
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
