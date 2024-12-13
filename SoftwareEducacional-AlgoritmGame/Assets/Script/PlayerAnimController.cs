using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    private Animator _animator;
    private Vector2 move;
    private string lastDirection = "Down"; // Dire��o padr�o inicial
    [SerializeField] AudioSource Walk;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void UpdateMove(Vector2 movement)
    {
        move = movement;
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (move.magnitude > 0.1f) // Se houver movimento
        {
            Walk.mute = false;
            if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
            {
                // Movimenta��o lateral
                _animator.Play("CaracterPlayer_RunSide");
                FlipSprite(move.x);
                lastDirection = "Side"; // Atualiza a dire��o
            }
            else if (move.y > 0)
            {
                // Movimenta��o para cima
                _animator.Play("CaracterPlayer_RunUp");
                lastDirection = "Up"; // Atualiza a dire��o
            }
            else
            {
                // Movimenta��o para baixo
                _animator.Play("CaracterPlayer_RunDown");
                lastDirection = "Down"; // Atualiza a dire��o
            }
        }
        else
        {
            Walk.mute = true;
            // Idle Animation com base na �ltima dire��o
            PlayIdleAnimation();
        }
    }

    private void PlayIdleAnimation()
    {
        switch (lastDirection)
        {
            case "Side":
                _animator.Play("CaracterPlayer_IdleSide");
                break;
            case "Up":
                _animator.Play("CaracterPlayer_IdleUp");
                break;
            case "Down":
                _animator.Play("CaracterPlayer_IdleDown");
                break;
        }
    }

    private void FlipSprite(float horizontalInput)
    {
        Vector3 scale = transform.localScale;
        scale.x = horizontalInput > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}
