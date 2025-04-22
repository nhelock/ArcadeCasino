using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {

        //put final loading scene into here
        SceneManager.LoadScene("GameScene");
    }

    public void LoadGame()
    {
        if (SaveSystem.SaveFileExists())
        {
            //put final loading scene into here
            SceneManager.LoadScene("GameScene");
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
}
