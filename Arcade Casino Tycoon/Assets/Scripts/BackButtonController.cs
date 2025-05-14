using UnityEngine;
using UnityEngine.UIElements;

public class BackButtonController : MonoBehaviour
{
    public enum BackTarget { MainMenu, GameMenu }
    public BackTarget backTarget = BackTarget.GameMenu; // Default to GameMenu
    public GameObject customTarget; // Optional for custom targets

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var backButton = root.Q<Button>("back-button");

        if (backButton == null)
        {
            Debug.LogError("Back button not found!");
        }

        backButton.clicked += () =>
        {
            Debug.Log("Back button clicked!");

            // Check where to go based on the BackTarget
            switch (backTarget)
            {
                case BackTarget.MainMenu:
                    Debug.Log("Going to Main Menu...");
                    MainMenu.Instance?.ShowMainMenu();
                    break;
                case BackTarget.GameMenu:
                    Debug.Log("Going to Game Menu...");
                    MainMenu.Instance?.ShowGameMenu();
                    break;
                default:
                    if (customTarget != null)
                    {
                        Debug.Log("Activating custom target...");
                        customTarget.SetActive(true);
                    }
                    break;
            }
        };
    }
}
