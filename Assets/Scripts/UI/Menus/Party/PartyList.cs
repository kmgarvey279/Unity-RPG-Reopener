using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyList : MonoBehaviour
{
    public Party partyData;
    public List<PartySlot> partySlots = new List<PartySlot>();
    public GameObject slotPrefab;

    private void OnEnable()
    {
        foreach (PartyMember partyMember in partyData.partyList)
        {
            GameObject newSlot = Instantiate(slotPrefab, transform.position, Quaternion.identity);
            PartySlot partySlot = newSlot.GetComponent<PartySlot>();
            partySlot.AssignSlot(partyMember);
            partySlot.transform.parent = gameObject.transform; 
            partySlots.Add(partySlot);
        }
    }
}
