using System.Collections;
using UnityEngine;
using TMPro; // Para usar TMP_InputField

public class TestImputPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField userName; // Campo para o nome do jogador
    [SerializeField] TMP_InputField idade;   // Campo para a idade do jogador
    [SerializeField] TMP_InputField playerId; // Campo para o ID do jogador

    // Inst�ncia do banco de dados
    private BancoDeDados bancoDeDados = new BancoDeDados();

    // Fun��o chamada quando o usu�rio envia as informa��es
    public void SubmitInfo()
    {
        // Obt�m os valores dos campos de entrada
        string nome = userName.text;
        int idadeJogador = int.Parse(idade.text);
        int idJogador = int.Parse(playerId.text);

        // Insere ou atualiza os dados no banco
        bancoDeDados.InserirOuAtualizarPlayer(idJogador, nome, idadeJogador);

        // L� os dados do jogador para exibi��o
        var dados = bancoDeDados.LerPlayer(idJogador);

        if (dados.Read()) // Se encontrou o registro
        {
            string nomeLido = dados["Player_Name"].ToString();
            string idadeLida = dados["Player_Idade"].ToString();
            Debug.Log($"Jogador atualizado/criado com sucesso! Nome: {nomeLido}, Idade: {idadeLida}");
        }
        else
        {
            Debug.Log("Erro: Jogador n�o encontrado ap�s inser��o/atualiza��o.");
        }

        dados.Close();
        bancoDeDados.FecharConexao();
    }
}
