using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanelController : MonoBehaviour
{
    [SerializeField] GameObject tela1Panel;
    [SerializeField] GameObject tela2Panel;
    [SerializeField] GameObject tela3Panel;
    [SerializeField] GameObject tela4Panel;
    [SerializeField] GameObject tela5Panel;
    [SerializeField] GameObject tela6Panel;
    // Start is called before the first frame update
    void OnEnable()
    {
        DefaultPanel();
    }

    public void ResetPanels()
    {
        tela1Panel.SetActive(false); 
        tela2Panel.SetActive(false);
        tela3Panel.SetActive(false);
        tela4Panel.SetActive(false);
        tela5Panel.SetActive(false);
        tela6Panel.SetActive(false);
    }
    public void DefaultPanel()
    {
        OpenTela1Panel();
    }

    public void OpenTela1Panel()
    {
        ResetPanels();
        tela1Panel.SetActive(true);
    }
    public void OpenTela2Panel()
    {
        ResetPanels();
        tela2Panel.SetActive(true);

    }
    public void OpenTela3Panel()
    {
        ResetPanels();
        tela3Panel.SetActive(true);
    }
    public void OpenTela4Panel()
    {
        ResetPanels();
        tela4Panel.SetActive(true);
    }
    public void OpenTela5Panel()
    {
        ResetPanels();
        tela5Panel.SetActive(true);
    }
    public void OpenTela6Panel()
    {
        ResetPanels();
        tela6Panel.SetActive(true);
    }
}
