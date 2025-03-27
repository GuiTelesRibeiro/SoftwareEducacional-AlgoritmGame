using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Globalization;
using UnityEngine.Networking;
using System;

public class DBPlayer
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
            CREATE TABLE IF NOT EXISTS Player (
                id_player INTEGER PRIMARY KEY AUTOINCREMENT,
                name VARCHAR(100) NOT NULL,
                age INT NOT NULL,
                grade INT NOT NULL,
                inventory_items BLOB,
                itens_delivered_list BLOB
            );";
            comandoCriarTabelas.ExecuteNonQuery();
        }
        return conexaoBanco;
    }

    public void SetOrUpdatePlayerData(int id_player, string nome, int idade, int grade)
    {
        BancoDados = criarEAbrirBancoDeDados();

        // Verifica se o jogador já existe
        bool jogadorExiste = false;
        using (IDbCommand checkCommand = BancoDados.CreateCommand())
        {
            checkCommand.CommandText = "SELECT COUNT(*) FROM Player WHERE id_player = @id_player";
            checkCommand.Parameters.Add(new SqliteParameter("@id_player", id_player));
            jogadorExiste = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;
        }

        using (IDbCommand comando = BancoDados.CreateCommand())
        {
            if (jogadorExiste)
            {
                // Atualiza se já existir
                comando.CommandText = @"
                UPDATE Player 
                SET name = @name, age = @age, grade = @grade 
                WHERE id_player = @id_player";
            }
            else
            {
                // Insere um novo jogador se não existir
                comando.CommandText = @"
                INSERT INTO Player (id_player, name, age, grade) 
                VALUES (@id_player, @name, @age, @grade)";
            }

            comando.Parameters.Add(new SqliteParameter("@id_player", id_player));
            comando.Parameters.Add(new SqliteParameter("@name", nome));
            comando.Parameters.Add(new SqliteParameter("@age", idade));
            comando.Parameters.Add(new SqliteParameter("@grade", grade));
            comando.ExecuteNonQuery();
        }

        int[] inventarioVazio = new int[9]; // Inventário inicial vazio
        SetPlayer_Inventory_itens(id_player, inventarioVazio);

        BancoDados.Close();
    }


    public string GetPlayer_name(int id_player)
    {
        return GetValue<string>("name", id_player);
    }

    public int GetPlayer_age(int id_player)
    {
        return GetValue<int>("age", id_player);
    }

    public string GetPlayer_grade(int id_player)
    {
        return GetValue<string>("grade", id_player);
    }

    public bool GetIsItemDelivered(int id_Player, int id_item_to_delivered)
    {
        BancoDados = criarEAbrirBancoDeDados();
        using (IDbCommand comando = BancoDados.CreateCommand())
        {
            comando.CommandText = "SELECT itens_delivered_list FROM Player WHERE id_player = @id_Player;";
            comando.Parameters.Add(new SqliteParameter("@id_Player", id_Player));

            using (IDataReader reader = comando.ExecuteReader())
            {
                if (reader.Read() && !reader.IsDBNull(0))
                {
                    byte[] blob = (byte[])reader["itens_delivered_list"];
                    List<int> itensEntregues = new List<int>();

                    for (int i = 0; i < blob.Length; i += 4) // Cada int ocupa 4 bytes
                    {
                        itensEntregues.Add(BitConverter.ToInt32(blob, i));
                    }

                    reader.Close();
                    BancoDados.Close();

                    // Verifica se o item já foi entregue
                    return itensEntregues.Contains(id_item_to_delivered);
                }
            }
        }
        BancoDados.Close();
        return false; // Retorna falso caso não haja itens entregues
    }

    public void SetItemDelivered(int id_Player, int id_item_to_delivered)
    {
        BancoDados = criarEAbrirBancoDeDados();
        List<int> itensEntregues = new List<int>();

        // Obtém os itens já entregues
        using (IDbCommand comando = BancoDados.CreateCommand())
        {
            comando.CommandText = "SELECT itens_delivered_list FROM Player WHERE id_player = @id_Player;";
            comando.Parameters.Add(new SqliteParameter("@id_Player", id_Player));

            using (IDataReader reader = comando.ExecuteReader())
            {
                if (reader.Read() && !reader.IsDBNull(0))
                {
                    byte[] blob = (byte[])reader["itens_delivered_list"];
                    for (int i = 0; i < blob.Length; i += 4)
                    {
                        itensEntregues.Add(BitConverter.ToInt32(blob, i));
                    }
                }
            }
        }

        // Adiciona o novo item à lista, se ainda não estiver presente
        if (!itensEntregues.Contains(id_item_to_delivered))
        {
            itensEntregues.Add(id_item_to_delivered);
        }

        // Converte a lista para um array de bytes
        List<byte> blobAtualizado = new List<byte>();
        foreach (int item in itensEntregues)
        {
            blobAtualizado.AddRange(BitConverter.GetBytes(item));
        }

        // Atualiza a tabela no banco de dados
        using (IDbCommand comando = BancoDados.CreateCommand())
        {
            comando.CommandText = @"
        UPDATE Player
        SET itens_delivered_list = @ItensDelivered
        WHERE id_player = @id_Player;";

            comando.Parameters.Add(new SqliteParameter("@id_Player", id_Player));
            comando.Parameters.Add(new SqliteParameter("@ItensDelivered", blobAtualizado.ToArray()));

            comando.ExecuteNonQuery();
        }
        BancoDados.Close();
    }

    public void SetPlayer_Inventory_itens(int id_player, int[] inventario)
    {
        BancoDados = criarEAbrirBancoDeDados();
        using (IDbCommand comando = BancoDados.CreateCommand())
        {
            // Converte o array de inteiros para um array de bytes
            List<byte> blob = new List<byte>();
            foreach (int numero in inventario)
            {
                blob.AddRange(BitConverter.GetBytes(numero));
            }

            comando.CommandText = @"
            UPDATE Player
            SET inventory_items = @Inventory
            WHERE id_player = @id_player;";

            comando.Parameters.Add(new SqliteParameter("@id_player", id_player));
            comando.Parameters.Add(new SqliteParameter("@Inventory", blob.ToArray())); // Passa o array de bytes como parâmetro
            comando.ExecuteNonQuery();
        }
        BancoDados.Close();
    }

    public int[] GetPlayer_Inventory_itens(int id_player)
    {
        BancoDados = criarEAbrirBancoDeDados();
        using (IDbCommand comando = BancoDados.CreateCommand())
        {
            comando.CommandText = "SELECT inventory_items FROM Player WHERE id_player = @id_player;";
            comando.Parameters.Add(new SqliteParameter("@id_player", id_player));

            using (IDataReader reader = comando.ExecuteReader())
            {
                if (reader.Read() && !reader.IsDBNull(0))
                {
                    byte[] blob = (byte[])reader["inventory_items"];
                    List<int> inventario = new List<int>();

                    for (int i = 0; i < blob.Length; i += 4) // Cada int ocupa 4 bytes
                    {
                        inventario.Add(BitConverter.ToInt32(blob, i));
                    }

                    reader.Close();
                    BancoDados.Close();
                    return inventario.ToArray();
                }
            }
        }

        BancoDados.Close();
        return new int[9]; // Retorna um inventário vazio caso não encontre dados
    }

    private T GetValue<T>(string columnName, int id_player)
    {
        BancoDados = criarEAbrirBancoDeDados();
        using (IDbCommand comando = BancoDados.CreateCommand())
        {
            comando.CommandText = $"SELECT {columnName} FROM Player WHERE id_player = @id_player;";
            comando.Parameters.Add(new SqliteParameter("@id_player", id_player));

            object result = comando.ExecuteScalar();
            BancoDados.Close();

            if (result != null && result != DBNull.Value)
            {
                return (T)Convert.ChangeType(result, typeof(T), CultureInfo.InvariantCulture);
            }
            return default;
        }
    }

    public IDataReader ReadPlayer(int id)
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand ComandoLer = BancoDados.CreateCommand();
        ComandoLer.CommandText = $"SELECT * FROM Player WHERE id_Player = {id};";
        return ComandoLer.ExecuteReader();
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
    public void DeletePlayerById(int playerId)
    {
        BancoDados = criarEAbrirBancoDeDados();
        using (IDbCommand comando = BancoDados.CreateCommand())
        {
            comando.CommandText = "DELETE FROM Player WHERE id_player = @playerId;";
            comando.Parameters.Add(new SqliteParameter("@playerId", playerId));

            int rowsAffected = comando.ExecuteNonQuery(); // Retorna o número de linhas afetadas
            Debug.Log($"{rowsAffected} jogador(es) deletado(s) com o ID {playerId}.");
        }
        BancoDados.Close();
    }




}
