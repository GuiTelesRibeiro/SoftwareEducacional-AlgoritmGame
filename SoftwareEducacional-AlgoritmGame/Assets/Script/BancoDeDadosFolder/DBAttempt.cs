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
            CREATE TABLE IF NOT EXISTS Attempt (
                id_attempt INTEGER PRIMARY KEY AUTOINCREMENT,
                id_player INT NOT NULL,
                id_missao INT NOT NULL,
                is_first_attempt BOOLEAN NOT NULL,
                number_of_commands INT NOT NULL,
                time_to_recive INT NOT NULL,
                is_failed_attempt BOOLEAN NOT NULL,
                time_data DATETIME NOT NULL,
                FOREIGN KEY (id_player) REFERENCES Player(id_player),
                FOREIGN KEY (id_missao) REFERENCES Level(id_level)
            );";
            comandoCriarTabelas.ExecuteNonQuery();
        }
        return conexaoBanco;
    }

    public int SetAttemptData(int id_player, int id_missao, bool is_first_attempt, int number_of_commands, int time_to_recive, bool is_failed_attempt, DateTime time_data)
    {
        int attemptId = -1;

        BancoDados = criarEAbrirBancoDeDados();
        using (var comando = BancoDados.CreateCommand())
        {
            comando.CommandText = @"
            INSERT INTO Attempt (id_player, id_missao, is_first_attempt, number_of_commands, time_to_recive, is_failed_attempt, time_data)
            VALUES (@id_player, @id_missao, @is_first_attempt, @number_of_commands, @time_to_recive, @is_failed_attempt, @time_data);

            SELECT last_insert_rowid();";

            comando.Parameters.Add(new SqliteParameter("@id_player", id_player));
            comando.Parameters.Add(new SqliteParameter("@id_missao", id_missao));
            comando.Parameters.Add(new SqliteParameter("@is_first_attempt", is_first_attempt));
            comando.Parameters.Add(new SqliteParameter("@number_of_commands", number_of_commands));
            comando.Parameters.Add(new SqliteParameter("@time_to_recive", time_to_recive));
            comando.Parameters.Add(new SqliteParameter("@is_failed_attempt", is_failed_attempt));
            comando.Parameters.Add(new SqliteParameter("@time_data", time_data.ToString("yyyy-MM-dd HH:mm:ss")));

            attemptId = Convert.ToInt32(comando.ExecuteScalar());
        }
        BancoDados.Close();

        return attemptId;
    }

    public int GetAttempt_Id_Player(int id_attempt)
    {
        return GetValue<int>("id_player", id_attempt);
    }

    public int GetAttempt_Id_Missao(int id_attempt)
    {
        return GetValue<int>("id_missao", id_attempt);
    }

    public bool GetAttempt_Is_First_Attempt(int id_attempt)
    {
        return GetValue<bool>("is_first_attempt", id_attempt);
    }

    public int GetAttempt_Number_Of_Commands(int id_attempt)
    {
        return GetValue<int>("number_of_commands", id_attempt);
    }

    public int GetAttempt_Time_To_Recive(int id_attempt)
    {
        return GetValue<int>("time_to_recive", id_attempt);
    }

    public bool GetAttempt_Is_Failed_Attempt(int id_attempt)
    {
        return GetValue<bool>("is_failed_attempt", id_attempt);
    }

    public DateTime? GetAttempt_Time_Data(int id_attempt)
    {
        return GetValue<DateTime?>("time_data", id_attempt);
    }

    private T GetValue<T>(string columnName, int id_attempt)
    {
        BancoDados = criarEAbrirBancoDeDados();
        using (var comando = BancoDados.CreateCommand())
        {
            comando.CommandText = $"SELECT {columnName} FROM Attempt WHERE id_attempt = @id_attempt;";
            comando.Parameters.Add(new SqliteParameter("@id_attempt", id_attempt));

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
    public void DeleteAttemptByPlayer(int playerId)
    {
        BancoDados = criarEAbrirBancoDeDados();
        using (var comando = BancoDados.CreateCommand())
        {
            comando.CommandText = "DELETE FROM Attempt WHERE id_player = @playerId;";
            comando.Parameters.Add(new SqliteParameter("@playerId", playerId));

            int rowsAffected = comando.ExecuteNonQuery(); // Retorna o número de linhas afetadas
            Debug.Log($"{rowsAffected} tentativas deletadas para o jogador com ID {playerId}.");
        }
        BancoDados.Close();
    }
    public int GetHighestSuccessfulLevelId(int playerId)
    {
        BancoDados = criarEAbrirBancoDeDados();
        using (var comando = BancoDados.CreateCommand())
        {
            comando.CommandText = @"
        SELECT MAX(id_missao) 
        FROM Attempt 
        WHERE id_player = @playerId AND is_failed_attempt = 0;";

            comando.Parameters.Add(new SqliteParameter("@playerId", playerId));

            object result = comando.ExecuteScalar();
            BancoDados.Close();

            if (result != null && result != DBNull.Value)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }
    }


}

