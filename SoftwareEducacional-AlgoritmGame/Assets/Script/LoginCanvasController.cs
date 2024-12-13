using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject cutScene;
    [SerializeField] private string nomeCenaJogo; // Nome da cena que será carregada
    [SerializeField] private float tempoCutScene = 40f; // Tempo da cutscene em segundos

    void Start()
    {
        OpenLoginPanel();
    }

    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        cutScene.SetActive(false);
    }

    public void CutScene()
    {
        loginPanel.SetActive(false);
        cutScene.SetActive(true);

        // Inicia uma coroutine para aguardar antes de carregar a próxima cena
        StartCoroutine(CarregarCenaAposCutScene());
    }

    private IEnumerator CarregarCenaAposCutScene()
    {
        // Aguarda o tempo da cutscene antes de carregar a próxima cena
        yield return new WaitForSeconds(tempoCutScene);

        // Carrega a cena principal
        SceneManager.LoadScene(nomeCenaJogo);
    }
}
