using UnityEngine;

public class ColorScanUIManager : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject orangePokedexPanel;

    [Header("Specific Detail Panels")]
    [SerializeField] private GameObject blueCharacteristicsPanel;
    [SerializeField] private GameObject redHistoryPanel;
    [SerializeField] private GameObject yellowFunFactsPanel;
    [SerializeField] private GameObject greenWorldRecordsPanel;
    [SerializeField] private GameObject whiteTypesPanel;

    [Header("Prompt UI")]
    [SerializeField] private GameObject scanPromptUI;

    [Header("Stabilization Settings")]
    [Tooltip("Minimum time (in seconds) to keep a UI open before allowing another scan event to override it.")]
    [SerializeField] private float scanCooldown = 2.0f;

    private float lastScanTime = -10f;
    private GameObject currentActivePanel;

    void Start()
    {
        CloseAllPanels();
    }

    // ==========================================
    // STABILIZED IMAGE TRACKING EVENTS
    // ==========================================

    public void OnOrangeFaceScanned() => TryOpenPanel(orangePokedexPanel, "Orange");
    public void OnBlueFaceScanned() => TryOpenPanel(blueCharacteristicsPanel, "Blue");
    public void OnRedFaceScanned() => TryOpenPanel(redHistoryPanel, "Red");
    public void OnYellowFaceScanned() => TryOpenPanel(yellowFunFactsPanel, "Yellow");
    public void OnGreenFaceScanned() => TryOpenPanel(greenWorldRecordsPanel, "Green");
    public void OnWhiteFaceScanned() => TryOpenPanel(whiteTypesPanel, "White");

    private void TryOpenPanel(GameObject targetPanel, string colorName)
    {
        // Ignore rapid phantom triggers if a target was recently scanned
        if (Time.time - lastScanTime < scanCooldown) return;

        // If the requested panel is already visible, don't restart it
        if (currentActivePanel == targetPanel && targetPanel.activeSelf) return;

        Debug.Log($"[AR Scan] Confirmed target: {colorName}");
        lastScanTime = Time.time;
        OpenSinglePanel(targetPanel);
    }

    // ==========================================
    // UI NAVIGATION BUTTONS
    // ==========================================

    public void OpenBluePanel() => OpenSinglePanel(blueCharacteristicsPanel);
    public void OpenRedPanel() => OpenSinglePanel(redHistoryPanel);
    public void OpenYellowPanel() => OpenSinglePanel(yellowFunFactsPanel);
    public void OpenGreenPanel() => OpenSinglePanel(greenWorldRecordsPanel);
    public void OpenWhitePanel() => OpenSinglePanel(whiteTypesPanel);
    public void OpenPokedexPanel() => OpenSinglePanel(orangePokedexPanel);

    // ==========================================
    // HELPER METHODS
    // ==========================================

    private void OpenSinglePanel(GameObject panelToOpen)
    {
        CloseAllPanels();

        if (scanPromptUI != null) scanPromptUI.SetActive(false);
        
        if (panelToOpen != null)
        {
            panelToOpen.SetActive(true);
            currentActivePanel = panelToOpen;
        }
    }

    public void CloseAllPanels()
    {
        if (orangePokedexPanel != null) orangePokedexPanel.SetActive(false);
        if (blueCharacteristicsPanel != null) blueCharacteristicsPanel.SetActive(false);
        if (redHistoryPanel != null) redHistoryPanel.SetActive(false);
        if (yellowFunFactsPanel != null) yellowFunFactsPanel.SetActive(false);
        if (greenWorldRecordsPanel != null) greenWorldRecordsPanel.SetActive(false);
        if (whiteTypesPanel != null) whiteTypesPanel.SetActive(false);

        if (scanPromptUI != null) scanPromptUI.SetActive(true);
        currentActivePanel = null;
    }
}