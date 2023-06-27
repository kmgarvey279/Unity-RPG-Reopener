using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad 
{
    public static List<SaveData> savedGames = new List<SaveData>();

    public static void Save()
    {
        savedGames.Add(SaveData.current);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        binaryFormatter.Serialize(file, savedGames);
        file.Close();
    }

    public static void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            SaveLoad.savedGames = (List<SaveData>)binaryFormatter.Deserialize(file);
            file.Close();
        }
    }
}
