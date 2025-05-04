using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Globalization;
using UnityEngine.Networking;
using System;
using Unity.VisualScripting;

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
                id_level INT NOT NULL,
                is_first_attempt BOOLEAN NOT NULL,
                number_of_commands INT NOT NULL,
                seconds_to_solve INT NOT NULL,
                is_failed_attempt BOOLEAN NOT NULL,
                time_data DATETIME NOT NULL,
                FOREIGN KEY (id_player) REFERENCES Player(id_player),
                FOREIGN KEY (id_level) REFERENCES Level(id_level)
            );";
            comandoCriarTabelas.ExecuteNonQuery();
        }
        return conexaoBanco;
    }

    public int SetAttemptData(int id_player, int id_level, int number_of_commands, int seconds_to_solve, bool is_failed_attempt)
    {
        int attemptId = -1;
        bool is_first_attempt = IsFirstAttempt(id_player, id_level); // Aqui entra sua lógica
        DateTime time_data = DateTime.Now; // Captura o tempo no momento da inserção
        BancoDados = criarEAbrirBancoDeDados();
        

        using (var comando = BancoDados.CreateCommand())
        {
            comando.CommandText = @"
            INSERT INTO Attempt (id_player, id_level, is_first_attempt, number_of_commands, seconds_to_solve, is_failed_attempt, time_data)
            VALUES (@id_player, @id_level, @is_first_attempt, @number_of_commands, @seconds_to_solve, @is_failed_attempt, @time_data);

            SELECT last_insert_rowid();";

            comando.Parameters.Add(new SqliteParameter("@id_player", id_player));
            comando.Parameters.Add(new SqliteParameter("@id_level", id_level));
            comando.Parameters.Add(new SqliteParameter("@is_first_attempt", is_first_attempt));
            comando.Parameters.Add(new SqliteParameter("@number_of_commands", number_of_commands));
            comando.Parameters.Add(new SqliteParameter("@seconds_to_solve", seconds_to_solve));
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
        return GetValue<int>("id_level", id_attempt);
    }

    public bool IsFirstAttempt(int id_player, int id_level)
    {
        BancoDados = criarEAbrirBancoDeDados();

        using (var comando = BancoDados.CreateCommand())
        {
            comando.CommandText = @"
            SELECT COUNT(*) 
            FROM Attempt 
            WHERE id_player = @id_player 
              AND id_level = @id_level;";

            comando.Parameters.Add(new SqliteParameter("@id_player", id_player));
            comando.Parameters.Add(new SqliteParameter("@id_level", id_level));

            int count = Convert.ToInt32(comando.ExecuteScalar());
            BancoDados.Close();

            return count == 0;
        }
    }


    public int GetAttempt_Number_Of_Commands(int id_attempt)
    {
        return GetValue<int>("number_of_commands", id_attempt);
    }

    public int GetAttempt_Seconds_To_Solve(int id_attempt)
    {
        return GetValue<int>("seconds_to_solve", id_attempt);
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
        SELECT MAX(id_level) 
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

    public bool IsFirstComplete(int idPlayer, int idLevel)
    {
        BancoDados = criarEAbrirBancoDeDados();
        bool isComplete = false;

        using (var comando = BancoDados.CreateCommand())
        {
            comando.CommandText = @"
        SELECT COUNT(*) 
        FROM Attempt 
        WHERE id_player = @idPlayer 
          AND id_level = @idLevel
          AND is_failed_attempt = 0;";  // Considera apenas tentativas bem-sucedidas

            comando.Parameters.Add(new SqliteParameter("@idPlayer", idPlayer));
            comando.Parameters.Add(new SqliteParameter("@idLevel", idLevel));

            int count = Convert.ToInt32(comando.ExecuteScalar());
            isComplete = (count == 1);  // Retorna true apenas se houver exatamente 1 tentativa completa
        }

        BancoDados.Close();
        return isComplete;
    }

}

