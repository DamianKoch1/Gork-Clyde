using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

/// <summary>
/// Saves options/unlocked levels or wipes save
/// </summary>
public class GameSaver : MonoBehaviour
{

    private static int highestLevelId = 0;

  
    public static int HighestLevelId
    {
        get => LoadHighestLevelId();
        private set => SaveHighestLevelId(value);
    }

    private void Start()
    {
        UpdateHighestLevelId();
    }

    /// <summary>
    /// Checks for level number in scene name, sets highestLevelId to that if its lower
    /// </summary>
    private void UpdateHighestLevelId()
    {
        HighestLevelId = int.Parse(new string(SceneManager.GetActiveScene().name.Where(Char.IsDigit).ToArray()));
    }

    /// <summary>
    /// Loads highestLevelId from save file
    /// </summary>
    /// <returns>Returns 1 if no save file, otherwise returns loaded value</returns>
    private static int LoadHighestLevelId()
    {
        highestLevelId = 1;
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "Save.dat");
        if (filePaths.Length <= 0) return highestLevelId;
    
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        using (FileStream fileStream = File.Open(Path(), FileMode.Open))
        {
            highestLevelId = (int)binaryFormatter.Deserialize(fileStream);
        }
        return highestLevelId;
    }

    /// <summary>
    /// Saves new highestLevelId if higher than saved
    /// </summary>
    /// <param name="newValue">New value to save</param>
    private static void SaveHighestLevelId(int newValue)
    {
        if (newValue <= LoadHighestLevelId()) return;

        highestLevelId = newValue;

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        using (FileStream fileStream = File.Open(Path(), FileMode.OpenOrCreate))
        {
            binaryFormatter.Serialize(fileStream, newValue);
        }
    }
    
    /// <summary>
    /// Default save file path
    /// </summary>
    /// <returns>Default save file path</returns>
    private static String Path()
    {
        return System.IO.Path.Combine(Application.persistentDataPath, "Save.dat");
    }


    /// <summary>
    /// Clears PlayerPrefs and deletes save
    /// </summary>
    public static void WipeSave()
    {
        PlayerPrefs.DeleteAll();
        File.Delete(Path());
    }
}
