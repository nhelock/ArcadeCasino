using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetManager : MonoBehaviour
{
    public float resultDisplayTime = 2f;
    public Image[] slotImages;
    public Sprite[] slotSprites;

    public Text totalMoneyText;
    public Text totalBetText;
    public Text resultText;
    public Text gameOverText;
    public Button startButton;
    public Button add50Button;
    public Button add25Button;
    public Button add10Button;
    public Text playAgainText;

    private int currentBet = 0;

//--------------------------------------------------------------------//

    void Start()
    {
        UpdateMoneyUI();
        UpdateBetUI();
        UpdateStartButton();

        resultText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        playAgainText.gameObject.SetActive(false);
        add50Button.gameObject.SetActive(false);
        add25Button.gameObject.SetActive(false);
        add10Button.gameObject.SetActive(false);
    }

    public void StartSpin()
    {
        StartCoroutine(SpinSlotsAndCheckWin());
    }

    public void AddBet(int amount)
    {
        if (WalletManager.Instance.SubtractMoney(amount))
        {
            currentBet += amount;
            UpdateMoneyUI();
            UpdateBetUI();
            UpdateStartButton();
        }
    }

    public void AddMoney(int amount)
    {
        WalletManager.Instance.AddMoney(amount);
        UpdateMoneyUI();

        if (WalletManager.Instance.Wallet > 0)
        {
            startButton.interactable = true;
            add50Button.gameObject.SetActive(false);
            add25Button.gameObject.SetActive(false);
            add10Button.gameObject.SetActive(false);
            gameOverText.gameObject.SetActive(false);
            playAgainText.gameObject.SetActive(false);
        }
    }

    IEnumerator SpinSlotsAndCheckWin()
    {
        bool isWin = Random.value <= 0.35f;
        int winningIndex = Random.Range(0, slotSprites.Length);

        Debug.Log("Winning Symbol Index: " + winningIndex);
        Debug.Log("Winning Sprite: " + slotSprites[winningIndex].name);

        foreach (NumSlot slot in FindObjectsOfType<NumSlot>())
        {
            slot.StartSpinning();
        }

        yield return new WaitForSeconds(3f);

        if (isWin)
        {
            foreach (NumSlot slot in FindObjectsOfType<NumSlot>())
            {
                slot.StopSpinning(slotSprites[winningIndex]);
            }
        }
        else
        {
            for (int i = 0; i < slotImages.Length; i++)
            {
                int randomIndex = Random.Range(0, slotSprites.Length);
                FindObjectsOfType<NumSlot>()[i].StopSpinning(slotSprites[randomIndex]);
            }
        }

        yield return new WaitForSeconds(2f);

        if (isWin)
        {
            int winnings = currentBet * 2;
            WalletManager.Instance.AddMoney(winnings);
            resultText.text = "+$" + winnings;
        }
        else
        {
            resultText.text = "-$" + currentBet;
        }

        resultText.gameObject.SetActive(true);
        yield return new WaitForSeconds(resultDisplayTime);
        resultText.gameObject.SetActive(false);

        currentBet = 0;
        UpdateMoneyUI();
        UpdateBetUI();

        if (WalletManager.Instance.Wallet <= 0)
        {
            StartCoroutine(ShowGameOver());
        }
    }

    public void OnStopButtonClicked()
    {
        StartCoroutine(StopAfterDelay());
    }

    IEnumerator StopAfterDelay()
    {
        yield return new WaitForSeconds(5f);

        currentBet = 0;
        UpdateMoneyUI();
        UpdateBetUI();

        startButton.interactable = false;
    }

    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(resultDisplayTime);
        gameOverText.gameObject.SetActive(true);
        playAgainText.gameObject.SetActive(true);
        startButton.interactable = false;
        add50Button.gameObject.SetActive(true);
        add10Button.gameObject.SetActive(true);
        add25Button.gameObject.SetActive(true);
    }

    public void UpdateMoneyUI()
    {
        totalMoneyText.text = "Money: $" + WalletManager.Instance.Wallet;
    }

    IEnumerator AnimateMoneyChange(int targetAmount)
    {
        int currentAmount;

        if (!int.TryParse(totalMoneyText.text.Replace("Money: $", ""), out currentAmount))
        {
            currentAmount = 0;
        }

        while (currentAmount != targetAmount)
        {
            currentAmount = (int)Mathf.MoveTowards(currentAmount, targetAmount, 5f);
            totalMoneyText.text = "Money: $" + currentAmount;
            yield return new WaitForSeconds(0.05f);
        }
    }

    void UpdateBetUI()
    {
        totalBetText.text = "Bet: $" + currentBet;
    }

    IEnumerator AnimateBetChange(int targetAmount)
    {
        int currentAmount;

        if (!int.TryParse(totalBetText.text.Replace("Bet: $", ""), out currentAmount))
        {
            currentAmount = 0;
        }

        while (currentAmount != targetAmount)
        {
            currentAmount = (int)Mathf.MoveTowards(currentAmount, targetAmount, 2f);
            totalBetText.text = "Bet: $" + currentAmount;
            yield return new WaitForSeconds(0.05f);
        }
    }

    void UpdateStartButton()
    {
        startButton.interactable = (currentBet > 0);
    }

    // Optional debug reset
    public void ResetWallet()
    {
        PlayerPrefs.DeleteKey("PlayerWallet");
        WalletManager.Instance.AddMoney(1000);
        UpdateMoneyUI();
    }
    
   

    
}
