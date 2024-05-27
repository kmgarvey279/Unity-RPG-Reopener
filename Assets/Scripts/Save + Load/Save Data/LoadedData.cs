using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoadedData
{
    [field: Header("File/Savable Data")]
    [field: SerializeField] public int LastInteractedFileNum { get; private set; }
    [field: SerializeField] public PlayerData PlayerData { get; private set; }

    [Header("Party")]
    [SerializeField] private List<PlayableCombatantRuntimeData> combatantRuntimeData = new List<PlayableCombatantRuntimeData>();

    public LoadedData(int fileNum, PlayerData playerData)
    {
        LastInteractedFileNum = fileNum;
        PlayerData = playerData;

        foreach (PlayableCharacterID id in PlayerData.PartyData.PartyMembers)
        {
            combatantRuntimeData.Add(new PlayableCombatantRuntimeData(PlayerData.PartyData.PlayableCombatantDataDict[id]));
        }
    }

    public PlayableCombatantRuntimeData GetPlayableCombatantRuntimeData(PlayableCharacterID playableCharacterID)
    {
        foreach (PlayableCombatantRuntimeData runtimeData in combatantRuntimeData)
        {
            if (runtimeData.PlayableCharacterID == playableCharacterID)
            {
                return runtimeData;
            }
        }
        return null;
    }

    //prep for saving
    public void SerializeData()
    {
        WritePlayableCombatantRuntimeData();
    }

    public void WritePlayableCombatantRuntimeData()
    {
        foreach (PlayableCombatantRuntimeData characterRuntimeData in combatantRuntimeData)
        {
            PlayerData.PartyData.PlayableCombatantDataDict[characterRuntimeData.PlayableCharacterID] = characterRuntimeData.GetSerializableData();
        }
    }
}
