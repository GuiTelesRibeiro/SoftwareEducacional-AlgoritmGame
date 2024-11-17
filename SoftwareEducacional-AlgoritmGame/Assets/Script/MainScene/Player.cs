using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 move;
    [SerializeField] private float moveSpeed = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetMoviment(InputAction.CallbackContext value)
    {
        move = value.ReadValue<Vector2>();
    }
    public void SetInteraction(InputAction.CallbackContext value)
    {

    }

    private void FixedUpdate()
    {
        rb.velocity = move.normalized * moveSpeed;
    }
}
