using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByName(string sceneName) {
        GameData gameData = FindObjectOfType<GameData>();
        if (gameData != null)
        {
            gameData.SaveGame();
        }
        else
        {
            Debug.LogWarning("No GameData object found in the scene.");
        }

        EventSystem currentEventSystem = EventSystem.current;
        if (currentEventSystem != null)
        {
            Destroy(currentEventSystem.gameObject);
        }
        SceneManager.LoadScene(sceneName);
    }
}
