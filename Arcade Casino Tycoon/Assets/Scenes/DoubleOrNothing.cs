using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleOrNothing : MonoBehaviour
{
    public Text winningsText;
    public Text resultText;
    public Button doubleButton;
    public Button cashOutButton;
    public Image coinImage;
    public Sprite heads;
    public Sprite tails;
    public float flipDuration = 1.5f;
    private bool flipping = false;

    private int currentWinnings = 100;
    private int totalBet = 0;

    public GameObject gameOverPanel;
    public Text gameOverText;
    public Button add10Button;
    public Button add25Button;
    public Button add50Button;

    public Text totalBetText;
    public Button bet1Button;
    public Button bet5Button;
    public Button bet10Button;
    public Button bet25Button;
    public Button clearBetButton;



    public Image glowImage;

    void Start()
    {
        UpdateUI();
        resultText.gameObject.SetActive(false);
        coinImage.sprite = heads;

        doubleButton.onClick.AddListener(DoubleWinnings);
        cashOutButton.onClick.AddListener(CashOut);

        gameOverPanel.SetActive(false);

        add10Button.onClick.AddListener(() => AddWinnings(10));
        add25Button.onClick.AddListener(() => AddWinnings(25));
        add50Button.onClick.AddListener(() => AddWinnings(50));

        bet1Button.onClick.AddListener(() => AddToBet(1));
        bet5Button.onClick.AddListener(() => AddToBet(5));
        bet10Button.onClick.AddListener(() => AddToBet(10));
        bet25Button.onClick.AddListener(() => AddToBet(25));

        clearBetButton.onClick.AddListener(ClearBet);
    }

    void UpdateUI()
    {
        winningsText.text = "You have: $" + currentWinnings;
        totalBetText.text = "Total Bet: $" + totalBet;
    }

    void AddToBet(int amount)
    {
        if (currentWinnings >= amount)
        {
            currentWinnings -= amount;
            totalBet += amount;
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough money to place this bet.");
        }
    }

    void DoubleWinnings()
    {
        if (flipping || totalBet == 0) return;

        bool win = Random.value < 0.5f;
        StartCoroutine(FlipCoin(win));
    }

    IEnumerator FlipCoin(bool win)
    {
        flipping = true;

        resultText.gameObject.SetActive(false);
        coinImage.gameObject.SetActive(true);
        glowImage.gameObject.SetActive(true);

        StartCoroutine(PulseGlow());

        float elapsed = 0f;

        while (elapsed < flipDuration)
        {
            float rotationY = Mathf.PingPong(elapsed * 720f, 180f);
            coinImage.rectTransform.rotation = Quaternion.Euler(0f, rotationY, 0f);

            if (rotationY > 90f)
                coinImage.sprite = win ? heads : tails;
            else
                coinImage.sprite = (Random.value > 0.5f) ? heads : tails;

            yield return null;
            elapsed += Time.deltaTime;
        }

        coinImage.sprite = win ? heads : tails;

        yield return new WaitForSeconds(0.5f);

        flipping = false;
        glowImage.gameObject.SetActive(false);
        coinImage.rectTransform.rotation = Quaternion.identity;

        if (win)
        {
            currentWinnings += totalBet * 2;
            resultText.text = "You doubled your bet! Now at $" + currentWinnings + "!";
        }
        else
        {
            resultText.text = "You're Cooked L Loser";

            doubleButton.interactable = true;
            cashOutButton.interactable = true;

            if (currentWinnings == 0)
            {
                coinImage.gameObject.SetActive(false);
                gameOverPanel.SetActive(true);
                gameOverText.text = "GAME OVER\nAdd funds to play again!";
            }
        }


        totalBet = 0;
        resultText.gameObject.SetActive(true);
        UpdateUI();
    }

    IEnumerator PulseGlow()
    {
        float pulseSpeed = 2f;
        Vector3 originalScale = glowImage.rectTransform.localScale;
        Vector3 targetScale = originalScale * 1.2f;

        Color originalColor = glowImage.color;

        while (flipping)
        {
            float t = Mathf.PingPong(Time.time * pulseSpeed, 1f);
            glowImage.rectTransform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            glowImage.color = Color.Lerp(originalColor, originalColor * 1.2f, t);
            yield return null;
        }

        glowImage.rectTransform.localScale = originalScale;
        glowImage.color = originalColor;
    }

    void CashOut()
    {
        resultText.gameObject.SetActive(true);
        resultText.text = "Cashed out with $" + currentWinnings;

        doubleButton.interactable = false;
        cashOutButton.interactable = false;
    }

    void AddWinnings(int amount)
    {
        currentWinnings += amount;
        UpdateUI();

        coinImage.gameObject.SetActive(true);
        gameOverPanel.SetActive(false);
        doubleButton.interactable = true;
        cashOutButton.interactable = true;
        resultText.gameObject.SetActive(false);
    }

    void ClearBet()
    {
        currentWinnings += totalBet;
        totalBet = 0;
        UpdateUI();
    }   

}
