using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class PlayerInputDisplay : MonoBehaviour
{
    [SerializeField] private TMP_InputField userName; 
    [SerializeField] private TMP_InputField userAge;
    int userGrade;
    [SerializeField] InputPlayerInfos inputPlayerInfos;
    private BancoDeDados bancoDeDados = new BancoDeDados();

    void Start()
    {
        DisplayPlayerInfo(1);
    }
    public void DisplayPlayerInfo(int playerId)
    {
        var dados = bancoDeDados.ReadPlayer(playerId);

        if (dados.Read())
        {
            // Exibe as informações do jogador nos campos de texto
            userName.text = $"{dados["name"].ToString()}";
            userAge.text = $"{dados["age"].ToString()}";
            userGrade = Convert.ToInt32(dados["grade"]);

            inputPlayerInfos.SetGrade(userGrade);
        }
        else
        {
            // Caso o jogador não exista
            userName.text = "Jogador não encontrado!";
            userAge.text = "";
        }

        dados.Close();
        bancoDeDados.CloseDatabase();
    }
}
