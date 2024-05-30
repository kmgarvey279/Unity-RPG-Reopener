using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pathfinding.Ionic;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//thank you, llamacademy

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private IDataService DataService = new JSONDataService();
    private bool EncryptionEnabled = false;
    private readonly string relativePath = "/save-data.json";
        
    [field: Header("Contains *all* save files")]
    [field: SerializeReference] public SaveData SaveData { get; private set; }
    
    [field: Header("Loaded Save Data")]
    [field: SerializeReference] public LoadedData LoadedData { get; private set; }
    
    [field: Header("Session Data")]
    [field: SerializeReference] public SessionData SessionData { get; private set; }
    private bool sessionTimerActive = false;
    [field: SerializeField] public float sessionTimer = 0;

    #if UNITY_EDITOR
    [Header("Editor")]
    [SerializeField] private DebugData debugData;

    public void LoadDebugData()
    {
        LoadedData = debugData.Data;
        SessionData = new SessionData();

        StartSessionTimer(0);
    }
    #endif

    //find or create json file
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (CheckPath())
        {
            Debug.Log("Found Existing Save Data");
            SaveData = LoadGeneric<SaveData>();
        }
        else
        {
            Debug.Log("Save Data not found, creating new save data");
            SaveData = new SaveData();
            SaveData.PopulateFiles();

            SaveGeneric(SaveData);
        }
    }

    private void Update()
    {
        if (sessionTimerActive)
        {
            sessionTimer += Time.unscaledDeltaTime;
        }
    }

    public bool SaveLoadedData(int fileNum)
    {
        if (LoadedData == null || fileNum > SaveData.Files.Count)
        {
            return false;
        }

        LoadedData.SerializeData();
        SaveData.Files[fileNum].SetData(LoadedData.PlayerData, sessionTimer);

        SaveGeneric<SaveData>(SaveData);

        return true;
    }

    public void StartNewGame()
    {
        int firstEmptyFile = 1;
        for (int i = 1; i < Instance.SaveData.Files.Count; i++)
        {
            if (Instance.SaveData.Files[i].PlayerData != null)
            {
                firstEmptyFile = i;
                break;
            }
        }

        LoadedData = new LoadedData(firstEmptyFile, new PlayerData());
        SessionData = new SessionData();

        StartSessionTimer(0);
    }

    public bool LoadSelectedFile(SaveFile fileToLoad)
    {
        if (fileToLoad.PlayerData != null)
        {
            string serializedFileData = JsonConvert.SerializeObject(fileToLoad.PlayerData);
            PlayerData copy = JsonConvert.DeserializeObject<PlayerData>(serializedFileData);

            LoadedData = new LoadedData(fileToLoad.FileNum, copy);
            SessionData = new SessionData();

            StartSessionTimer(LoadedData.PlayerData.SystemData.Playtime);
            return true;
        }
        return false;
    }

    private void StartSessionTimer(float startValue)
    {
        sessionTimer = startValue;
        sessionTimerActive = true;
    }

    private void PauseSessionTimer()
    {
        sessionTimerActive = false;
    }

    private void StopSessionTimer() 
    {
        sessionTimerActive = false;
        sessionTimer = 0;
    }

    public static bool CheckPath()
    {
        return Instance.DataService.CheckPath(Instance.relativePath);
    }

    private void SaveGeneric<T>(T dataToSave)
    {
        if (DataService.Save(relativePath, dataToSave, EncryptionEnabled))
        {
            Debug.Log("Succesfuly saved data");
        }
        else
        {
            Debug.LogError("Unable to save data!");
        }
    }

    private T LoadGeneric<T>()
    {
        T data = DataService.Load<T>(relativePath, EncryptionEnabled);
        if (data != null)
        {
            Debug.Log("Succesfuly loaded data");
            return data;
        }
        else
        {
            Debug.LogError("Unable to load data!");
            return default;
        }
    }
}
