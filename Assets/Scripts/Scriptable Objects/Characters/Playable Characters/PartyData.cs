using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]

[CreateAssetMenu(fileName = "New Party Data", menuName = "PartyData")]
public class PartyData : ScriptableObject
{
    [SerializeField] private List<PlayableCharacterID> PartyList;

    [SerializeField] private List<PlayableCharacterInfo> PlayableCharacterInfoList;
    public Dictionary<PlayableCharacterID, PlayableCharacterInfo> PlayableCharacterInfoDict { get; private set; }
    public List<PlayableCharacterID> PartySlots { get; private set; }
    public List<Vector2Int> SpawnPositions { get; private set; } = new List<Vector2Int>();

    private void OnEnable()
    {
        PlayableCharacterInfoDict = new Dictionary<PlayableCharacterID, PlayableCharacterInfo>();
        PartySlots = new List<PlayableCharacterID>()
        {
        PlayableCharacterID.None,
        PlayableCharacterID.None,
        PlayableCharacterID.None,
        PlayableCharacterID.None,
        PlayableCharacterID.None,
        PlayableCharacterID.None
        };
        SpawnPositions = new List<Vector2Int>()
        {
            new Vector2Int(0,1),
            new Vector2Int(0,0),
            new Vector2Int(0,2),
        };

        foreach (PlayableCharacterInfo playableCharacterInfo in PlayableCharacterInfoList)
        {
            PlayableCharacterInfoDict.Add(playableCharacterInfo.PlayableCharacterID, playableCharacterInfo);
        }
        PlayableCharacterInfoDict.Add(PlayableCharacterID.None, null);

        for (int i = 0; i < PartyList.Count; i++)
        {
            if(i >= 6)
            {
                break;
            }
            PartySlots[i] = PartyList[i];
        }
    }

    public PlayableCharacterInfo GetSlotInfo(int slot)
    {
        if(PartySlots[slot] == PlayableCharacterID.None)
        {
            return null;
        }
        return PlayableCharacterInfoDict[PartySlots[slot]];
    }

    public void SwapPartyMembers(PlayableCharacterID partyMember1, PlayableCharacterID partyMember2)
    {
        int index1 = PartySlots.IndexOf(partyMember1);
        int index2 = PartySlots.IndexOf(partyMember2);
        PartySlots[index1] = partyMember2;
        PartySlots[index2] = partyMember1;
    }

    public void AddToParty(PlayableCharacterID playableCharacterID)
    {
        for (int i = 0; i < PartySlots.Count; i++)
        {
            if (PartySlots[i] == PlayableCharacterID.None)
            {
                PartySlots[i] = playableCharacterID; 
                break;
            }
        }
    }

    public void RemoveFromParty(PlayableCharacterID playableCharacterID)
    {
        for (int i = 0; i < PartySlots.Count; i++)
        {
            if (PartySlots[i] == playableCharacterID)
            {
                PartySlots[i] = PlayableCharacterID.None;
                break;
            }
        }
    }
}
