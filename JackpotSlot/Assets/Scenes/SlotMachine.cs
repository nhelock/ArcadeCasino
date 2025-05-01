using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SlotMachine : MonoBehaviour
{
    public SlotReel[] slotReels;   
    public Sprite jackpotSymbol;
    public Sprite[] rareSymbols; 

    public GameObject startButton;
    public GameObject stopButton;

    private int currentBet = 0;
    private bool betPlaced = false;

    public Text totalBetText;
    public Text jackpotText; 
    public Text totalMoneyText; 
    public Text resultText;

    public GameObject gameOverPanel;
    private Coroutine hideTextCoroutine;

    public Button add10Button, add25Button, add50Button;
    public Button clearBetButton;

    void Start()
    {
        stopButton.SetActive(false);

        Debug.Log("Slot Reels Count: " + slotReels.Length);
        for (int i = 0; i < slotReels.Length; i++)
        {
            Debug.Log("SlotReel " + i + ": " + slotReels[i]);
        }

        UpdateUI();
    }

    public void StartSpinning()
    {
        if (currentBet == 0) return;

        stopButton.SetActive(true);

        foreach (SlotReel reel in slotReels)
        {
            reel.StartSpinning();
        }
    }

    public void StopSpinning()
    {
        stopButton.SetActive(false);
        StartCoroutine(StopSlotsOneByOne());
    }

    IEnumerator StopSlotsOneByOne()
    {
        for (int i = 0; i < slotReels.Length; i++)
        {
            slotReels[i].StopSpinning(i * 0.3f); 
            yield return new WaitForSeconds(0.4f); 
        }

        yield return new WaitForSeconds(0.5f);
        CheckResults();
        betPlaced = false;
        currentBet = 0;
        totalBetText.text = "Bet: $0";
        UpdateUI();
    }

    void CheckResults()
    {
        float roll = Random.Range(0f, 1f);
        if (roll <= 0.25f)
        {
            List<Sprite> commonSymbols = new List<Sprite>();
            foreach (var symbol in slotReels[0].symbols)
            {
                if (symbol != jackpotSymbol && !System.Array.Exists(rareSymbols, s => s == symbol))
                {
                    commonSymbols.Add(symbol);
                }
            }

            Sprite commonSymbol = commonSymbols[Random.Range(0, commonSymbols.Count)];

            int startIndex = Random.Range(0, slotReels.Length - 2); 
            for (int i = startIndex; i < startIndex + 3; i++)
            {
                var renderer = slotReels[i].GetComponent<SpriteRenderer>();
                renderer.sprite = commonSymbol;
            }

            WalletManager.Instance.AddMoney(Mathf.RoundToInt(currentBet * 2f));
            resultText.gameObject.SetActive(true);
            resultText.text = "Common win! +2X bet";
            resultText.color = new Color32(174, 233, 255, 255);

            if (hideTextCoroutine != null)
                StopCoroutine(hideTextCoroutine);
            hideTextCoroutine = StartCoroutine(HideResultTextAfterDelay(3.5f));

            UpdateUI();

            if (WalletManager.Instance.Wallet <= 0 && currentBet == 0)
                StartCoroutine(ShowGameOverAfterDelay(1.5f));

            return;
        }

        int matchCount = 1;
        Sprite last = slotReels[0].GetCurrentSymbol();

        for (int i = 1; i < slotReels.Length; i++)
        {
            Sprite current = slotReels[i].GetCurrentSymbol();

            if (current == last)
            {
                matchCount++;

                if (matchCount == 3)
                {
                    bool allMatch = true;
                    foreach (var reel in slotReels)
                    {
                        if (reel.GetCurrentSymbol() != jackpotSymbol)
                        {
                            allMatch = false;
                            break;
                        }
                    }

                    if (allMatch)
                    {
                        WalletManager.Instance.AddMoney(WalletManager.Instance.Jackpot);
                        WalletManager.Instance.ResetJackpot();
                        resultText.gameObject.SetActive(true);
                        resultText.text = "JACKPOT WIN!";
                        resultText.color = new(255, 102, 204);
                        if (hideTextCoroutine != null)
                            StopCoroutine(hideTextCoroutine);
                        hideTextCoroutine = StartCoroutine(HideResultTextAfterDelay(3.5f));
                        StartCoroutine(JackpotPulse());
                        UpdateUI();
                        return;
                    }
                    else if (System.Array.Exists(rareSymbols, s => s == last))
                    {
                        WalletManager.Instance.AddMoney(currentBet * 5);
                        resultText.gameObject.SetActive(true);
                        resultText.text = "RARE prize! 5x your bet!";
                        resultText.color = new(255, 215, 0);
                        if (hideTextCoroutine != null)
                            StopCoroutine(hideTextCoroutine);
                        hideTextCoroutine = StartCoroutine(HideResultTextAfterDelay(3.5f));
                        UpdateUI();
                        return;
                    }
                    else
                    {
                        WalletManager.Instance.AddMoney(Mathf.RoundToInt(currentBet * 2f));
                        resultText.gameObject.SetActive(true);
                        resultText.text = "You won a common prize! x2";
                        resultText.color = new Color32(174, 233, 255, 255);
                        if (hideTextCoroutine != null)
                            StopCoroutine(hideTextCoroutine);
                        hideTextCoroutine = StartCoroutine(HideResultTextAfterDelay(3.5f));
                        UpdateUI();
                        return;
                    }
                }
            }
            else
            {
                matchCount = 1;
                last = current;
            }
        }

        WalletManager.Instance.AddToJackpot((int)(currentBet * 0.5f));
        resultText.gameObject.SetActive(true);
        resultText.text = "No win. Jackpot increased.";
        resultText.color = new(170, 170, 170);
        StartCoroutine(JackpotPulse());

        if (hideTextCoroutine != null)
            StopCoroutine(hideTextCoroutine);
        hideTextCoroutine = StartCoroutine(HideResultTextAfterDelay(3.5f));
        UpdateUI();

        if (WalletManager.Instance.Wallet <= 0)
            StartCoroutine(ShowGameOverAfterDelay(1.5f));
    }

    public void SetBet1()
    {
        if (!WalletManager.Instance.SubtractMoney(1)) return;

        currentBet += 1;
        UpdateUI();
    }

    public void SetBet5()
    {
        if (betPlaced || !WalletManager.Instance.SubtractMoney(5)) return;

        currentBet = 5;
        betPlaced = true;
        UpdateUI();
    }

    public void SetBet10()
    {
        if (betPlaced || !WalletManager.Instance.SubtractMoney(10)) return;

        currentBet = 10;
        betPlaced = true;
        UpdateUI();
    }

    void UpdateUI()
    {
        totalBetText.text = "Total Bet: $" + currentBet;
        jackpotText.text = "Jackpot: $" + WalletManager.Instance.Jackpot;
        totalMoneyText.text = "Money: $" + WalletManager.Instance.Wallet;
    }

    public void AddMoney(int amount)
    {
        WalletManager.Instance.AddMoney(amount);
        gameOverPanel.SetActive(false);
        StartCoroutine(FlashMoney(Color.yellow));
        UpdateUI();
    }

    IEnumerator ShowGameOverAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameOverPanel.SetActive(true);
    }

    IEnumerator HideResultTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        resultText.gameObject.SetActive(false);
    }

    IEnumerator JackpotPulse()
    {
        Vector3 originalScale = jackpotText.transform.localScale;
        Vector3 targetScale = originalScale * 1.15f;

        float time = 0f;
        float duration = 0.3f;

        while (time < duration)
        {
            time += Time.deltaTime;
            jackpotText.transform.localScale = Vector3.Lerp(originalScale, targetScale, time / duration);
            yield return null;
        }

        time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            jackpotText.transform.localScale = Vector3.Lerp(targetScale, originalScale, time / duration);
            yield return null;
        }

        jackpotText.transform.localScale = originalScale;
    }

    IEnumerator FlashMoney(Color flashColor)
    {
        Color original = totalMoneyText.color;
        totalMoneyText.color = flashColor;
        yield return new WaitForSeconds(0.2f);
        totalMoneyText.color = original;
    }
    
    public void ClearBet()
{
    if (currentBet > 0)
    {
        WalletManager.Instance.AddMoney(currentBet);
        currentBet = 0;
        UpdateUI();
    }
}



   
}
