using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PartyMember
{
    public string characterName;
    public bool inActiveParty;

    public PartyMember(string name, bool inActiveParty)
    {
        this.characterName = characterName;
        this.inActiveParty = inActiveParty;
    }
}

[CreateAssetMenu(fileName = "New Party Data", menuName = "Party")]
public class Party : ScriptableObject
{
    public List<PartyMember> partyList = new List<PartyMember>();
    public int activePartySize;

    public void AddPartyMember(string name, bool inActiveParty)
    {
        PartyMember newMember = new PartyMember(name, inActiveParty);
        partyList.Add(newMember);
    }

    public void RemovePartyMember(PartyMember partyMember)
    {
        partyList.Remove(partyMember);
    }

    public void SwapPartyMembers(string name1, string name2)
    {
    //     int index1 = allyList.IndexOf(name1);
    //     int index2 = allyList.IndexOf(name2);

    //     allyList[index1] = name2;
    //     allyList[index2] = name1;
    }
}
