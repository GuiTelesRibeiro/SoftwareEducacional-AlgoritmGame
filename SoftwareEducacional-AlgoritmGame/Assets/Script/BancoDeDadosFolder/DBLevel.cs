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
                System.IO.File.Copy(origem, destino);
            }
        }

        return destino;
    }

    private IDbConnection criarEAbrirBancoDeDados()
    {
        string caminhoBanco = ObterCaminhoBanco();
        string idburi = $"URI=file:{caminhoBanco}";
        IDbConnection conexaoBanco = new SqliteConnection(idburi);
        conexaoBanco.Open();
        using (var comandoCriarTabelas = conexaoBanco.CreateCommand())
        {
            comandoCriarTabelas.CommandText = @"
            CREATE TABLE IF NOT EXISTS Level (
                id_level INTEGER PRIMARY KEY AUTOINCREMENT,
                id_item_to_recive INT NOT NULL,
                level_description VARCHAR(255) NOT NULL
            );";
            comandoCriarTabelas.ExecuteNonQuery();
        }
        return conexaoBanco;
    }

    public void SetLevelData(int id_level, int id_item_to_recive, string level_description)
    {
        BancoDados = criarEAbrirBancoDeDados();
        using (IDbCommand comando = BancoDados.CreateCommand())
        {
            comando.CommandText = @"
            INSERT INTO Level (id_level, id_item_to_recive, level_description)
            VALUES (@id_level, @id_item_to_recive, @level_description)
            ON CONFLICT(id_level) DO UPDATE SET
                id_item_to_recive = @id_item_to_recive,
                level_description = @level_description;";

            comando.Parameters.Add(new SqliteParameter("@id_level", id_level));
            comando.Parameters.Add(new SqliteParameter("@id_item_to_recive", id_item_to_recive));
            comando.Parameters.Add(new SqliteParameter("@level_description", level_description));
            comando.ExecuteNonQuery();
        }
        BancoDados.Close();
    }

    public int GetLevel_Id_Item_To_Recive(int id_level)
    {
        return GetValue<int>("id_item_to_recive", id_level);
    }

    public string GetLevel_Level_Description(int id_level)
    {
        return GetValue<string>("level_description", id_level);
    }

    private T GetValue<T>(string columnName, int id_level)
    {
        BancoDados = criarEAbrirBancoDeDados();
        using (IDbCommand comando = BancoDados.CreateCommand())
        {
            comando.CommandText = $"SELECT {columnName} FROM Level WHERE id_level = @id_level;";
            comando.Parameters.Add(new SqliteParameter("@id_level", id_level));

            object result = comando.ExecuteScalar();
            BancoDados.Close();

            if (result != null && result != DBNull.Value)
            {
                return (T)Convert.ChangeType(result, typeof(T), CultureInfo.InvariantCulture);
            }
            return default;
        }
    }
    public void CloseDatabase()
    {
        if (BancoDados != null && BancoDados.State != ConnectionState.Closed)
        {
            BancoDados.Close();
            BancoDados = null; // Libera o recurso para evitar vazamentos
            Debug.Log("Banco de dados fechado com sucesso.");
        }
    }

}