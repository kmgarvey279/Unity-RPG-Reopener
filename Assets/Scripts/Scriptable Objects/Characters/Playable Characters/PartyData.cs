using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PartyMember
{
    public PlayableCharacterInfo playableCharacterInfo;
    public bool inParty;
}

[CreateAssetMenu(fileName = "New Party Data", menuName = "PartyData")]
public class PartyData : ScriptableObject
{
    public List<PartyMember> partyMembers;

    public void SwapPartyMembers(PartyMember partyMember1, PartyMember partyMember2)
    {
        int index1 = partyMembers.IndexOf(partyMember1);
        int index2 = partyMembers.IndexOf(partyMember2);
        partyMembers[index1] = partyMember2;
        partyMembers[index2] = partyMember1;
    }
}
