using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PartyData
{
    public List<PlayableCharacterID> PartyMembers = new List<PlayableCharacterID>() { PlayableCharacterID.Claire };
    public List<PlayableCharacterID> ActivePartyMembers = new List<PlayableCharacterID>() { PlayableCharacterID.Claire };

    public List<PlayableCharacterID> ReservePartyMembers
    {
        get
        {
            List<PlayableCharacterID> reserveParty = new List<PlayableCharacterID>();
            foreach (PlayableCharacterID playableCharacterID in PartyMembers)
            {
                if (!ActivePartyMembers.Contains(playableCharacterID))
                {
                    reserveParty.Add(playableCharacterID);
                }
            }
            return reserveParty;
        }
    }

    public Dictionary<PlayableCharacterID, PlayableCombatantData> PlayableCombatantDataDict = new Dictionary<PlayableCharacterID, PlayableCombatantData>()
    {
        { PlayableCharacterID.Claire, new PlayableCombatantData(PlayableCharacterID.Claire) },
        { PlayableCharacterID.Mutiny, new PlayableCombatantData(PlayableCharacterID.Mutiny) },
        { PlayableCharacterID.Blaine, new PlayableCombatantData(PlayableCharacterID.Blaine) },
        { PlayableCharacterID.Shad, new PlayableCombatantData(PlayableCharacterID.Shad) },
        { PlayableCharacterID.Oshi, new PlayableCombatantData(PlayableCharacterID.Oshi) },
        { PlayableCharacterID.Lucy, new PlayableCombatantData(PlayableCharacterID.Lucy) }
    };

    public void AddToActiveParty(PlayableCharacterID playableCharacterID)
    {
        if (PartyMembers.Contains(playableCharacterID) && !ActivePartyMembers.Contains(playableCharacterID) && ActivePartyMembers.Count < 3)
        {
            ActivePartyMembers.Add(playableCharacterID);
            ActivePartyMembers = ActivePartyMembers.OrderBy(playableCharacterID => playableCharacterID).ToList();
        }
    }

    public void RemoveFromActiveParty(PlayableCharacterID playableCharacterID)
    {
        if (PartyMembers.Contains(playableCharacterID) && ActivePartyMembers.Contains(playableCharacterID) && ActivePartyMembers.Count > 1)
        {
            ActivePartyMembers.Remove(playableCharacterID);
        }
    }

    public void AddToParty(PlayableCharacterID playableCharacterID)
    {
        if (!PartyMembers.Contains(playableCharacterID))
        {
            PartyMembers.Add(playableCharacterID);
            PartyMembers = PartyMembers.OrderBy(playableCharacterID => playableCharacterID).ToList();

            if (ActivePartyMembers.Count < 3)
            {
                ActivePartyMembers.Add(playableCharacterID);
                ActivePartyMembers = ActivePartyMembers.OrderBy(playableCharacterID => playableCharacterID).ToList();
            }
        }
    }

    public void RemoveFromParty(PlayableCharacterID playableCharacterID)
    {
        if (PartyMembers.Contains(playableCharacterID))
        {
            PartyMembers.Remove(playableCharacterID);
            if (ActivePartyMembers.Contains(playableCharacterID))
            {
                ActivePartyMembers.Remove(playableCharacterID);

                if (ActivePartyMembers.Count == 0)
                {
                    AddToActiveParty(PartyMembers[0]);
                }
            }
        }
    }
}
