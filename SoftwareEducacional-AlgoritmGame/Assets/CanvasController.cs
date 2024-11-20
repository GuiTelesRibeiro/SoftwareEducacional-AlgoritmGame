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
    
    void Awake()
    {
        Singleton = this;
        TouchControllPanel.SetActive(Application.isMobilePlatform);
    }
    private void Start()
    {
        DefaultPainels();
    }

    public void ResetPanel()
    {
        if(Application.isMobilePlatform)
            TouchControllPanel.SetActive(false);
        DialoguePanel.SetActive(false);
        InventoryPanel.SetActive(false);
        UiPanel.SetActive(false);
        MenuPanel.SetActive(false);

    }
    void Update()
    {
        if (DialoguePanel.activeSelf)
        {
            UiPanel.SetActive(false);
            return;
        }
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
