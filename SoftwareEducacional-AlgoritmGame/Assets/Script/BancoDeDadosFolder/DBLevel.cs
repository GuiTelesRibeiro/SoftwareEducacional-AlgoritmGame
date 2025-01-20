using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Globalization;
using UnityEngine.Networking;
using System;

public class DBLevel
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
                Player_Inventory__Items BLOB
            );";
            comandoCriarTabelas.ExecuteNonQuery();
        }
        return conexaoBanco;
    }
    
    public void SetLevelData()
    {
        ////Level
        //id_level
        //id_item_to_recive
        //level_description
    }

    public int GetLevel_Id_Level()
    {
        return 0;
    }
    public int GetLevel_Id_Item_To_Recive()
    {
        return 0;
    }
    public string GetLevel_Level_Description()
    {
        return "";
    }


}
