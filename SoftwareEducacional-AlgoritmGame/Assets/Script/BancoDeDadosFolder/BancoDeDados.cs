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

    // Métodos relacionados a Player
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



    // Métodos relacionados a Level
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



    // Métodos relacionados a Attempt
    public void SetAttemptData(int id_player, int id_missao, bool is_first_attempt, int number_of_commands, int time_to_recive, bool is_failed_attempts, DateTime time_data)
    {
        dbAttempt.SetAttemptData(id_player,  id_missao,  is_first_attempt,  number_of_commands,  time_to_recive,  is_failed_attempts, time_data);
    }

    public int GetAttemptPlayerId(int id_attempt )
    {
        return dbAttempt.GetAttempt_Id_Player(id_attempt );
    }

    public int GetAttemptMissionId(int id_attempt )
    {
        return dbAttempt.GetAttempt_Id_Missao(id_attempt );
    }

    public bool GetAttemptIsFirst(int id_attempt )
    {
        return dbAttempt.GetAttempt_Is_First_Attempt(id_attempt );
    }

    public int GetAttemptNumberOfCommands(int id_attempt )
    {
        return dbAttempt.GetAttempt_Number_Of_Commands(id_attempt );
    }

    public int GetAttemptTimeToReceive(int id_attempt )
    {
        return dbAttempt.GetAttempt_Time_To_Recive(id_attempt );
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

   
    // deixarei pronto para possível uso
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

    //----------------------------------------------------
    //funcoes para resolver

    public int GetNumber_Attempts( int IdPlayer, int IdMissao)
    {
        return 1;
    }

    public void SetNumber_Attempts( int IdPlayer, int IdMissao, int tempTentativas)
    {

    }

    public int GetIsMissionComplete( int IdPlayer, int IdMissao)
    {
        return 1;
    }

    public int GetMove_To_Complete(int IdPlayer, int IdMissao)
    {
        return 1;
    }

    public void  SetMove_To_Complete( int IdPlayer, int IdMissao, int Move_To_Complete)
    {

    }

    public int GetMissaoIdItem( int IdMissao) // ver o que faz 
    {
        return 1;
    }
    public void SetMissaoIdItem( int IdMissao) // ver  o que faz 
    {

    }

    public void SetIsMissionComplete (int aa, int bb, int cc)// ver o que faz
    {

    }

    public bool VerificarPlayerMissaoExiste(int IdPlayer, int IdMissao) // alterar funcionamento 
    {
        return true;
    }

    public void CriarPlayerMissao(int IdPlayer, int IdMissao)// Attenmpt 
    {

    }
}
