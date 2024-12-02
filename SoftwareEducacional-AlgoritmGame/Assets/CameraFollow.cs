using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] private Transform player; // Referência ao jogador (Transform)
    [SerializeField] private float smoothSpeed = 0.125f; // Velocidade de suavização
    [SerializeField] private Vector2 offset = new Vector2(0, 0); // Offset em X e Y

    private void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogWarning("Jogador não atribuído à câmera!");
            return;
        }

        // Calcula a posição desejada no plano 2D
        Vector2 desiredPosition = new Vector2(player.position.x + offset.x, player.position.y + offset.y);

        // Interpola suavemente entre a posição atual e a desejada
        Vector2 smoothedPosition = Vector2.Lerp(new Vector2(transform.position.x, transform.position.y), desiredPosition, smoothSpeed);

        // Atualiza a posição da câmera
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
