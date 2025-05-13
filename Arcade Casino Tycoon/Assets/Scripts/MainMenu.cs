using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject GameMenu;

    private void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        MenuPanel.SetActive(true);
        if (OptionsMenu.Instance != null)
            OptionsMenu.Instance.gameObject.SetActive(false);

        GameMenu.SetActive(false);
    }

    public void ShowOptions()
    {
        MenuPanel.SetActive(false);
        if (OptionsMenu.Instance != null)
            OptionsMenu.Instance.gameObject.SetActive(true);

        GameMenu.SetActive(false);
    }

    public void ShowGameMenu()
    {
        MenuPanel.SetActive(false);
        if (OptionsMenu.Instance != null)
            OptionsMenu.Instance.gameObject.SetActive(false);

        GameMenu.SetActive(true);
    }

    public void NewGame() => ShowGameMenu();

    public void LoadGame()
    {
        if (SaveSystem.SaveFileExists())
            ShowGameMenu();
        else
            Debug.LogWarning("No save found.");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }

    public void Back() => ShowMainMenu();
}
