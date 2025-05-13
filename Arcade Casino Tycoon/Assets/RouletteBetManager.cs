using UnityEngine;
using TMPro;

public class RouletteBetManager : MonoBehaviour
{
    public RouletteWheel wheel;
    public TMP_InputField numberInput;

    public void BetOnColor(string color)
    {
        StartCoroutine(wheel.SpinWheel(() =>
        {
            if (wheel.currentColor.Equals(color, System.StringComparison.OrdinalIgnoreCase))
                Debug.Log($"You won on {color}!");
            else
                Debug.Log("You lost.");
        }));
    }

    public void BetOnNumber()
    {
        string bet = numberInput.text.Trim();

        if (string.IsNullOrEmpty(bet))
        {
            Debug.Log("Invalid input.");
            return;
        }

        StartCoroutine(wheel.SpinWheel(() =>
        {
            if (wheel.currentLabel == bet)
                Debug.Log($"You won on number {bet}!");
            else
                Debug.Log("You lost.");
        }));
    }
}

