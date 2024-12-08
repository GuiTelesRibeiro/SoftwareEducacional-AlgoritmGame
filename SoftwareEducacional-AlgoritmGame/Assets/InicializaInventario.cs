using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InicializaInventario : MonoBehaviour
{
    [SerializeField] GameObject inventory; 
    void Awake()
    {
        inventory.SetActive(true);
    }
    void Start()
    {
        inventory.SetActive(false);
    }

}
