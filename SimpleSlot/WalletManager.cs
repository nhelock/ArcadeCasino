
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalletManager
{
    private static bool hi = false;
    private static WalletManager hInstance;
    public static WalletManager Instance
    {
        get
        {
            if (!hi)
            {
                hInstance = new WalletManager();
                hi = true;
            }

            return hInstance;
        }
        
        private set
        {
            hInstance = value;
        }
    }

    public int Wallet
    {
        get; private set;
    }
    private const string WalletKey = "PlayerWallet";

   private WalletManager()
{
    Wallet = PlayerPrefs.GetInt(WalletKey, 1000);
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

    private void SaveWallet()
    {
        PlayerPrefs.SetInt(WalletKey, Wallet);
        PlayerPrefs.Save();
    }
}

// // //Functions you CAN do

// // WalletManager.Instance.AddMoney(40);

// // WalletManager.Instance.SubtractMoney(10)
// // //Can run an if-statement to see if this is possible