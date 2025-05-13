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
    public GameObject optionsPanel;

    [Header("Save Confirmation")]
    public GameObject saveConfirmationPanel;
    public TMP_Text saveConfirmationText;
    public float displayTime = 2f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeUI();
        EnsureEventSystem();
    }

    private void InitializeUI()
    {
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (saveConfirmationPanel != null) saveConfirmationPanel.SetActive(false);
    }

    private void EnsureEventSystem()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            DontDestroyOnLoad(eventSystem);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
    {
        Debug.Log("Escape key pressed - toggling Options Menu.");
        ToggleOptionsMenu();
    }
    }

    public void ToggleOptionsMenu()
    {
        if (optionsPanel == null) return;

        bool willOpen = !optionsPanel.activeSelf;
        optionsPanel.SetActive(willOpen);
        Time.timeScale = willOpen ? 0f : 1f;
    }

    public void SaveGame()
    {
        gameData.SaveGame();
        ShowSaveConfirmation("Game Saved Successfully!");
    }

    private void ShowSaveConfirmation(string message)
    {
        if (saveConfirmationPanel != null)
        {
            saveConfirmationText.text = message;
            saveConfirmationPanel.SetActive(true);
            StartCoroutine(HideSaveConfirmation());
        }
    }

    private IEnumerator HideSaveConfirmation()
    {
        yield return new WaitForSecondsRealtime(displayTime);
        saveConfirmationPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
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
        if (optionsPanel != null) optionsPanel.SetActive(false);
    }
}
