using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Para usar TMP_InputField e TMP_Text

public class PlayerInfoDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameText; // Campo para exibir o nome do jogador
    [SerializeField] private TMP_Text playerAgeText;  // Campo para exibir a idade do jogador

    private BancoDeDados bancoDeDados = new BancoDeDados();

    void Start()
    {
        DisplayPlayerInfo(1);
    }
    // Método para buscar informações do jogador pelo ID
    public void DisplayPlayerInfo(int playerId)
    {
        var dados = bancoDeDados.LerPlayer(playerId);

        if (dados.Read())
        {
            // Exibe as informações do jogador nos campos de texto
            playerNameText.text = $"Nome: {dados["Player_Name"].ToString()}";
            playerAgeText.text = $"Idade: {dados["Player_Idade"].ToString()}";
        }
        else
        {
            // Caso o jogador não exista
            playerNameText.text = "Jogador não encontrado!";
            playerAgeText.text = "";
        }

        dados.Close();
        bancoDeDados.FecharConexao();
    }
}
