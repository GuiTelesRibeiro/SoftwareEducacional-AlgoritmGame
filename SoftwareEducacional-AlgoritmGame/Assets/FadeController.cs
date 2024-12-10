using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    [SerializeField] private Image fadeScreen; // Imagem preta para o fade
    [SerializeField] private float fadeDuration = 2f; // Duração do fade em segundos


    void Start()
    {
        // Garante que a imagem começa totalmente opaca
        if (fadeScreen != null)
        {
            fadeScreen.color = new Color(0, 0, 0, 1); // Cor preta com opacidade máxima
            StartCoroutine(FadeIn());
        }
        else
        {
            Debug.LogWarning("FadeScreen não atribuído no inspector!");
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - (elapsedTime / fadeDuration);
            fadeScreen.color = new Color(0, 0, 0, alpha); // Gradualmente reduz a opacidade
            yield return null; // Aguarda o próximo frame
        }

        // Garante que a imagem esteja completamente transparente no final
        fadeScreen.color = new Color(0, 0, 0, 0);

        // Destrói ou desativa o objeto
        fadeScreen.gameObject.SetActive(false);
        // Se deseja destruir
        // fadeScreen.gameObject.SetActive(false); // Se deseja apenas desativar
    }
}
