using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemSpawnCache : MonoBehaviour
{
    public int itemSpawnId;
    public int playerId = 1;
    [SerializeField] GameObject CanvasItemRecebido;

    void Awake()
    {
        //Debug.Log("Awake");
        DontDestroyOnLoad(gameObject); // Marca o objeto para persistir entre cenas
        SceneManager.sceneLoaded += OnSceneLoaded; // Adiciona o evento de carregamento de cena
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Remove o evento ao destruir o objeto
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Verifica se a cena carregada é "Main"
        if (scene.name == "Main")
        {
            //Debug.Log("Cena 'Main' carregada. Executando código.");

            // Configura a câmera do Canvas
            SetupCanvasCamera();

            // Realiza o spawn do item
            SpawnItem();
        }
        else
        {
            //Debug.Log($"Cena carregada: {scene.name}. Nenhuma ação realizada.");
        }
    }

    void SetupCanvasCamera()
    {
        //Debug.Log("Configurando câmera do Canvas...");
        if (CanvasItemRecebido != null)
        {
            Canvas canvas = CanvasItemRecebido.GetComponent<Canvas>();
            if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                Camera mainCamera = Camera.main; // Obtém a câmera principal da cena
                if (mainCamera != null)
                {
                    canvas.worldCamera = mainCamera; // Atribui a câmera ao Canvas
                    Debug.Log("Câmera atribuída ao Canvas com sucesso.");
                }
                else
                {
                    Debug.LogWarning("Nenhuma câmera principal encontrada na cena!");
                }
            }
        }
        else
        {
            Debug.LogWarning("CanvasItemRecebido não está configurado.");
        }
    }

    void SpawnItem()
    {
        if (itemSpawnId == 0)
        {
            Debug.LogWarning("Nenhum item para spawnar (itemSpawnId é 0).");
            Destroy(gameObject);
            return;
        }

        //Debug.Log($"Iniciando o spawn do item com ID {itemSpawnId}.");

        // Supondo que BancoDeDados tenha uma instância ou método estático para acesso
        BancoDeDados bancoDeDados = new BancoDeDados();
        int[] InventarioPlayer = bancoDeDados.LerInventario(playerId);

        bool itemAdicionado = false;

        for (int i = 0; i < InventarioPlayer.Length; i++)
        {
            if (InventarioPlayer[i] == 0) // Verifica o primeiro espaço vazio
            {
                InventarioPlayer[i] = itemSpawnId;
                itemAdicionado = true;
                bancoDeDados.SalvarInventario(playerId, InventarioPlayer);
                PanelItemRecebido();
                //Debug.Log($"Item com ID {itemSpawnId} adicionado ao slot {i}.");
                break; // Sai do loop após adicionar o item
            }
        }

        if (itemAdicionado)
        {
            itemSpawnId = 0; // Reseta o ID do item após adicioná-lo
            
        }
        else
        {
            
            Debug.LogWarning("Inventário cheio! Não foi possível adicionar o item.");
            Destroy(gameObject); // Destrói o objeto após adicionar o item
        }
    }

    public void PanelItemRecebido()
    {
        //Debug.Log("Exibindo painel de item recebido...");

        // Ativa o Canvas e exibe por 5 segundos
        CanvasItemRecebido.SetActive(true);
        StartCoroutine(DisablePanelAfterDelay(2)); // Aguarda 5 segundos para desativar
    }

    IEnumerator DisablePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject); // Destrói o objeto após adicionar o item
        CanvasItemRecebido.SetActive(false); // Desativa o Canvas
        //Debug.Log("Painel de item recebido ocultado.");
    }
}
