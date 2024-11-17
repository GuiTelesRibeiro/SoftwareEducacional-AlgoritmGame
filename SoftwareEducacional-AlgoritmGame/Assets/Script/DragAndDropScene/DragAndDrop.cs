using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private RectTransform rectTransform; // Para UI
    private Canvas canvas; // Refer�ncia ao Canvas (caso seja um elemento UI)
    private CanvasGroup canvasGroup;
    

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>(); // Para UI (voc� tamb�m pode usar Transform para objetos 3D)
        canvas = GetComponentInParent<Canvas>(); // A refer�ncia ao canvas � necess�ria para UI
        canvasGroup = GetComponentInParent<CanvasGroup>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;

        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");

        // Para mover o objeto, vamos atualizar a posi��o da RectTransform com base na posi��o do ponteiro.
        if (rectTransform != null && canvas != null)
        {
            // Converte as coordenadas do mouse para o espa�o da tela levando em conta o canvas
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out pos);
            rectTransform.anchoredPosition = pos; // Atualiza a posi��o do objeto
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();

    }
}
