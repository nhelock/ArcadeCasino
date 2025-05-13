using TMPro;
using UnityEngine;

public class WalletDisplay : MonoBehaviour
{
    public TMP_Text walletText;
    
    private void Update()
    {
        walletText.text = $"${WalletManager.Instance.Wallet:N0}";
    }
}