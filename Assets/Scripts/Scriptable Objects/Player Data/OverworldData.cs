using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Overworld Data", menuName = "Runtime Data/Overworld Data")]
public class OverworldData: ScriptableObject
{
    public Vector2 PlayerLocation { get; private set; }
    public Room CurrentRoom { get; private set; }
    public string PreviousSceneName { get; private set; }
    public int NextSceneEntryPoint { get; private set; }    

    public void OnEnable()
    {
        PreviousSceneName = "";        
        PlayerLocation = Vector2.zero;
    }

    public void SetPreviousSceneName(string previousSceneName)
    {
        PreviousSceneName = previousSceneName;
    }

    public void SetCurrentRoom(Room room)
    {
        CurrentRoom = room;
    }

    public void SetNextSceneEntryPoint(int entryPointIndex)
    {
        NextSceneEntryPoint = entryPointIndex;
    }
}
