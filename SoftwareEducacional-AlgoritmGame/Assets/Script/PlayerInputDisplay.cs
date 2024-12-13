using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInputDisplay : MonoBehaviour
{
    [SerializeField] private TMP_InputField userName; 
    [SerializeField] private TMP_InputField userAge;  

    private BancoDeDados bancoDeDados = new BancoDeDados();

    void Start()
    {
        DisplayPlayerInfo(1);
    }
    public void DisplayPlayerInfo(int playerId)
    {
        var dados = bancoDeDados.LerPlayer(playerId);

        if (dados.Read())
        {
            // Exibe as informa��es do jogador nos campos de texto
            userName.text = $"{dados["Player_Name"].ToString()}";
            userAge.text = $"{dados["Player_Idade"].ToString()}";
        }
        else
        {
            // Caso o jogador n�o exista
            userName.text = "Jogador n�o encontrado!";
            userAge.text = "";
        }

        dados.Close();
        bancoDeDados.FecharConexao();
    }
}
