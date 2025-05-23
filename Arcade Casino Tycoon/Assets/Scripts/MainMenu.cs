using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }

    public GameObject MenuPanel;
    public GameObject GameMenu;


    private void Start()
    {
        if (GameData.Instance != null)
        {
            GameData.Instance.SaveGame();
        }
        else
        {
            Debug.LogWarning("GameData.Instance is null. Cannot save.");
        }

        ShowMainMenu();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshUIReferences();
        ShowMainMenu();
    }

    private void RefreshUIReferences()
{
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

    RefreshUIReferences();

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
        WalletManager.Instance.SetWallet(1000);
        WalletManager.Instance.SetJackpot(1000);   
        ShowGameMenu();
    }




    public void LoadGame()
    {
        WalletManager.Instance.LoadWallet();

        Debug.Log("Wallet loaded. Current amount: " + WalletManager.Instance.Wallet);
        ShowGameMenu();
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
