using UnityEngine;
using System.IO;

public class WalletManager
{
    private static WalletManager hInstance;
    public static WalletManager Instance
    {
        get
        {
            if (hInstance == null)
                hInstance = new WalletManager();

            return hInstance;
        }
    }

    public int Wallet { get; private set; }
    public int Jackpot { get; private set; }

    private WalletManager()
    {
        LoadWallet();
    }

    public void AddMoney(int amount)
    {
        Wallet += amount;
        SaveWallet();
    }

    public bool SubtractMoney(int amount)
    {
        if (Wallet >= amount)
        {
            Wallet -= amount;
            SaveWallet();
            return true;
        }
        return false;
    }

    public void AddToJackpot(int amount)
    {
        Jackpot += amount;
        SaveWallet(); // Save both values together
    }

    public void ResetJackpot()
    {
        Jackpot = 0;
        SaveWallet(); // Save both values together
    }

    public void SaveWallet()
    {
        SaveData data = new SaveData { walletAmount = Wallet, jackpotAmount = Jackpot };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(), json);
        Debug.Log("Saved to: " + Application.persistentDataPath);

    }

    public void LoadWallet()
    {
        string path = GetPath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Wallet = data.walletAmount;
            Jackpot = data.jackpotAmount;
        }
        else
        {
            Wallet = 1000;
            Jackpot = 0;
        }
    }

    public void ResetGameData(int startingWallet, int startingJackpot)
    {
        Wallet = startingWallet;
        Jackpot = startingJackpot;
        SaveWallet();
    }

    private string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, "wallet_save.json");
    }

    public void SetWallet(int amount)
    {
        Wallet = amount;
        SaveWallet();
    }

    public void SetJackpot(int amount)
    {
        Jackpot = amount;
        SaveWallet();
    }

}
