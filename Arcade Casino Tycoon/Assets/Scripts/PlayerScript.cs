using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // --- This script is for BOTH player and dealer

    public CardScript cardScript;
    public DeckScript deckScript;

    public int handValue = 0;

    public GameObject[] hand;
    public int cardIndex = 0;

    // Tracking aces for 1 to 11 conversions
    private List<CardScript> aceList = new List<CardScript>();

    public void StartHand()
    {
        GetCard();
        GetCard();
    }

    public int GetCard()
    {
        int cardValue = deckScript.DealCard(hand[cardIndex].GetComponent<CardScript>());

        hand[cardIndex].GetComponent<Renderer>().enabled = true;

        handValue += cardValue;

        if (cardValue == 1)
        {
            aceList.Add(hand[cardIndex].GetComponent<CardScript>());
        }

        AceCheck();
        cardIndex++;

        return handValue;
    }

    public void AceCheck()
    {
        foreach (CardScript ace in aceList)
        {
            if (handValue + 10 < 22 && ace.GetValueOfCard() == 1)
            {
                ace.SetValue(11);
                handValue += 10;
            }
            else if (handValue > 21 && ace.GetValueOfCard() == 11)
            {
                ace.SetValue(1);
                handValue -= 10;
            }
        }
    }

    public void AdjustMoney(int amount)
    {
        if (amount >= 0)
        {
            WalletManager.Instance.AddMoney(amount);
        }
        else
        {
            // Try to subtract only if enough money
            bool success = WalletManager.Instance.SubtractMoney(Mathf.Abs(amount));
            if (!success)
            {
                Debug.LogWarning("Not enough funds to subtract " + Mathf.Abs(amount));
            }
        }
    }

    public int GetMoney()
    {
        return WalletManager.Instance.Wallet;
    }

    public void ResetHand()
    {
        for (int i = 0; i < hand.Length; i++)
        {
            hand[i].GetComponent<CardScript>().ResetCard();
            hand[i].GetComponent<Renderer>().enabled = false;
        }

        cardIndex = 0;
        handValue = 0;
        aceList = new List<CardScript>();
    }
}
