using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("GameData singleton initialized.");
    }

    public void NewGame()
    {
        WalletManager.Instance.ResetGameData(1000, 1000); // default values
        Debug.Log("New game started.");
    }

    public void SaveGame()
    {
        WalletManager.Instance.SaveWallet();
        Debug.Log("Game saved.");
    }

    public void LoadGame()
    {
        WalletManager.Instance.LoadWallet();
        Debug.Log($"Game loaded. Wallet: {WalletManager.Instance.Wallet}, Jackpot: {WalletManager.Instance.Jackpot}");
    }
}
