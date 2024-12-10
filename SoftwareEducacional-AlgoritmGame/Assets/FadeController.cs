using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    [SerializeField] private Image fadeScreen; // Imagem preta para o fade
    [SerializeField] private float fadeDuration = 2f; // Dura��o do fade em segundos


    void Start()
    {
        // Garante que a imagem come�a totalmente opaca
        if (fadeScreen != null)
        {
            fadeScreen.color = new Color(0, 0, 0, 1); // Cor preta com opacidade m�xima
            StartCoroutine(FadeIn());
        }
        else
        {
            Debug.LogWarning("FadeScreen n�o atribu�do no inspector!");
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
            yield return null; // Aguarda o pr�ximo frame
        }

        // Garante que a imagem esteja completamente transparente no final
        fadeScreen.color = new Color(0, 0, 0, 0);

        // Destr�i ou desativa o objeto
        fadeScreen.gameObject.SetActive(false);
        // Se deseja destruir
        // fadeScreen.gameObject.SetActive(false); // Se deseja apenas desativar
    }
}
