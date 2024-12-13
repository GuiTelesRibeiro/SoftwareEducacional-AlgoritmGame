using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] private Transform player; // Refer�ncia ao jogador (Transform)
    [SerializeField] private float smoothSpeed = 0.125f; // Velocidade de suaviza��o
    [SerializeField] private Vector2 offset = new Vector2(0, 0); // Offset em X e Y

    private void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogWarning("Jogador n�o atribu�do � c�mera!");
            return;
        }

        // Calcula a posi��o desejada no plano 2D
        Vector2 desiredPosition = new Vector2(player.position.x + offset.x, player.position.y + offset.y);

        // Interpola suavemente entre a posi��o atual e a desejada
        Vector2 smoothedPosition = Vector2.Lerp(new Vector2(transform.position.x, transform.position.y), desiredPosition, smoothSpeed);

        // Atualiza a posi��o da c�mera
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
