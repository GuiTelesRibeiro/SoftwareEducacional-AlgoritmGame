using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public Transform movePoint;
    private Vector3 startPosition;
    public LayerMask Obstacle;
    public LayerMask Objective; // LayerMask para identificar o objetivo
    public PuzzleController puzzleController;
    public float actionDelay = 0.5f;

    private Queue<string> actionQueue = new Queue<string>();
    private bool isExecutingActions = false;
    private Coroutine currentExecution;
    private bool objectiveReached = false;

    void Start()
    {
        movePoint.parent = null;
        // Alinha a posição inicial de movePoint ao grid
        movePoint.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), transform.position.z);

        startPosition = transform.position;
    }

    void Update()
    {
        // Move o personagem em direção ao movePoint até alcançar o ponto
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        // Bloqueia novos movimentos até que o personagem chegue ao ponto
        if (Vector3.Distance(transform.position, movePoint.position) > 0.05f)
            return;
    }

    public void StartExecutingActions()
    {
        actionQueue.Clear();

        foreach (var action in puzzleController.listAction)
        {
            if (!string.IsNullOrEmpty(action))
            {
                actionQueue.Enqueue(action);
            }
        }

        if (!isExecutingActions)
        {
            puzzleController.ToggleButtons(false); // Desativa os botões
            currentExecution = StartCoroutine(ExecuteActions());
        }
    }

    private IEnumerator ExecuteActions()
    {
        isExecutingActions = true;

        while (actionQueue.Count > 0)
        {
            string action = actionQueue.Dequeue();
            ExecuteAction(action);
            yield return new WaitForSeconds(actionDelay);

            // Verifica se o objetivo foi alcançado durante a execução
            if (objectiveReached)
            {
                StopExecutingActions();
                Debug.Log("Objetivo alcançado!");
                yield break;
            }
        }

        // Se a fila acabar e o objetivo não foi alcançado
        if (!objectiveReached)
        {
            transform.position = startPosition;
            movePoint.position = startPosition;
            Debug.Log("Perdeu");
        }

        isExecutingActions = false;
        puzzleController.ToggleButtons(true); // Reativa os botões
    }

    private void ExecuteAction(string action)
    {
        if (action == "Up")
        {
            Move(Vector3Int.up); // Move 1 unidade inteira para cima
        }
        else if (action == "Down")
        {
            Move(Vector3Int.down); // Move 1 unidade inteira para baixo
        }
        else if (action == "Left")
        {
            Move(Vector3Int.left); // Move 1 unidade inteira para a esquerda
        }
        else if (action == "Right")
        {
            Move(Vector3Int.right); // Move 1 unidade inteira para a direita
        }
    }

    private void Move(Vector3Int direction)
    {
        // Verifica se o jogador está no ponto antes de atualizar
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            // Calcula a nova posição alvo usando a direção
            Vector3 targetPosition = movePoint.position + (Vector3)direction;

            // Verifica se o novo ponto está livre de obstáculos
            if (!Physics2D.OverlapCircle(targetPosition, 0.2f, Obstacle))
            {
                movePoint.position = targetPosition; // Atualiza o ponto de movimento
            }
            else
            {
                Debug.Log("Movimento bloqueado por obstáculo!"); // Log para depuração
            }
        }
    }

    public void StopExecutingActions()
    {
        if (currentExecution != null)
        {
            StopCoroutine(currentExecution); // Interrompe a execução
            currentExecution = null;
        }

        isExecutingActions = false;
        puzzleController.ToggleButtons(true); // Reativa os botões
        actionQueue.Clear(); // Limpa a fila de ações
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto colidido está na camada definida como Objective
        if ((Objective.value & (1 << collision.gameObject.layer)) != 0)
        {
            objectiveReached = true;
            StopExecutingActions();
            Debug.Log("Objetivo alcançado por colisão!");
        }
    }
}
