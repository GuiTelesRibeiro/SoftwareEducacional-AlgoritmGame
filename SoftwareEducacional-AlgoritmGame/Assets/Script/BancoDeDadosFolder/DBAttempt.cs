using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Globalization;
using UnityEngine.Networking;
using System;

public class DBAttempt
{
    private IDbConnection BancoDados;
    private string ObterCaminhoBanco()
    {
        string origem = System.IO.Path.Combine(Application.streamingAssetsPath, "DB.db");
        string destino = System.IO.Path.Combine(Application.persistentDataPath, "DB.db");

        if (!System.IO.File.Exists(destino))
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                using (UnityWebRequest www = UnityWebRequest.Get(origem))
                {
                    www.SendWebRequest();
                    while (!www.isDone) { }

                    if (www.result == UnityWebRequest.Result.Success)
                    {
                        System.IO.File.WriteAllBytes(destino, www.downloadHandler.data);
                    }
                    else
                    {
                        Debug.LogError($"Erro ao copiar banco de dados: {www.error}");
                    }
                }
            }
            else
            {
                // No PC ou iOS, pode-se copiar diretamente
                System.IO.File.Copy(origem, destino);
            }
        }

        //Debug.Log($"{destino}");
        return destino;
    }
    private IDbConnection criarEAbrirBancoDeDados()
    {
        string caminhoBanco = ObterCaminhoBanco(); // Use o novo método
        string idburi = $"URI=file:{caminhoBanco}";
        IDbConnection conexaoBanco = new SqliteConnection(idburi);
        conexaoBanco.Open();
        using (var comandoCriarTabelas = conexaoBanco.CreateCommand())
        {
            comandoCriarTabelas.CommandText = @"
            CREATE TABLE IF NOT EXISTS Player (
                id_Player INTEGER PRIMARY KEY AUTOINCREMENT,
                Player_Name TEXT,
                Player_Idade INTEGER,
                Player_Inventory_Items BLOB
            );";
            comandoCriarTabelas.ExecuteNonQuery();
        }
        return conexaoBanco;
    }

    public void SetAttemptData()
    {
        ////Attempt
        //id_attempt
        //id_player
        //id_missao
        //is_first_attempt
        //number_of_commands
        //time_to_recive
        //is_failed_attenpts
        //time_data
    }
    public int GetAttempt_Id_attempt()
    {
        return 0;
    }
    public int GetAttempt_Id_Player()
    {
        return 0;
    }
    public int GetAttempt_Id_Missao()
    {
        return 0;
    }
    public bool GetAttempt_Is_First_Attempt()
    {
        return false;
    }
    public int GetAttempt_Number_Of_Commands()
    {
        return 0;
    }
    public int GetAttempt_Time_To_Recive()
    {
        return 0;
    }
    public bool GetAttempt_Is_Failed_Attenpts()
    {
        return false;
    }
    public int GetAttempt_Time_Data()
    {
        return 0;
    }
}
