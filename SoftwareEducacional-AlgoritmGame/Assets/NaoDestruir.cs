using UnityEngine;

public class NaoDestruir : MonoBehaviour
{
    void Awake()
    {
        // Garante que este objeto n�o ser� destru�do ao trocar de cena
        DontDestroyOnLoad(gameObject);
    }
}
