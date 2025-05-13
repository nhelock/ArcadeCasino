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
        LoadJackpot();
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
        SaveJackpot();
    }

    public void ResetJackpot()
    {
        Jackpot = 0;
        SaveJackpot();
    }

    public void SaveWallet()
    {
        SaveData data = new SaveData { walletAmount = Wallet, jackpotAmount = Jackpot };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(), json);
    }

    public void LoadWallet()
    {
        string path = GetPath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Wallet = data.walletAmount;
        }
        else
        {
            Wallet = 1000;
        }
    }

    private void SaveJackpot()
    {
        SaveWallet(); // Jackpot is saved together with Wallet
    }

    private void LoadJackpot()
    {
        string path = GetPath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Jackpot = data.jackpotAmount;
        }
        else
        {
            Jackpot = 0;
        }
    }

    private string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, "wallet_save.json");
    }
}
