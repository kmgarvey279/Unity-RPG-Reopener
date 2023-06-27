using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyListUI : MonoBehaviour
{
    [SerializeField] private PartyData partyData;
    private List<PartySlotPanel> partySlotPanels = new List<PartySlotPanel>();
    [SerializeField] private GameObject slotPrefab;

    private void OnEnable()
    {
        for(int i = 0; i < partyData.PartySlots.Count; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, transform.position, Quaternion.identity);
            PartySlotPanel partySlotPanel = newSlot.GetComponent<PartySlotPanel>();
            partySlotPanel.AssignSlot(partyData.GetSlotInfo(i), i);
            partySlotPanel.transform.parent = gameObject.transform; 
            partySlotPanels.Add(partySlotPanel);
        }
    }
}
