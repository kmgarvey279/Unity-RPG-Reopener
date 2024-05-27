using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class SaveData
{
    public List<SaveFile> Files;

    public void PopulateFiles()
    {
        Files = new List<SaveFile>();
        for (int i = 0; i < 13; i++)
        {
            Files.Add(new SaveFile(i));
        }
    }
}
