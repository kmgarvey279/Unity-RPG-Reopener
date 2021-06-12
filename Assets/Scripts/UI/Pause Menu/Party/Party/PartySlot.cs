using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PartySlot : MonoBehaviour
{
    public Party partyData;
    public PartyMember partyMember;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image slotImage;
    [SerializeField] private Color activePartyColor;
    [SerializeField] private Color reservePartyColor;
    [SerializeField] private SignalSender onActivePartyChange;

    public void AssignSlot(PartyMember newPartyMember)
    {
        partyMember = newPartyMember;
        nameText.text = partyMember.name;
        if(partyMember.inActiveParty)
        {
            slotImage.color = activePartyColor;
        }
        else
        {
            slotImage.color = reservePartyColor;
        }
    }

    public void ChangeActiveParty()
    {
        if(partyMember.name != "Claire")
        {
            if(partyMember.inActiveParty)
            {
                partyMember.inActiveParty = false;
                slotImage.color = reservePartyColor;
                partyData.activePartySize--;
            }
            else
            {
                if(partyData.activePartySize < 3)
                {
                    partyMember.inActiveParty = true;
                    slotImage.color = activePartyColor;
                    partyData.activePartySize++;
                }
            }
            onActivePartyChange.Raise();
        } 
    }
}
