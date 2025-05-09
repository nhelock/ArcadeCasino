using TMPro;
using UnityEngine;

public class WalletUI : MonoBehaviour
{
    private TextMeshProUGUI walletText;

    private void Start()
    {
        // Auto-find a TextMeshProUGUI in the scene named "WalletText"
        walletText = GameObject.Find("WalletText")?.GetComponent<TextMeshProUGUI>();

        if (walletText == null)
        {
            Debug.LogError("WalletText not found! Make sure there's a TextMeshProUGUI object named 'WalletText' in the scene.");
            return;
        }

        UpdateWalletUI();
    }

    private void Update()
    {
        if (walletText != null)
        {
            UpdateWalletUI();
        }
    }

    void UpdateWalletUI()
    {
        walletText.text = $"Wallet: ${WalletManager.Instance.Wallet}";
    }
}
