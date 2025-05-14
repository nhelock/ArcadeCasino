using UnityEngine;

public class PersistentUILoader : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenuPrefab;
    private static bool isInitialized = false;

    private void Awake()
    {
        if (isInitialized)
        {
            Destroy(gameObject);
            return;
        }

        if (optionsMenuPrefab != null)
        {
            GameObject instance = Instantiate(optionsMenuPrefab);
            instance.name = "OptionsMenu (Instantiated)";  // Clear identification
            DontDestroyOnLoad(instance);
            Debug.Log("[PersistentUI] OptionsMenu instantiated", instance);
        }
        else
        {
            Debug.LogError("[PersistentUI] OptionsMenu prefab not assigned!");
        }

        isInitialized = true;
        DontDestroyOnLoad(gameObject);
    }
}