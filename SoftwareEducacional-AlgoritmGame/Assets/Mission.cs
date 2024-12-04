using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mission : MonoBehaviour
{

    [SerializeField] string missionName;
    [SerializeField] Item Item;
    [SerializeField] GameObject concluidoPanel;
    [SerializeField] GameObject pendentePanel;
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text missionNameTMP;
    bool isMissionCompleted;

    private void Awake()
    {
        isMissionCompleted = MissionState();
        missionNameTMP.text = missionName;
        itemImage.sprite = Item.sprite;
        GameObjectsOff();
    }
    public void Interaction()
    {
        int[] itensDoInventario = Inventory.Singleton.getItemIds;
        if (itensDoInventario == null)
        {
            Debug.LogError("Itens do Inventário is null!");
            return;
        }

        if (isMissionCompleted == false && HaveItemInInventory(itensDoInventario))
            isMissionCompleted = true;

        GameObjectsOn();
    }


    bool MissionState()
    {
        return false;
    }

    bool HaveItemInInventory (int[] itensDoInventario)
    {
        for (int i = 0; i< itensDoInventario.Length; i++)
        {
            if (itensDoInventario[i] == Item.itemID)
            {
                Inventory.Singleton.DeleteItemById(Item.itemID);
                return true;
            }
        }
        return false;
    }
    void GameObjectsOn()
    {

        concluidoPanel.SetActive(isMissionCompleted);
        pendentePanel.SetActive(!isMissionCompleted);
    }
    void GameObjectsOff()
    {

        concluidoPanel.SetActive(false);
        pendentePanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObjectsOn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObjectsOff();
        }
    }
}
