using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemSpawnCache : MonoBehaviour
{
    public int itemSpawnId;
    public int playerId = 1;

    void Awake()
    {
        Debug.Log("Awake");
        DontDestroyOnLoad(gameObject); // Marca o objeto para persistir entre cenas
        SceneManager.sceneLoaded += OnSceneLoaded; // Adiciona o evento de carregamento de cena
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Remove o evento ao destruir o objeto
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Verifica se a cena carregada � "main"
        if (scene.name == "Main")
        {
            Debug.Log("Cena 'main' carregada. Executando c�digo.");
            SpawnItem();
        }
        else
        {
            Debug.Log($"Cena carregada: {scene.name}. Nenhuma a��o realizada.");
        }
    }

    void SpawnItem()
    {
        if (itemSpawnId == 0)
        {
            Debug.LogWarning("Nenhum item para spawnar (itemSpawnId � 0).");
            return;
        }

        Debug.Log($"Iniciando o spawn do item com ID {itemSpawnId}.");

        // Supondo que BancoDeDados tenha uma inst�ncia ou m�todo est�tico para acesso
        BancoDeDados bancoDeDados = new BancoDeDados();
        int[] InventarioPlayer = bancoDeDados.LerInventario(playerId);

        bool itemAdicionado = false;

        for (int i = 0; i < InventarioPlayer.Length; i++)
        {
            if (InventarioPlayer[i] == 0) // Verifica o primeiro espa�o vazio
            {
                InventarioPlayer[i] = itemSpawnId;
                itemAdicionado = true;
                bancoDeDados.SalvarInventario(playerId, InventarioPlayer);
                PanelItemRecebido();
                Debug.Log($"Item com ID {itemSpawnId} adicionado ao slot {i}.");
                break; // Sai do loop ap�s adicionar o item
            }
        }

        if (itemAdicionado)
        {
            itemSpawnId = 0; // Reseta o ID do item ap�s adicion�-lo
            Destroy(gameObject); // Destr�i o objeto ap�s adicionar o item
        }
        else
        {
            Debug.LogWarning("Invent�rio cheio! N�o foi poss�vel adicionar o item.");
        }
    }
    public void PanelItemRecebido()
    {
        Debug.Log("PanelItemRecebido");
    }

}
