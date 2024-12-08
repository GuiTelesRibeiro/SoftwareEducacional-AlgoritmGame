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
        // Verifica se a cena carregada � "main"
        if (scene.name == "Main")
        {
            Debug.Log("Cena 'Main' carregada. Executando c�digo com atraso.");
            if (Inventory.Singleton != null)
            {
                StartCoroutine(SpawnItemWithDelay(itemSpawnId, 1.0f)); // Chama com 1 segundo de atraso
            }
        }
        else
        {
            Debug.Log($"Cena carregada: {scene.name}. Nenhuma a��o realizada.");
        }
    }

    IEnumerator SpawnItemWithDelay(int itemSpawnId, float delay)
    {
        yield return new WaitForSeconds(delay); // Espera pelo tempo definido
        SpawnItem(itemSpawnId);
    }

    void SpawnItem(int itemSpawnId)
    {
        if (itemSpawnId != 0)
        {
            Inventory.Singleton.SpawnInventoryItemById(itemSpawnId);
            this.itemSpawnId = 0;
        }
        Destroy(gameObject); // Destr�i o objeto ap�s spawnar o item
    }
}
