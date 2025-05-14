using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByName(string sceneName) {
        EventSystem currentEventSystem = EventSystem.current;
        if (currentEventSystem != null)
        {
            Destroy(currentEventSystem.gameObject);
        }
        SceneManager.LoadScene(sceneName);
    }
}
