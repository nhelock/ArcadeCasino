using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumSlot : MonoBehaviour
{
    // Array of sprites 
    public Sprite[] numberSprites;  
    public Image slotImage;  
    public bool isSpinning = false; 

    // Individual stop delay per slot
    public float stopDelay = 0f;  

//--------------------------------------------------------------------//




    // Update is called once per frame
    void Update()
    {
        if (isSpinning)
        {
            slotImage.sprite = numberSprites[Random.Range(0, numberSprites.Length)];
        }
    }

    // This method is called when pressing Start
    public void StartSpinning()
    {
        isSpinning = true;
    }




    //This method is called when pressing Stop
    public void StopSpinning(Sprite finalSprite)
{
    StartCoroutine(StopAfterDelay(finalSprite));  
}





//This method is called when pressing Stop also but with a delay
IEnumerator StopAfterDelay(Sprite finalSprite)
{
    yield return new WaitForSeconds(stopDelay);  
    isSpinning = false;
    slotImage.sprite = finalSprite;
}
}
