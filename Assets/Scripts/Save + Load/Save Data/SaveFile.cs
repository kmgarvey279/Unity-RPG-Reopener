using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile
{
    //public bool HasData = false;

    public int FileNum;
    public string SaveLocation = "New";
    public float Playtime;
    public string SaveDate = "--/--/----";
    [SerializeReference] public PlayerData PlayerData;

    public SaveFile(int fileNum)
    {
        FileNum = fileNum;
    }

    public void SetData(PlayerData playerData, float sessionTimer)
    {
        //HasData = true;

        PlayerData = playerData;
        SaveLocation = playerData.CurrentLocationName;
        Playtime = sessionTimer;
        SaveDate = DateTime.Now.ToString("MM/dd/yyyy");
    }
}
