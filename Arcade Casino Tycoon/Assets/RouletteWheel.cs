using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 


[System.Serializable]
public class RouletteSegment
{
    public string label; // "0", "00", "1", etc.
    public string color; // "Red", "Black", "Green"
    public float startAngle; // degrees
    public float endAngle;
}

public class RouletteWheel : MonoBehaviour
{
    private string currentBetColor = null; // Stores player's color bet
    private string currentBetNumber = null; // Stores player's number bet

    public Transform wheel;
    public List<RouletteSegment> segments = new List<RouletteSegment>();

    private readonly string[] numberOrder = new string[]
    {
        "0", "28", "9", "26", "30", "11", "7", "20", "32", "17", "5", "22",
        "34", "15", "3", "24", "36", "13", "1", "00", "27", "10", "25", "29",
        "12", "8", "19", "31", "18", "6", "21", "33", "16", "4", "23", "35",
        "14", "2"
    };

    private readonly HashSet<string> redNumbers = new HashSet<string>
    {
        "1", "3", "5", "7", "9", "12", "14", "16", "18",
        "19", "21", "23", "25", "27", "30", "32", "34", "36"
    };

    public string currentColor;
    public string currentLabel;

    public TMP_InputField numberInputField; // Reference to the InputField for betting a number

    private void Awake()
    {
        GenerateSegments();
    }

    private void GenerateSegments()
    {
        segments.Clear();
        float segmentSize = 360f / numberOrder.Length;

        for (int i = 0; i < numberOrder.Length; i++)
        {
            float centerAngle = (270f + i * segmentSize) % 360f;
            float start = (centerAngle - segmentSize / 2 + 360f) % 360f;
            float end = (centerAngle + segmentSize / 2) % 360f;

            string number = numberOrder[i];
            string color;

            if (number == "0" || number == "00")
                color = "Green";
            else if (redNumbers.Contains(number))
                color = "Red";
            else
                color = "Black";

            segments.Add(new RouletteSegment
            {
                label = number,
                color = color,
                startAngle = start,
                endAngle = end
            });
        }
    }

    public IEnumerator SpinWheel(System.Action onSpinComplete)
    {
        float duration = 3f;
        float elapsed = 0f;
        float startRotation = wheel.localEulerAngles.x;
        float targetRotation = startRotation + Random.Range(720f, 1080f);

        while (elapsed < duration)
        {
            float rotation = Mathf.Lerp(startRotation, targetRotation, elapsed / duration);
            wheel.localEulerAngles = new Vector3(rotation, 0f, 90f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        wheel.localEulerAngles = new Vector3(targetRotation, 0f, 90f);
        DetermineSegment(targetRotation % 360f);
        onSpinComplete?.Invoke();
    }

    private void DetermineSegment(float xRotation)
    {
        foreach (var segment in segments)
        {
            bool inRange = segment.startAngle < segment.endAngle
                ? xRotation >= segment.startAngle && xRotation < segment.endAngle
                : xRotation >= segment.startAngle || xRotation < segment.endAngle;

            if (inRange)
            {
                currentLabel = segment.label;
                currentColor = segment.color;
                Debug.Log($"Landed on: {currentLabel} ({currentColor})");
                return;
            }
        }

        Debug.LogWarning("Could not determine result.");
    }

    public void StartSpin()
    {
        if (!WalletManager.Instance.SubtractMoney(10))
        {
            Debug.LogWarning("Not enough money to spin.");
            return;
        }

        StartCoroutine(SpinWheel(() =>
        {
            Debug.Log("Spin Complete!");
        }));
    }

    // Bet on Color
    public void BetOnColor(string color)
    {
        if (!WalletManager.Instance.SubtractMoney(10))
        {
            Debug.LogWarning("Not enough money to bet.");
            return;
        }

        currentBetColor = color;

        StartCoroutine(SpinWheel(() =>
        {
            DetermineSegment(wheel.localEulerAngles.x % 360f);

            if (currentColor == currentBetColor)
            {
                int reward = currentColor == "Green" ? 1000 : 50;
                WalletManager.Instance.AddMoney(reward);
                Debug.Log($"You WON! Bet: {currentBetColor}, Result: {currentLabel} ({currentColor}) — Gained {reward}. New Wallet: {WalletManager.Instance.Wallet}");
            }
            else
            {
                Debug.Log($"You LOST! Bet: {currentBetColor}, Result: {currentLabel} ({currentColor}) — Lost 10. Wallet: {WalletManager.Instance.Wallet}");
            }

            currentBetColor = null;
        }));
    }

    // Bet on Number
    public void BetOnNumber()
    {
        if (!WalletManager.Instance.SubtractMoney(10))
        {
            Debug.LogWarning("Not enough money to bet.");
            return;
        }

        currentBetNumber = numberInputField.text;

        StartCoroutine(SpinWheel(() =>
        {
            DetermineSegment(wheel.localEulerAngles.x % 360f);

            if (currentLabel == currentBetNumber)
            {
                WalletManager.Instance.AddMoney(500);
                Debug.Log($"You WON! Bet: {currentBetNumber}, Result: {currentLabel} — Gained 500. New Wallet: {WalletManager.Instance.Wallet}");
            }
            else
            {
                Debug.Log($"You LOST! Bet: {currentBetNumber}, Result: {currentLabel} — Lost 10. Wallet: {WalletManager.Instance.Wallet}");
            }

            currentBetNumber = null; // reset bet
        }));
    }
}
