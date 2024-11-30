using System.Collections;
using UnityEngine;
using TMPro; // Para usar TMP_InputField

public class TestImputPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField userName; // Campo para o nome do jogador
    [SerializeField] TMP_InputField idade;   // Campo para a idade do jogador
    [SerializeField] TMP_InputField playerId; // Campo para o ID do jogador

    // Instância do banco de dados
    private BancoDeDados bancoDeDados = new BancoDeDados();

    // Função chamada quando o usuário envia as informações
    public void SubmitInfo()
    {
        // Obtém os valores dos campos de entrada
        string nome = userName.text;
        int idadeJogador = int.Parse(idade.text);
        int idJogador = int.Parse(playerId.text);

        // Insere ou atualiza os dados no banco
        bancoDeDados.InserirOuAtualizarPlayer(idJogador, nome, idadeJogador);

        // Lê os dados do jogador para exibição
        var dados = bancoDeDados.LerPlayer(idJogador);

        if (dados.Read()) // Se encontrou o registro
        {
            string nomeLido = dados["Player_Name"].ToString();
            string idadeLida = dados["Player_Idade"].ToString();
            Debug.Log($"Jogador atualizado/criado com sucesso! Nome: {nomeLido}, Idade: {idadeLida}");
        }
        else
        {
            Debug.Log("Erro: Jogador não encontrado após inserção/atualização.");
        }

        dados.Close();
        bancoDeDados.FecharConexao();
    }
}
