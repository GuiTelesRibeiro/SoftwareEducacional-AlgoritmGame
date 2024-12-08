using UnityEngine;

public class NaoDestruir : MonoBehaviour
{
    void Awake()
    {
        // Garante que este objeto não será destruído ao trocar de cena
        DontDestroyOnLoad(gameObject);
    }
}
