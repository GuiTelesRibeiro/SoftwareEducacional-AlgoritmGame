using UnityEngine;
using UnityEngine.UI;

public class SlotActionsButton : MonoBehaviour
{
    [SerializeField] private Sprite defaultSprite; // Sprite padr�o
    private Sprite actionSprite; // Sprite associado � a��o
    private Image buttonImage; // Componente Image do bot�o
    public string action; // Nome da a��o associada ao slot

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        if (defaultSprite == null)
        {
            Debug.LogWarning("O sprite padr�o n�o foi atribu�do!");
        }
        buttonImage.sprite = defaultSprite;
    }

    private void SetSprite()
    {
        // Atualiza o sprite do bot�o com base na a��o
        buttonImage.sprite = string.IsNullOrEmpty(action) ? defaultSprite : actionSprite;
    }

    public void SetAction(string newAction, Sprite externalSprite)
    {
        action = newAction;
        actionSprite = externalSprite;
        SetSprite();
    }

    public void ClearActions()
    {
        // Limpa a a��o e retorna ao sprite padr�o
        action = string.Empty;
        actionSprite = null;
        SetSprite();
    }
}
