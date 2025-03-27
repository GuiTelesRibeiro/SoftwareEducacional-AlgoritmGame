using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectController : MonoBehaviour
{
    [SerializeField] private Button[] botoes;
    private Color corPadrao = new Color(0xFC / 255f, 0x8F / 255f, 0x54 / 255f); // FC8F54
    private Color corSelecionado = new Color(0xFF / 255f, 0x73 / 255f, 0x7B / 255f); // FF737B

    private void Start()
    {
        foreach (Button botao in botoes)
        {
            botao.onClick.AddListener(() => SelectButton(botao));
        }
    }

    public void ResetButtons()
    {
        foreach (Button botao in botoes)
        {
            botao.image.color = corPadrao;
        }
    }

    public void SelectButton(Button botaoSelecionado)
    {
        ResetButtons();
        botaoSelecionado.image.color = corSelecionado;
    }
}
