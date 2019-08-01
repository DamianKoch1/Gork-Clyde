using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameSaver : MonoBehaviour
{

    private static int highestLevelId = 0;

    public static int HighestLevelId
    {
        get
        {
            if (highestLevelId != 0) return highestLevelId;
        
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
        set
        {
            if (value <= highestLevelId) return;
            
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (FileStream fileStream = File.Open(Path(), FileMode.OpenOrCreate))
            {
                binaryFormatter.Serialize(fileStream, value);
            }
        }
    }

    private void Start()
    {
        HighestLevelId = int.Parse(new string(SceneManager.GetActiveScene().name.Where(Char.IsDigit).ToArray()));
    }

    private static String Path()
    {
        return System.IO.Path.Combine(Application.persistentDataPath, "Save.dat");
    }
}
