using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirMissoes : MonoBehaviour
{
    [SerializeField] GameObject gameObjectPanel;
    // Start is called before the first frame update

    private void Start()
    {
        gameObjectPanel.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObjectPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObjectPanel.SetActive(false);
        }
    }
}
