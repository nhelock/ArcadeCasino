using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    {
        if (GameData.Instance != null)
        {
            GameData.Instance.SaveGame();
        }
        else
        {
            Debug.LogWarning("GameData.Instance is null. Cannot save.");
        }

        EventSystem currentEventSystem = EventSystem.current;
        if (currentEventSystem != null)
        {
            Destroy(currentEventSystem.gameObject);
        }

        SceneManager.LoadScene(sceneName);
    }
}
