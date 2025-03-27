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
    // M�todo para buscar informa��es do jogador pelo ID
    public void DisplayPlayerInfo(int playerId)
    {
        var dados = bancoDeDados.ReadPlayer(playerId);

        if (dados.Read())
        {
            // Exibe as informa��es do jogador nos campos de texto
            playerNameText.text = $"{dados["Player_Name"].ToString()}.";
            playerAgeText.text = $"{dados["Player_Idade"].ToString()} anos.";
        }
        else
        {
            // Caso o jogador n�o exista
            playerNameText.text = "Jogador n�o encontrado!";
            playerAgeText.text = "";
        }

        dados.Close();
        bancoDeDados.CloseDatabase();
    }
}
