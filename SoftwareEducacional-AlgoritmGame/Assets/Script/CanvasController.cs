using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public static CanvasController Singleton;

    // Start is called before the first frame update
    [SerializeField] GameObject DialoguePanel;
    [SerializeField] GameObject TouchControllPanel;
    [SerializeField] GameObject InventoryPanel;
    [SerializeField] GameObject UiPanel;
    [SerializeField] GameObject MenuPanel;
    [SerializeField] GameObject MisisonPanel;
    [SerializeField] GameObject EndGamePanel;
    [SerializeField] GameObject TutorialPanels;
    
    void Awake()
    {
        Singleton = this;
        TouchControllPanel.SetActive(Application.isMobilePlatform);

    }


    public void OpenEndGame()
    {
        ResetPanel();
        EndGamePanel.SetActive(true);
    }
    private void Start()
    {
        GameObject obj = GameObject.Find("FirstLogin");
        if (obj != null)
        {
            OpenTutorialPanel();
            Destroy(obj);
            return;
        }
        DefaultPainels();
    } 
    void Update()
    {
        if (DialoguePanel.activeSelf)
        {
            UiPanel.SetActive(false);
            return;
        }
    }

    private void OpenTutorialPanel()
    {
        ResetPanel();
        TutorialPanels.SetActive(true);

    }

    public void ResetPanel()
    {
        if(Application.isMobilePlatform)
            TouchControllPanel.SetActive(false);
        DialoguePanel.SetActive(false);
        InventoryPanel.SetActive(false);
        UiPanel.SetActive(false);
        MenuPanel.SetActive(false);
        MisisonPanel.SetActive(false);
        EndGamePanel.SetActive(false);
        TutorialPanels.SetActive(false);
    }

    public void OpenMissionPanel()
    {
        ResetPanel();
        MisisonPanel.SetActive(true);

    }
    public void OpenInventoryPainel()
    {
        ResetPanel();
        InventoryPanel.SetActive(true);
    }

    public void OpenMenuPanel()
    {
        ResetPanel();
        MenuPanel.SetActive(true);
    }

    public void DefaultPainels()
    {
        ResetPanel();
        UiPanel.SetActive(true);
        if (Application.isMobilePlatform)
            TouchControllPanel.SetActive(true);

    }
}
