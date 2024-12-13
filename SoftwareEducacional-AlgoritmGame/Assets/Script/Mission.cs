using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mission : MonoBehaviour
{
    [SerializeField] string missionName;
    [SerializeField] Item Item;
    [SerializeField] GameObject concluidoPanel;
    [SerializeField] GameObject pendentePanel;
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text missionNameTMP;
    [SerializeField] int idPlayer = 1;
    [SerializeField] int idMission;
    public bool isMissionCompleted;
    [SerializeField] MissoesController missoesController;
    [SerializeField] AudioSource Sucess;

    private void Awake()
    {
        isMissionCompleted = MissionState();
        missionNameTMP.text = missionName;
        itemImage.sprite = Item.sprite;
        GameObjectsOff();
    }

    public void Interaction()
    {
        BancoDeDados bancoDeDados = new BancoDeDados();
        int[] itensDoInventario = bancoDeDados.LerInventario(idPlayer);

        if (itensDoInventario == null)
        {
            Debug.LogError("Itens do Inventário is null!");
            return;
        }

        if (isMissionCompleted == false && HaveItemInInventory(itensDoInventario))
        {
            Sucess.Play();
            isMissionCompleted = true;
            missoesController.FimDeJogo();
        }
        else if (!HaveItemInInventory(itensDoInventario))
        {
            StartCoroutine(FlashPendentePanel());
        }

        GameObjectsOn();
    }

    bool MissionState()
    {
        BancoDeDados bancoDeDados = new BancoDeDados();
        if (bancoDeDados.GetIsItemDelivered(idPlayer, idMission) == 1)
        {
            return true;
        }

        return false;
    }

    bool HaveItemInInventory(int[] itensDoInventario)
    {
        for (int i = 0; i < itensDoInventario.Length; i++)
        {
            if (itensDoInventario[i] == Item.itemID)
            {
                itensDoInventario[i] = 0;
                BancoDeDados bancoDeDados = new BancoDeDados();
                Debug.Log($" interaction2 : {itensDoInventario[0]},{itensDoInventario[1]},{itensDoInventario[2]},{itensDoInventario[3]},{itensDoInventario[4]},{itensDoInventario[5]},{itensDoInventario[6]},{itensDoInventario[7]},{itensDoInventario[8]}");

                bancoDeDados.SalvarInventario(idPlayer, itensDoInventario);
                bancoDeDados.SetIsItemDelivered(idPlayer, idMission, 1);

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

    private IEnumerator FlashPendentePanel()
    {
        // Save the original color

        // Change to red
        pendentePanel.GetComponent<Image>().color = Color.red;

        // Wait for 1 second
        yield return new WaitForSeconds(.5f);

        // Revert to the original color
        pendentePanel.GetComponent<Image>().color = Color.black;
    }
}
