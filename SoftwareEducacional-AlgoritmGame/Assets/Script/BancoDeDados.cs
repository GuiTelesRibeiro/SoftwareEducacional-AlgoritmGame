using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Globalization;
using UnityEngine.Networking;
using System;

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
    public void InserirOuAtualizarPlayer(int id, string nome, int idade)
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand InserOuAtualizaDados = BancoDados.CreateCommand();
        InserOuAtualizaDados.CommandText = $"SELECT COUNT(*) FROM Player WHERE id_Player = {id};";
        int count = int.Parse(InserOuAtualizaDados.ExecuteScalar().ToString());
        if (count > 0)
        {
            InserOuAtualizaDados.CommandText = $@"
                UPDATE Player
                SET Player_Name = '{nome}', Player_Idade = {idade}
                WHERE id_Player = {id};";
        }
        else
        {
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
    public void SalvarInventario(int id, int[] inventario)
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand ComandoSalvar = BancoDados.CreateCommand();

        // Converte o array de inteiros para um array de bytes
        List<byte> blob = new List<byte>();
        foreach (int numero in inventario)
        {
            blob.AddRange(System.BitConverter.GetBytes(numero));
        }

        ComandoSalvar.CommandText = $@"
        UPDATE Player
        SET Player_Inventory__Items = @Inventory
        WHERE id_Player = {id};";

        IDbDataParameter parametro = ComandoSalvar.CreateParameter();
        parametro.ParameterName = "@Inventory";
        parametro.Value = blob.ToArray(); // Passa o array de bytes como parâmetro
        ComandoSalvar.Parameters.Add(parametro);

        ComandoSalvar.ExecuteNonQuery();
        BancoDados.Close();
    }
    public int[] LerInventario(int id)
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand ComandoLer = BancoDados.CreateCommand();
        ComandoLer.CommandText = $"SELECT Player_Inventory__Items FROM Player WHERE id_Player = {id};";

        IDataReader reader = ComandoLer.ExecuteReader();
        if (reader.Read() && !reader.IsDBNull(0))
        {
            // Lê os bytes do BLOB
            byte[] blob = (byte[])reader["Player_Inventory__Items"];
            List<int> inventario = new List<int>();

            for (int i = 0; i < blob.Length; i += 4) // Um int ocupa 4 bytes
            {
                inventario.Add(System.BitConverter.ToInt32(blob, i));
            }

            reader.Close();
            BancoDados.Close();
            return inventario.ToArray();
        }
        reader.Close();
        BancoDados.Close();
        return new int[0]; // Retorna um array vazio se não encontrar dados
    }

    // -------------------------------------------------------------------- MISSAO e MISSAO_PLAYER------------------------------------------------------------------------------

    public int GetMissaoIdItem(int idMissao)
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand comando = BancoDados.CreateCommand();
        comando.CommandText = $"SELECT ID_Item FROM Missao WHERE id_missao = {idMissao};";

        IDataReader reader = comando.ExecuteReader();
        int idItem = -1;
        if (reader.Read())
        {
            idItem = reader.GetInt32(0);
        }

        reader.Close();
        BancoDados.Close();
        return idItem;
    }

    public string GetMissaoDescricao(int idMissao)
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand comando = BancoDados.CreateCommand();
        comando.CommandText = $"SELECT Descricao FROM Missao WHERE id_missao = {idMissao};";

        IDataReader reader = comando.ExecuteReader();
        string descricao = null;
        if (reader.Read())
        {
            descricao = reader.GetString(0);
        }

        reader.Close();
        BancoDados.Close();
        return descricao;
    }

    public string GetMissionName(int idMissao)
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand comando = BancoDados.CreateCommand();
        comando.CommandText = $"SELECT MissionName FROM Missao WHERE id_missao = {idMissao};";

        IDataReader reader = comando.ExecuteReader();
        string missionName = null;
        if (reader.Read())
        {
            missionName = reader.GetString(0);
        }

        reader.Close();
        BancoDados.Close();
        return missionName;
    }

    // Chat GPT Só altere códigos abaixo desta linha!!

    public void CriarPlayerMissao(int IDPlayer, int IDMissao)
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand comando = BancoDados.CreateCommand();
        comando.CommandText = @"INSERT INTO Player_Missao (ID_Player, ID_Missao, Move_To_Complete, Number_Attempts, IsMissionComplete, isItemDelivered)
                            VALUES (@IDPlayer, @IDMissao,  null, 0, 0, 0);";

        IDbDataParameter paramPlayer = comando.CreateParameter();
        paramPlayer.ParameterName = "@IDPlayer";
        paramPlayer.Value = IDPlayer;
        comando.Parameters.Add(paramPlayer);

        IDbDataParameter paramMissao = comando.CreateParameter();
        paramMissao.ParameterName = "@IDMissao";
        paramMissao.Value = IDMissao;
        comando.Parameters.Add(paramMissao);

        comando.ExecuteNonQuery();
        BancoDados.Close();
    }

    public bool VerificarPlayerMissaoExiste(int IDPlayer, int IDMissao)
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand comando = BancoDados.CreateCommand();
        comando.CommandText = "SELECT COUNT(*) FROM Player_Missao WHERE ID_Player = @IDPlayer AND ID_Missao = @IDMissao;";

        IDbDataParameter paramPlayer = comando.CreateParameter();
        paramPlayer.ParameterName = "@IDPlayer";
        paramPlayer.Value = IDPlayer;
        comando.Parameters.Add(paramPlayer);

        IDbDataParameter paramMissao = comando.CreateParameter();
        paramMissao.ParameterName = "@IDMissao";
        paramMissao.Value = IDMissao;
        comando.Parameters.Add(paramMissao);

        object resultado = comando.ExecuteScalar();
        BancoDados.Close();

        // Convertendo o resultado para inteiro
        int count = resultado != null && resultado != DBNull.Value ? Convert.ToInt32(resultado) : 0;

        return count > 0 ? true : false;
    }


    public int GetMove_To_Complete(int IDPlayer, int IDMissao)
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand comando = BancoDados.CreateCommand();
        comando.CommandText = "SELECT Move_To_Complete FROM Player_Missao WHERE ID_Player = @IDPlayer AND ID_Missao = @IDMissao;";

        IDbDataParameter paramPlayer = comando.CreateParameter();
        paramPlayer.ParameterName = "@IDPlayer";
        paramPlayer.Value = IDPlayer;
        comando.Parameters.Add(paramPlayer);

        IDbDataParameter paramMissao = comando.CreateParameter();
        paramMissao.ParameterName = "@IDMissao";
        paramMissao.Value = IDMissao;
        comando.Parameters.Add(paramMissao);

        IDataReader reader = comando.ExecuteReader();
        int moves = reader.Read() ? reader.GetInt32(0) : 0;

        reader.Close();
        BancoDados.Close();
        return moves;
    }

    public void SetMove_To_Complete(int IDPlayer, int IDMissao, int moves)
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand comando = BancoDados.CreateCommand();
        comando.CommandText = "UPDATE Player_Missao SET Move_To_Complete = @moves WHERE ID_Player = @IDPlayer AND ID_Missao = @IDMissao;";

        IDbDataParameter paramMoves = comando.CreateParameter();
        paramMoves.ParameterName = "@moves";
        paramMoves.Value = moves;
        comando.Parameters.Add(paramMoves);

        IDbDataParameter paramPlayer = comando.CreateParameter();
        paramPlayer.ParameterName = "@IDPlayer";
        paramPlayer.Value = IDPlayer;
        comando.Parameters.Add(paramPlayer);

        IDbDataParameter paramMissao = comando.CreateParameter();
        paramMissao.ParameterName = "@IDMissao";
        paramMissao.Value = IDMissao;
        comando.Parameters.Add(paramMissao);

        comando.ExecuteNonQuery();
        BancoDados.Close();
    }

    public int GetNumber_Attempts(int IDPlayer, int IDMissao)
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand comando = BancoDados.CreateCommand();
        comando.CommandText = "SELECT Number_Attempts FROM Player_Missao WHERE ID_Player = @IDPlayer AND ID_Missao = @IDMissao;";

        IDbDataParameter paramPlayer = comando.CreateParameter();
        paramPlayer.ParameterName = "@IDPlayer";
        paramPlayer.Value = IDPlayer;
        comando.Parameters.Add(paramPlayer);

        IDbDataParameter paramMissao = comando.CreateParameter();
        paramMissao.ParameterName = "@IDMissao";
        paramMissao.Value = IDMissao;
        comando.Parameters.Add(paramMissao);

        IDataReader reader = comando.ExecuteReader();
        int attempts = reader.Read() ? reader.GetInt32(0) : 0;

        reader.Close();
        BancoDados.Close();
        return attempts;
    }

    public void SetNumber_Attempts(int IDPlayer, int IDMissao, int attempts)
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand comando = BancoDados.CreateCommand();
        comando.CommandText = "UPDATE Player_Missao SET Number_Attempts = @attempts WHERE ID_Player = @IDPlayer AND ID_Missao = @IDMissao;";

        IDbDataParameter paramAttempts = comando.CreateParameter();
        paramAttempts.ParameterName = "@attempts";
        paramAttempts.Value = attempts;
        comando.Parameters.Add(paramAttempts);

        IDbDataParameter paramPlayer = comando.CreateParameter();
        paramPlayer.ParameterName = "@IDPlayer";
        paramPlayer.Value = IDPlayer;
        comando.Parameters.Add(paramPlayer);

        IDbDataParameter paramMissao = comando.CreateParameter();
        paramMissao.ParameterName = "@IDMissao";
        paramMissao.Value = IDMissao;
        comando.Parameters.Add(paramMissao);

        comando.ExecuteNonQuery();
        BancoDados.Close();
    }

    public int GetIsMissionComplete(int IDPlayer, int IDMissao)
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand comando = BancoDados.CreateCommand();
        comando.CommandText = "SELECT IsMissionComplete FROM Player_Missao WHERE ID_Player = @IDPlayer AND ID_Missao = @IDMissao;";

        IDbDataParameter paramPlayer = comando.CreateParameter();
        paramPlayer.ParameterName = "@IDPlayer";
        paramPlayer.Value = IDPlayer;
        comando.Parameters.Add(paramPlayer);

        IDbDataParameter paramMissao = comando.CreateParameter();
        paramMissao.ParameterName = "@IDMissao";
        paramMissao.Value = IDMissao;
        comando.Parameters.Add(paramMissao);

        IDataReader reader = comando.ExecuteReader();
        int isComplete = reader.Read() ? reader.GetInt32(0) : 0;

        reader.Close();
        BancoDados.Close();
        return isComplete;
    }

    public void SetIsMissionComplete(int IDPlayer, int IDMissao, int isComplete)
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand comando = BancoDados.CreateCommand();
        comando.CommandText = "UPDATE Player_Missao SET IsMissionComplete = @isComplete WHERE ID_Player = @IDPlayer AND ID_Missao = @IDMissao;";

        IDbDataParameter paramIsComplete = comando.CreateParameter();
        paramIsComplete.ParameterName = "@isComplete";
        paramIsComplete.Value = isComplete;
        comando.Parameters.Add(paramIsComplete);

        IDbDataParameter paramPlayer = comando.CreateParameter();
        paramPlayer.ParameterName = "@IDPlayer";
        paramPlayer.Value = IDPlayer;
        comando.Parameters.Add(paramPlayer);

        IDbDataParameter paramMissao = comando.CreateParameter();
        paramMissao.ParameterName = "@IDMissao";
        paramMissao.Value = IDMissao;
        comando.Parameters.Add(paramMissao);

        comando.ExecuteNonQuery();
        BancoDados.Close();
    }
    //------------------------

    public int GetIsItemDelivered(int IDPlayer, int IDMissao)
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand comando = BancoDados.CreateCommand();
        comando.CommandText = "SELECT isItemDelivered FROM Player_Missao WHERE ID_Player = @IDPlayer AND ID_Missao = @IDMissao;";

        IDbDataParameter paramPlayer = comando.CreateParameter();
        paramPlayer.ParameterName = "@IDPlayer";
        paramPlayer.Value = IDPlayer;
        comando.Parameters.Add(paramPlayer);

        IDbDataParameter paramMissao = comando.CreateParameter();
        paramMissao.ParameterName = "@IDMissao";
        paramMissao.Value = IDMissao;
        comando.Parameters.Add(paramMissao);

        IDataReader reader = comando.ExecuteReader();
        int isDelivered = reader.Read() ? reader.GetInt32(0) : 0;

        reader.Close();
        BancoDados.Close();
        return isDelivered;
    }

    public void SetIsItemDelivered(int IDPlayer, int IDMissao, int isComplete)
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand comando = BancoDados.CreateCommand();
        comando.CommandText = "UPDATE Player_Missao SET isItemDelivered = @isComplete WHERE ID_Player = @IDPlayer AND ID_Missao = @IDMissao;";

        IDbDataParameter paramIsComplete = comando.CreateParameter();
        paramIsComplete.ParameterName = "@isComplete";
        paramIsComplete.Value = isComplete;
        comando.Parameters.Add(paramIsComplete);

        IDbDataParameter paramPlayer = comando.CreateParameter();
        paramPlayer.ParameterName = "@IDPlayer";
        paramPlayer.Value = IDPlayer;
        comando.Parameters.Add(paramPlayer);

        IDbDataParameter paramMissao = comando.CreateParameter();
        paramMissao.ParameterName = "@IDMissao";
        paramMissao.Value = IDMissao;
        comando.Parameters.Add(paramMissao);

        comando.ExecuteNonQuery();
        BancoDados.Close();
    }

    //------------------------
    public int GetMaiorIDMissao(int IDPlayer)
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand comando = BancoDados.CreateCommand();

        // Consulta para pegar o maior ID_Missao onde IsMissionComplete == 1
        comando.CommandText = $"SELECT MAX(ID_Missao) FROM Player_Missao WHERE ID_Player = {IDPlayer} AND IsMissionComplete = 1;";

        object resultado = comando.ExecuteScalar();
        BancoDados.Close();

        if (resultado != null && resultado != DBNull.Value)
        {
            return Convert.ToInt32(resultado);
        }

        return 0;
    }
}
