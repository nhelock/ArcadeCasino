using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSpin : MonoBehaviour
{

    public float genSpeed;
    public float slowSpeed;
    public bool isSpinning = false;
    public bool hasPaid;

    // Update is called once per frame
    void Update()
    {
        if (isSpinning)
        {
            transform.Rotate(genSpeed, 0, 0, Space.World);
            genSpeed -= slowSpeed;
        }

        if (genSpeed <= 0 && isSpinning)
        {
            genSpeed = 0;
            isSpinning = false;

            // Calculate the Z-Axis
            float zRotation = transform.eulerAngles.z;

            // Normalize to [0, 360)
            zRotation = zRotation % 360f;
            if (zRotation < 0f)
                zRotation += 360f;

            // Determine if it's in red or green
            int sliceIndex = Mathf.FloorToInt(zRotation / 60f);

            // Odd results are winners for this wheel
            bool isWin = (sliceIndex % 2 != 0);

            // Log result
            Debug.Log($"Z Rotation: {zRotation}° — Slice: {sliceIndex} — Result: {(isWin ? "WIN!" : "TRY AGAIN")}");

            if (isWin)
            {
                WalletManager.Instance.AddMoney(40);
            }
        }
    }


    public void SpinWheel()
    {
        if (!isSpinning)
        {
            if (WalletManager.Instance.SubtractMoney(10))
            {
                hasPaid = true;

                genSpeed = Random.Range(5.000f, 8.000f);
                slowSpeed = Random.Range(0.003f, 0.008f);
                isSpinning = true;

                Debug.Log("Game Started, Money Taken from Wallet");
            }
            else
            {
                Debug.Log("You need more Money, here have another");
                WalletManager.Instance.AddMoney(100);

                Debug.Log("Not enough money to play");
            }
        }

        
    }
}
