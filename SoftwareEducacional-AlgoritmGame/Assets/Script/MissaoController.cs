using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MissaoController : MonoBehaviour
{
    [SerializeField] GameObject missoesPanel; 
    [SerializeField] GameObject missaoPanel;
    [SerializeField] Image spriteItem;
    [SerializeField] TMP_Text nomeMissao;
    [SerializeField] TMP_Text descricaoMissao;
    [SerializeField] int PlayerID;
    [SerializeField] int ultimaMissaoAberta;
    public int MaiorPlayerMissionCompleta;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        FecharMissaoPanel();
        UpdateMaiorPlayerMissionCompleta();
    }

    public void UpdateMaiorPlayerMissionCompleta()
    {
        BancoDeDados bancoDeDados = new BancoDeDados();
        MaiorPlayerMissionCompleta = bancoDeDados.GetMaiorIDMissao(PlayerID);
    }

    public void ObrirMissaoPanel(int missionID)
    {
        ultimaMissaoAberta = missionID;

        InfoMissionUpdate(missionID);
        missaoPanel.SetActive(true);
        missoesPanel.SetActive(false);
    }
    public void FecharMissaoPanel()
    {
        missaoPanel.SetActive(false);
        missoesPanel.SetActive(true);
    }

    public void InfoMissionUpdate(int missionID)
    {
        BancoDeDados bancoDeDados = new BancoDeDados();
        int idItemRecompensa = bancoDeDados.GetMissaoIdItem(missionID);
        //Debug.Log($"{idItemRecompensa}");
        Item item = Inventory.Singleton.allItemsList[idItemRecompensa -1];
        spriteItem.sprite = item.sprite;
        nomeMissao.text = bancoDeDados.GetMissionName(missionID);
        descricaoMissao.text =bancoDeDados.GetMissaoDescricao(missionID);

    }
    public void IrParaMissao()
    {
        int missionID = ultimaMissaoAberta;
        // Construa o nome da cena baseado no ID da missão
        string nomeCena = "Missao " + missionID;

        // Verifique se a cena existe no build settings antes de carregar
        if (SceneUtility.GetBuildIndexByScenePath(nomeCena) != -1)
        {
            SceneManager.LoadScene(nomeCena);
        }
        else
        {
            Debug.LogError($"Cena '{nomeCena}' não foi encontrada. Verifique se ela está adicionada no Build Settings.");
        }
    }

}
