using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonResponse : MonoBehaviour
{
    [SerializeField] GameObject button;
    // Start is called before the first frame update
    private void Start()
    {
        button.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("NPC") || collision.CompareTag("Mission") || collision.CompareTag("MP"))
        {
            button.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        button.SetActive(false);

    }
}
