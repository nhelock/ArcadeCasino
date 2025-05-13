using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject MenuPanel;
    public GameObject OptionsMenu;
    public GameObject GameMenu;
    public GameObject BackButton;

    private void Start()
    {
        // Ensure only the main menu is visible when starting
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        MenuPanel.SetActive(true);
        OptionsMenu.SetActive(false);
        GameMenu.SetActive(false);
    }

    public void ShowOptions()
    {
        MenuPanel.SetActive(false);
        OptionsMenu.SetActive(true);
        GameMenu.SetActive(false);
    }

    public void ShowGameMenu()
    {
        MenuPanel.SetActive(false);
        OptionsMenu.SetActive(false);
        GameMenu.SetActive(true);
    }

    public void NewGame()
    {
        ShowGameMenu();
    }

    public void LoadGame()
    {
        if (SaveSystem.SaveFileExists())
        {
            ShowGameMenu();
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

    public void Back() => ShowMainMenu();
}