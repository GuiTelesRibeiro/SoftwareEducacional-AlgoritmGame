using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class InputPlayerInfos : MonoBehaviour
{
    [SerializeField] TMP_InputField userName;
    [SerializeField] TMP_InputField idade;
    int gradeNumber = -1;
    [SerializeField] TMP_Text buttonText;
    [SerializeField]  LoginCanvasController loginCanvasController;
    [SerializeField] MenuCanvasController menuCanvasController;

    [SerializeField] int idPlayer = 1;
    private BancoDeDados bancoDeDados = new BancoDeDados();

    public void CreateAccount()
    {
        if (userName.text == "" || idade.text == "" || gradeNumber == -1)
        {
            buttonText.text = "Não deixe campos vazios";
            return;
        }
        SubmitInfo();
        loginCanvasController.CutScene();
    }

    public void UpdateAccount()
    {
        if (userName.text == "" || idade.text == "" || gradeNumber == -1)
        {
            buttonText.text = "Não deixe campos vazios";
            return;
        }
        SubmitInfo();
        menuCanvasController.OpenMenu();
    }
    public void SubmitInfo()
    {
        string nome = userName.text;
        int idadeJogador = int.Parse(idade.text);
        int idJogador = idPlayer;
        int grade = gradeNumber;
        bancoDeDados.SetOrUpdatePlayerData(idJogador, nome, idadeJogador, grade);
        var dados = bancoDeDados.ReadPlayer(idJogador);
        if (dados.Read())
        {
            string nomeLido = dados["name"].ToString();
            string idadeLida = dados["age"].ToString();
            string escolaridadeLida = dados["grade"].ToString();
            Debug.Log($"Jogador atualizado/criado com sucesso! Nome: {nomeLido}, Idade: {idadeLida} , escolaridade: {escolaridadeLida}");
        }
        else
        {
            Debug.Log("Erro: Jogador não encontrado após inserção/atualização.");
        }
        dados.Close();
    }

    public void SetGrade(int grade)
    {
        gradeNumber = grade;
    }
}
