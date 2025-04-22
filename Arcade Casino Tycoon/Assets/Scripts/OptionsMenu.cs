using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsMenu : MonoBehaviour
{
    public GameData gameData;
    public GameObject saveConfirmationText;

    public void SaveGame()
    {
        gameData.SaveGame();
        Debug.Log("Game Saved!");
    }

    private IEnumerator ShowSaveConfirmation()
    {
        saveConfirmationText.SetActive(true);
        yield return new WaitForSeconds(2f);
        saveConfirmationText.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }
}
