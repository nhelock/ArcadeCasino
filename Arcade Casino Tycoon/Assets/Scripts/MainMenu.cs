using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }

    public GameObject MenuPanel;
    public GameObject GameMenu;


    private void Start()
    {
        ShowMainMenu();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshUIReferences();
        ShowMainMenu();
    }

    private void RefreshUIReferences()
{
    // Ensure the GameObjects are correctly found
    if (MenuPanel == null)
    {
        MenuPanel = GameObject.Find("MenuPanel");
        if (MenuPanel == null) Debug.LogError("MenuPanel not found!");
    }

    if (GameMenu == null)
    {
        GameMenu = GameObject.Find("GameMenu");
        if (GameMenu == null) Debug.LogError("GameMenu not found!");
    }
}

    public void ShowMainMenu()
    {
        RefreshUIReferences();

        if (MenuPanel != null) MenuPanel.SetActive(true);
        if (GameMenu != null) GameMenu.SetActive(false);
    }

    public void ShowGameMenu()
{
    Debug.Log("Showing Game Menu...");

    // Refresh UI references
    RefreshUIReferences();

    // Deactivate the MenuPanel and activate the GameMenu
    if (MenuPanel != null)
    {
        MenuPanel.SetActive(false);
        Debug.Log("MenuPanel set to inactive.");
    }
    else
    {
        Debug.LogWarning("MenuPanel is null.");
    }

    if (GameMenu != null)
    {
        GameMenu.SetActive(true);
        Debug.Log("GameMenu set to active.");
    }
    else
    {
        Debug.LogWarning("GameMenu is null.");
    }
}

    public void NewGame()
    {
        ShowGameMenu();
    }

    public void LoadGame()
    {
        if (SaveSystem.SaveFileExists())
        {
            SaveData data = SaveSystem.Load();
            if (data != null)
            {
                MenuGameManager.Instance.ApplySaveData(data);
                ShowGameMenu();
            }
            else
            {
                Debug.LogWarning("Failed to load save data.");
            }
        }
        else
        {
            Debug.LogWarning("No save found.");
        }
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }

    public void Back()
    {
        ShowMainMenu();
    }
}
