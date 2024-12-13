using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuCanvasController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject opcoesPainel;
    [SerializeField] GameObject sobrePainel;
    [SerializeField] GameObject creditosPainel;
    [SerializeField] GameObject menuPainel;
    [SerializeField] GameObject updateDataPainel;
    [SerializeField] GameObject resetAlert;
    [SerializeField] string nomeDaCenadoJogo;
    [SerializeField] string nomeDaCenadaIntroducao;

    private void Awake()
    {
        OpenAll();
    }
    void Start()
    {

            OpenMenu();

    }
    public void OpenAll()
    {
        menuPainel.SetActive(true);
        opcoesPainel.SetActive(true);
        sobrePainel.SetActive(true);
        creditosPainel.SetActive(true);
        updateDataPainel.SetActive(true);
        resetAlert.SetActive(true);
    }
    public bool ExistAccount()
    {
        BancoDeDados bancoDeDados = new BancoDeDados();
        var dados = bancoDeDados.LerPlayer(1); // Passa o ID 1 para buscar no banco de dados.

        bool existe = dados.Read(); // Se houver registros, retorna true.

        dados.Close(); // Fecha o leitor para liberar recursos.
        bancoDeDados.FecharConexao(); // Fecha a conexão com o banco de dados.
        return existe;
    }


    public void ResetPainel()
    {
        menuPainel.SetActive(false);
        opcoesPainel.SetActive(false);
        sobrePainel.SetActive(false);
        creditosPainel.SetActive(false);
        updateDataPainel.SetActive(false);
        resetAlert.SetActive(false);
    }
    // Update is called once per frame
    public void OpenUpdateDataPainel()
    {
        ResetPainel();
        updateDataPainel.SetActive(true);

    }

    public void OpenResetAlert()
    {
        ResetPainel();
        resetAlert.SetActive(true);
    }

    public void ResetGame(int playerId)
    {
        BancoDeDados bancoDeDados = new BancoDeDados();
        bancoDeDados.DeletePlayerById(playerId);
        bancoDeDados.DeletarPlayerMissoes(playerId); 

        OpenMenu();
    }
    public void OpenMenu()
    {
        ResetPainel();
        menuPainel.SetActive(true);
    }

    public void OpenOpcoes()
    {
        ResetPainel();
        opcoesPainel.SetActive(true);
    }
    public void OpenSobre()
    {
        ResetPainel();
        sobrePainel.SetActive(true);
    }
    public void OpenCreditos()
    {
        ResetPainel();
        creditosPainel.SetActive(true);
    }
    public void Jogar()
    {
        if (ExistAccount())
        {
            SceneManager.LoadScene(nomeDaCenadoJogo);
            return;
        }
        SceneManager.LoadScene(nomeDaCenadaIntroducao);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
