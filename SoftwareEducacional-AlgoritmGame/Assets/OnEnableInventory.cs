using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableInventory : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        if(Inventory.Singleton != null)
        {
            Debug.Log("Atualizou");
            Inventory.Singleton.UpdateInterface(Inventory.Singleton.GetInventoryBanco(1));
        }
        
    }
}
