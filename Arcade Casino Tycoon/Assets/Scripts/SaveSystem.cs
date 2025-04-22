using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string filePath = Application.persistentDataPath + "/save.json";

    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
        Debug.Log("Game Saved to: " + filePath);
    }

    public static SaveData Load()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            Debug.LogWarning("No save file found at: " + filePath);
            return null;
        }
    }

    public static bool SaveFileExists()
    {
        return File.Exists(filePath);
    }
}
