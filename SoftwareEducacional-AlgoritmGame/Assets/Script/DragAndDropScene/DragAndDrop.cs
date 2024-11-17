using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private RectTransform rectTransform; // Para UI
    private Canvas canvas; // Referência ao Canvas (caso seja um elemento UI)
    private CanvasGroup canvasGroup;
    

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>(); // Para UI (você também pode usar Transform para objetos 3D)
        canvas = GetComponentInParent<Canvas>(); // A referência ao canvas é necessária para UI
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

        // Para mover o objeto, vamos atualizar a posição da RectTransform com base na posição do ponteiro.
        if (rectTransform != null && canvas != null)
        {
            // Converte as coordenadas do mouse para o espaço da tela levando em conta o canvas
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out pos);
            rectTransform.anchoredPosition = pos; // Atualiza a posição do objeto
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();

    }
}
