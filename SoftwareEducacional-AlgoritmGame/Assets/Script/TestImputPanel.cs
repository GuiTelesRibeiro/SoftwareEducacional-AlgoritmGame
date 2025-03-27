using System.Collections;
using UnityEngine;
using TMPro; // Para usar TMP_InputField

public class TestImputPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField userName; // Campo para o nome do jogador
    [SerializeField] TMP_InputField idade;   // Campo para a idade do jogador
    [SerializeField] TMP_InputField playerId; // Campo para o ID do jogador
    [SerializeField] TMP_InputField gradeTXT; // Campo para o escolaridade do jogador

    // Inst�ncia do banco de dados
    private BancoDeDados bancoDeDados = new BancoDeDados();

    // Fun��o chamada quando o usu�rio envia as informa��es
    public void SubmitInfo()
    {
        // Obt�m os valores dos campos de entrada
        string name = userName.text;
        int age = int.Parse(idade.text);
        int idPlayer = int.Parse(playerId.text);

        int grade = int.Parse(gradeTXT.text);


        // Insere ou atualiza os dados no banco
        bancoDeDados.SetOrUpdatePlayerData(idPlayer, name, age, grade);

        // L� os dados do jogador para exibi��o
        var dados = bancoDeDados.ReadPlayer(idPlayer);

        if (dados.Read()) // Se encontrou o registro
        {
            string nomeLido = dados["name"].ToString();
            string idadeLida = dados["age"].ToString();
            string gradeLida = dados["grade"].ToString();
            Debug.Log($"Jogador atualizado/criado com sucesso! Nome: {nomeLido}, Idade: {idadeLida}, Grade: {gradeLida}");
        }
        else
        {
            Debug.Log("Erro: Jogador n�o encontrado ap�s inser��o/atualiza��o.");
        }

        dados.Close();
        bancoDeDados.CloseDatabase();
    }
}
