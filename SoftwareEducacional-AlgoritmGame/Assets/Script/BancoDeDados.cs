using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Globalization;
using UnityEngine.Networking;

public class BancoDeDados
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
                // No Android, StreamingAssets usa UnityWebRequest para copiar
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

        return destino;
    }

    private IDbConnection criarEAbrirBancoDeDados()
    {
        string caminhoBanco = ObterCaminhoBanco(); // Use o novo m�todo
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


    public void InserirOuAtualizarPlayer(int id, string nome, int idade)
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand InserOuAtualizaDados = BancoDados.CreateCommand();

        // Verifica se o jogador j� existe
        InserOuAtualizaDados.CommandText = $"SELECT COUNT(*) FROM Player WHERE id_Player = {id};";
        int count = int.Parse(InserOuAtualizaDados.ExecuteScalar().ToString());

        if (count > 0)
        {
            // Atualiza o jogador existente
            InserOuAtualizaDados.CommandText = $@"
                UPDATE Player
                SET Player_Name = '{nome}', Player_Idade = {idade}
                WHERE id_Player = {id};";
        }
        else
        {
            // Insere um novo jogador
            InserOuAtualizaDados.CommandText = $@"
                INSERT INTO Player (id_Player, Player_Name, Player_Idade)
                VALUES ({id}, '{nome}', {idade});";
        }

        InserOuAtualizaDados.ExecuteNonQuery();
        BancoDados.Close();
    }

    public IDataReader LerPlayer(int id)
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand ComandoLer = BancoDados.CreateCommand();
        ComandoLer.CommandText = $"SELECT * FROM Player WHERE id_Player = {id};";
        return ComandoLer.ExecuteReader();
    }

    public void FecharConexao()
    {
        if (BancoDados != null && BancoDados.State != ConnectionState.Closed)
        {
            BancoDados.Close();
        }
    }
}