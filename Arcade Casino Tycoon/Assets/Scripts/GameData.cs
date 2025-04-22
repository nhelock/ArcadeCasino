using UnityEngine;

public class GameData : MonoBehaviour
{
    //filler for real data when finalized
    public string playerName = "Hero";
    public int playerLevel = 1;
    public Transform playerTransform;

    public void SaveGame()
    {
        SaveData data = new SaveData
        {
            playerName = playerName,
            playerLevel = playerLevel,
            playerPosition = playerTransform.position
        };

        SaveSystem.Save(data);
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.Load();

        if (data != null)
        {
            playerName = data.playerName;
            playerLevel = data.playerLevel;
            playerTransform.position = data.playerPosition;

            Debug.Log($"Loaded Player: {playerName}, Level: {playerLevel}, Position: {playerTransform.position}");
        }
    }
}
