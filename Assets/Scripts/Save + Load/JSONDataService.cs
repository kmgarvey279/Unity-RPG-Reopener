using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class JSONDataService : IDataService
{
    public bool Save<T>(string RelativePath, T Data, bool Encrypted)
    {
        string path = Application.persistentDataPath + RelativePath;
        
        try
        {
            if (File.Exists(path))
            {
                Debug.Log("Overwriting existing save data");
                File.Delete(path);
            }

            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonConvert.SerializeObject(Data));
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
            return false;
        }
    }

    public bool CheckPath(string RelativePath)
    {
        string path = Application.persistentDataPath + RelativePath;
        if (File.Exists(path))
        {
            return true;
        }
        return false;
    }

    public T Load<T>(string RelativePath, bool Encrypted)
    {
        var serializerSettings = new JsonSerializerSettings
        {
            ObjectCreationHandling = ObjectCreationHandling.Replace
        };

        string path = Application.persistentDataPath + RelativePath;
        if (!File.Exists(path))
        {
            Debug.LogError($"Cannot load data from {path}!");
            throw new FileNotFoundException($"{path} does not exist");
        }
        
        try
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path), serializerSettings);
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
            throw e;
        }
    }
}
