using UnityEngine;

public class PersistentUILoader : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenuPrefab;
    private static GameObject optionsMenuInstance;

    private void Awake()
    {
        if (optionsMenuInstance != null)
        {
            Destroy(gameObject);
            return;
        }

        optionsMenuInstance = Instantiate(optionsMenuPrefab);
        DontDestroyOnLoad(optionsMenuInstance);
        Debug.Log("Options menu instantiated from PersistentUILoader.");
        DontDestroyOnLoad(gameObject);
    }
}
