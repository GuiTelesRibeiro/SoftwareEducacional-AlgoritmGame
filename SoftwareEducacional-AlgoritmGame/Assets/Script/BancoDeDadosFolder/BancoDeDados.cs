using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;

public class BancoDeDados
{
    private DBPlayer dbPlayer;
    private DBLevel dbLevel;
    private DBAttempt dbAttempt;

    public BancoDeDados()
    {
        dbPlayer = new DBPlayer();
        dbLevel = new DBLevel();
        dbAttempt = new DBAttempt();
    }

    // M�todos relacionados a Player
    public void SetOrUpdatePlayerData(int id_player, string nome, int idade, int grade)
    {
        dbPlayer.SetOrUpdatePlayerData(id_player, nome, idade, grade);
    }

    public IDataReader ReadPlayer(int id_player)
    {
        return dbPlayer.ReadPlayer(id_player);
    }

    public string GetPlayerName(int id_player)
    {
        return dbPlayer.GetPlayer_name(id_player);
    }

    public int GetPlayerAge(int id_player)
    {
        return dbPlayer.GetPlayer_age(id_player);
    }

    public string GetPlayerGrade(int id_player)
    {
        return dbPlayer.GetPlayer_grade(id_player);
    }

    public void SetPlayerInventory(int id_player, int[] inventario)
    {
        dbPlayer.SetPlayer_Inventory_itens(id_player, inventario);
    }

    public int[] GetPlayerInventory(int id_player)
    {
        return dbPlayer.GetPlayer_Inventory_itens(id_player);
    }

    public void DeletePlayerById(int playerId)
    {
        dbPlayer.DeletePlayerById(playerId);
    }

    public void SetItemDelivered(int id_Player, int id_item_to_delivered)
    {
        dbPlayer.SetItemDelivered(id_Player, id_item_to_delivered);
    }

    public bool GetIsItemDelivered(int id_player, int id_item_to_delivered)
    {
        return dbPlayer.GetIsItemDelivered(id_player, id_item_to_delivered);
    }



    // M�todos relacionados a Level
    public void SetLevelData(int id_level, int id_item_to_recive, string level_description)
    {
        dbLevel.SetLevelData(id_level, id_item_to_recive, level_description);
    }

    public Dictionary<string, object> GetLevelById(int id_level)
    {
        return dbLevel.GetLevelById(id_level);
    }
    public int GetLevelItemToReceive(int id_level)
    {
        return dbLevel.GetLevel_Id_Item_To_Recive(id_level);
    }

    public string GetLevelDescription(int id_level)
    {
        return dbLevel.GetLevel_Level_Description(id_level);
    }
    public int GetHighestSuccessfulLevelId(int playerId)
    {
        return dbAttempt.GetHighestSuccessfulLevelId(playerId);
    }



    // M�todos relacionados a Attempt

    public bool IsFirstComplete(int idPlayer, int idLevel)
    {
        return dbAttempt.IsFirstComplete(idPlayer, idLevel);
    }
    public int SetAttemptData(int id_player, int id_level, int number_of_commands, int seconds_to_solve, bool is_failed_attempts)
    {
        return dbAttempt.SetAttemptData(id_player,  id_level,  number_of_commands,  seconds_to_solve,  is_failed_attempts);
    }

    public int GetAttemptPlayerId(int id_attempt )
    {
        return dbAttempt.GetAttempt_Id_Player(id_attempt );
    }

    public int GetAttemptMissionId(int id_attempt )
    {
        return dbAttempt.GetAttempt_Id_Missao(id_attempt );
    }

    public int GetAttemptNumberOfCommands(int id_attempt )
    {
        return dbAttempt.GetAttempt_Number_Of_Commands(id_attempt );
    }

    public int GetAttemptSecondsToSolve(int id_attempt) { 
        return dbAttempt.GetAttempt_Seconds_To_Solve(id_attempt );
    }

    public bool GetAttemptIsFailed(int id_attempt )
    {
        return dbAttempt.GetAttempt_Is_Failed_Attempt(id_attempt );
    }

    public DateTime? GetAttemptTimeData(int id_attempt )
    {
        return dbAttempt.GetAttempt_Time_Data(id_attempt );
    }

    public void DeleteAttemptByPlayer(int playerId )
    {
        dbAttempt.DeleteAttemptByPlayer(playerId );
    }

   
    // deixarei pronto para poss�vel uso
    public void InitializeDatabase()
    {
        dbPlayer = new DBPlayer();
        dbLevel = new DBLevel();
        dbAttempt = new DBAttempt();
    }
    public void CloseDatabase()
    {
        dbPlayer.CloseDatabase();
        dbLevel.CloseDatabase();
        dbAttempt.CloseDatabase();
    }
}
