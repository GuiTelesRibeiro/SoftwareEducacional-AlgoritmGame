using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvasController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject jogarPainel;
    [SerializeField] GameObject opcoesPainel;
    [SerializeField] GameObject sobrePainel;
    [SerializeField] GameObject creditosPainel;
    [SerializeField] GameObject menuPainel;
    [SerializeField] GameObject updateDataPainel;
    [SerializeField] GameObject createAccountPainel;

    void Start()
    {
        if (ExistAccount())
        {
            OpenMenu();
            return;
        }
        OpenCreateAccount();
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
        jogarPainel.SetActive(false);
        updateDataPainel.SetActive(false);
        createAccountPainel.SetActive(false);
    }
    // Update is called once per frame

    public void OpenCreateAccount()
    {
        ResetPainel();
        createAccountPainel.SetActive(true);
    }
    public void OpenUpdateDataPainel()
    {
        ResetPainel();
        updateDataPainel.SetActive(true);

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
    public void OpenJogar()
    {
        ResetPainel();
        jogarPainel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
