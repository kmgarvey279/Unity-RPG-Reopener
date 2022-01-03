using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyList : MonoBehaviour
{
    public PartyData partyData;
    public List<PartySlot> partySlots = new List<PartySlot>();
    public GameObject slotPrefab;

    private void OnEnable()
    {
        for(int i = 0; i < partyData.partyMembers.Count; i++)
        {
            if(partyData.partyMembers[i].inParty)
            {
                GameObject newSlot = Instantiate(slotPrefab, transform.position, Quaternion.identity);
                PartySlot partySlot = newSlot.GetComponent<PartySlot>();
                partySlot.AssignSlot(partyData.partyMembers[i], i);
                partySlot.transform.parent = gameObject.transform; 
                partySlots.Add(partySlot);
            }
        }
    }
}
