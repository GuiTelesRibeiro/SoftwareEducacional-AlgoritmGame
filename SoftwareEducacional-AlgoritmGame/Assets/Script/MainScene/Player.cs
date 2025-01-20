using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 move;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private PlayerAnimController animController; // Refer�ncia ao controlador de anima��o
    [SerializeField] Transform spawnAlternativo;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        // Procura pelo objeto "ItemSpawnCache"
        GameObject obj = GameObject.Find("ItemSpawnCache");
        if (obj != null)
        {
            // Caso o objeto seja encontrado, define a posi��o inicial do player para a posi��o do spawn alternativo
            transform.position = spawnAlternativo.position;
        }
    }


    public void SetMoviment(InputAction.CallbackContext value)
    {
        move = value.ReadValue<Vector2>();
        //Debug.Log("Input recebido no Player: " + move);

        // Enviar movimento ao controlador de anima��o
        if (animController != null)
        {
            animController.UpdateMove(move);
        }
    }
    public void SetInterectio()
    {
        return;
    }

    private void FixedUpdate()
    {
        // Atualizar f�sica do movimento
        rb.linearVelocity = move.normalized * moveSpeed;
    }
}
