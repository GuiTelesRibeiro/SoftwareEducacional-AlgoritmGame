using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class MissionButton : MonoBehaviour
{
    [SerializeField] int MissionID;
    [SerializeField] int MaiorPlayerMissionCompleta;
    [SerializeField] GameObject BlockPanel;
    [SerializeField] Button Button;
    [SerializeField] MissaoController missaoController;
    [SerializeField] Image spriteItem;
    // Start is called before the first frame update
    private void Update()
    {
        UpdateState();
    }

    private void Start()
    {
        BancoDeDados bancoDeDados = new BancoDeDados();
        spriteItem.sprite = Inventory.Singleton.allItemsList[bancoDeDados.GetMissaoIdItem(MissionID)-1].sprite;
    }
    // Update is called once per frame
    void UpdateState()
    {
        MaiorPlayerMissionCompleta = missaoController.MaiorPlayerMissionCompleta;

        // Permite jogar apenas a próxima missão (Missão ID atual <= Missão concluída + 1)
        if (MissionID > MaiorPlayerMissionCompleta + 1)
        {
            LockMission();
            return;
        }

        UnlockMission();
    }

    public void LockMission()
    {
        Button.enabled = false;
        BlockPanel.SetActive(true);
    }
    public void UnlockMission()
    {
        Button.enabled = true;
        BlockPanel.SetActive(false);
    }
}
