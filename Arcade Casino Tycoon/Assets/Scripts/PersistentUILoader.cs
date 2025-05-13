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
            DontDestroyOnLoad(instance);
            Debug.Log("[PersistentUILoader] OptionsMenu instantiated.");
        }
        else
        {
            Debug.LogError("[PersistentUILoader] OptionsMenu prefab not assigned!");
        }

        isInitialized = true;
        DontDestroyOnLoad(gameObject);
    }
}