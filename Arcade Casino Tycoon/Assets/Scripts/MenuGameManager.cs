using UnityEngine;

public class MenuGameManager : MonoBehaviour
{
    public static MenuGameManager Instance;

    public int walletAmount;
    public int jackpotAmount;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ApplySaveData(SaveData data)
    {
        walletAmount = data.walletAmount;
        jackpotAmount = data.jackpotAmount;

        Debug.Log("Loaded wallet: " + walletAmount);
        Debug.Log("Loaded jackpot: " + jackpotAmount);
    }
}
