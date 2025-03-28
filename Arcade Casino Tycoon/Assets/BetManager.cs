using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetManager : MonoBehaviour
{
    // How long to show win/lose text
    public float resultDisplayTime = 2f;          
    public Image[] slotImages;                    
    public Sprite[] slotSprites;                  

    //References Total Money and Text / Buttons
    public int totalMoney = 100;                 
    public Text totalMoneyText;                   
    public Text totalBetText;                     
    public Text resultText;                       
    public Text gameOverText;                     
    public Button startButton;                    
    public Button add50Button;                    
    public Button add25Button;                    
    public Button add10Button;                    
    private int currentBet = 0;                   
    public Text playAgainText;  



//--------------------------------------------------------------------//


    void Start()
    {
        UpdateMoneyUI();                          
        UpdateBetUI();                            
        UpdateStartButton(); 

        //Hides all these buttons when started
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
        if (totalMoney >= amount)
        {
            currentBet += amount;
            totalMoney -= amount;
            UpdateMoneyUI();
            UpdateBetUI();
            UpdateStartButton();
        }
    }




    public void AddMoney(int amount)
    {
        totalMoney += amount;
        UpdateMoneyUI();

        if (totalMoney > 0)
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
    // float spinDelay = 0.5f; 

    // Randomly decide if it's a win or not at 35% chance right now 
    bool isWin = Random.value <= 0.35f;  
    int winningIndex = Random.Range(0, slotSprites.Length);  


    //Debugged in console to make sure the winning images are the same
    Debug.Log("Winning Symbol Index: " + winningIndex);
    Debug.Log("Winning Sprite: " + slotSprites[winningIndex].name);


    // Start spinning animation in NumSlot.cs
    foreach (NumSlot slot in FindObjectsOfType<NumSlot>())
    {
        slot.StartSpinning();
    }

    // Let sprites spin for 3 seconds
    yield return new WaitForSeconds(3f);  


    // Stop all slots properly
    if (isWin)
    {
        foreach (NumSlot slot in FindObjectsOfType<NumSlot>())
        {
            // Output winning sprite
            slot.StopSpinning(slotSprites[winningIndex]);  
        }
    }
    else
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            // Output random sprite
            int randomIndex = Random.Range(0, slotSprites.Length);
            FindObjectsOfType<NumSlot>()[i].StopSpinning(slotSprites[randomIndex]);  
        }
    }

    yield return new WaitForSeconds(2f);

    // Adds or subtracts the money to the total money
    if (isWin)
    {
        int winnings = currentBet * 2;
        totalMoney += winnings;
        resultText.text = "+$" + winnings;
    }
    else
    {
        resultText.text = "-$" + currentBet;
    }

    resultText.gameObject.SetActive(true);
    yield return new WaitForSeconds(resultDisplayTime);
    resultText.gameObject.SetActive(false);




    // Resets the game
    currentBet = 0;
    UpdateMoneyUI();
    UpdateBetUI();

    if (totalMoney <= 0)
    {
        StartCoroutine(ShowGameOver());
    }
}





    public void OnStopButtonClicked()
    {
        StartCoroutine(StopAfterDelay());
    }

    // Stops the slots after 5 seconds after stop is pressed
    IEnumerator StopAfterDelay()
    {
        yield return new WaitForSeconds(5f);  

        currentBet = 0;
        UpdateMoneyUI();
        UpdateBetUI();

        startButton.interactable = false;
    }



    // Shows the game over text and buttons
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
        totalMoneyText.text = "Money: $" + totalMoney;
    }
   IEnumerator AnimateMoneyChange(int targetAmount)
{
    int currentAmount;
    
    // Try to parse the existing text (safety check in case of formatting issues)
    if (!int.TryParse(totalMoneyText.text.Replace("Money: $", ""), out currentAmount))
    {
        currentAmount = 0;  // Default to 0 if parsing fails
    }

    while (currentAmount != targetAmount)
    {
        currentAmount = (int)Mathf.MoveTowards((float)currentAmount, (float)targetAmount, 5f);  // Smooth increase/decrease
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
    
    // Try to parse the existing text (safety check)
    if (!int.TryParse(totalBetText.text.Replace("Bet: $", ""), out currentAmount))
    {
        currentAmount = 0;  // Default to 0 if parsing fails
    }

    while (currentAmount != targetAmount)
    {
        currentAmount = (int)Mathf.MoveTowards((float)currentAmount, (float)targetAmount, 2f);  // Smooth bet increase
        totalBetText.text = "Bet: $" + currentAmount;
        yield return new WaitForSeconds(0.05f);  
    }
}


    void UpdateStartButton()
    {
        startButton.interactable = (currentBet > 0);
    }
}