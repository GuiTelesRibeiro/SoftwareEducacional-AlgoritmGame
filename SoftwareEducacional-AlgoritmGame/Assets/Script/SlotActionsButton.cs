using UnityEngine;
using UnityEngine.UI;

public class SlotActionsButton : MonoBehaviour
{
    [SerializeField] private Sprite defaultSprite; // Sprite padrão
    private Sprite actionSprite; // Sprite associado à ação
    private Image buttonImage; // Componente Image do botão
    public string action; // Nome da ação associada ao slot

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        if (defaultSprite == null)
        {
            Debug.LogWarning("O sprite padrão não foi atribuído!");
        }
        buttonImage.sprite = defaultSprite;
    }

    private void SetSprite()
    {
        // Atualiza o sprite do botão com base na ação
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
        // Limpa a ação e retorna ao sprite padrão
        action = string.Empty;
        actionSprite = null;
        SetSprite();
    }
}
