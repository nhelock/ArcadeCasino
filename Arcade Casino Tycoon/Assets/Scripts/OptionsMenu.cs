using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class OptionsMenu : MonoBehaviour
{
    public static OptionsMenu Instance { get; private set; }

    [Header("References")]
    public GameData gameData;
    public GameObject optionsCanvas;

    [Header("Save Confirmation")]
    public GameObject saveConfirmationPanel;
    public TMP_Text saveConfirmationText;
    public float displayTime = 2f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("[OptionsMenu] Initialized as Singleton.");
    }

    private void Start()
    {
        InitializeUI();
        EnsureEventSystem();
    }

    private void InitializeUI()
    {
        if (optionsCanvas != null) optionsCanvas.SetActive(false);
        if (saveConfirmationPanel != null) saveConfirmationPanel.SetActive(false);
        Debug.Log("[OptionsMenu] UI Initialized.");
    }

    private void EnsureEventSystem()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            Debug.Log("[OptionsMenu] Created new EventSystem.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("[OptionsMenu] Escape pressed - toggling menu.");
            ToggleOptionsMenu();
        }
    }

    public void ToggleOptionsMenu()
    {
        if (optionsCanvas == null)
        {
            Debug.LogWarning("[OptionsMenu] OptionsCanvas reference is missing!");
            return;
        }

        bool isOpening = !optionsCanvas.activeSelf;
        optionsCanvas.SetActive(isOpening);

        // Pause/unpause game
        Time.timeScale = isOpening ? 0f : 1f;
        Debug.Log($"[OptionsMenu] Menu {(isOpening ? "opened" : "closed")}. TimeScale: {Time.timeScale}");
    }

    public void SaveGame()
    {
        if (gameData != null)
        {
            gameData.SaveGame();
            ShowSaveConfirmation("Game Saved Successfully!");
            Debug.Log("[OptionsMenu] Game saved.");
        }
        else
        {
            Debug.LogError("[OptionsMenu] GameData reference is missing!");
        }
    }

    private void ShowSaveConfirmation(string message)
    {
        if (saveConfirmationPanel != null && saveConfirmationText != null)
        {
            saveConfirmationText.text = message;
            saveConfirmationPanel.SetActive(true);
            StartCoroutine(HideSaveConfirmation());
        }
        else
        {
            Debug.LogWarning("[OptionsMenu] Save confirmation references missing!");
        }
    }

    private IEnumerator HideSaveConfirmation()
    {
        yield return new WaitForSecondsRealtime(displayTime);
        saveConfirmationPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("[OptionsMenu] Exiting game...");
        Application.Quit();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        if (optionsCanvas != null) optionsCanvas.SetActive(false);
        Debug.Log($"[OptionsMenu] Scene loaded: {scene.name}. Menu closed.");
    }
}