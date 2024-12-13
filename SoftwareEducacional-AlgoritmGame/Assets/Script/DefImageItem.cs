using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefImageItem : MonoBehaviour
{
    [SerializeField] private PuzzleCanvasController puzzleCanvasController; // Referência ao controlador
    private SpriteRenderer minhaSpriteRenderer; // Referência ao componente SpriteRenderer deste GameObject

    void Start()
    {
        // Tenta obter o componente SpriteRenderer deste GameObject
        minhaSpriteRenderer = GetComponent<SpriteRenderer>();

        if (minhaSpriteRenderer == null)
        {
            Debug.LogError("O componente SpriteRenderer não foi encontrado neste GameObject!");
            return;
        }

        if (puzzleCanvasController != null)
        {
            // Atribui a imagem do PuzzleCanvasController ao SpriteRenderer deste GameObject
            minhaSpriteRenderer.sprite = puzzleCanvasController.imageItem.sprite;
        }
        else
        {
            Debug.LogError("PuzzleCanvasController não foi atribuído no Inspector!");
        }
    }
}
