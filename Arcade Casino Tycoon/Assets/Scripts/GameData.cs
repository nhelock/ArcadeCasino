using UnityEngine;

public class GameData : MonoBehaviour
{
    public void SaveGame()
    {
        WalletManager.Instance.SaveWallet();
        Debug.Log("Game saved (wallet only).");
    }

    public void LoadGame()
    {
        WalletManager.Instance.LoadWallet();
        Debug.Log($"Game loaded. Wallet: {WalletManager.Instance.Wallet}");
    }
}