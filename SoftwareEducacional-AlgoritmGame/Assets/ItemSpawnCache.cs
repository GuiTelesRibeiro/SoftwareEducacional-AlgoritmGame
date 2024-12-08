using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemSpawnCache : MonoBehaviour
{
    public int itemSpawnId;

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Marca o objeto para persistir entre cenas
        SceneManager.sceneLoaded += OnSceneLoaded; // Adiciona o evento de carregamento de cena
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Remove o evento ao destruir o objeto
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Verifica se a cena carregada é "main"
        if (scene.name == "Main")
        {
            Debug.Log("Cena 'main' carregada. Executando código.");
            if (Inventory.Singleton != null)
            {
                SpawnItem(itemSpawnId);
            }
        }
        else
        {
            Debug.Log($"Cena carregada: {scene.name}. Nenhuma ação realizada.");
        }
    }

    void SpawnItem(int itemSpawnId)
    {
        if (itemSpawnId != 0)
        {
            Inventory.Singleton.SpawnInventoryItemById(itemSpawnId);
            this.itemSpawnId = 0;
        }
        Destroy(gameObject); // Destrói o objeto após spawnar o item
    }
}
