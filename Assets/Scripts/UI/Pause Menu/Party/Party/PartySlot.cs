using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PartySlot : MonoBehaviour
{
    public PartyData partyData;
    public PartyMember partyMember;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image slotImage;
    [SerializeField] private Color activePartyColor;
    [SerializeField] private Color reservePartyColor;
    [SerializeField] private SignalSender onActivePartyChange;

    public void AssignSlot(PartyMember newPartyMember, int orderNum)
    {
        partyMember = newPartyMember;
        if(orderNum < 3)
        {
            slotImage.color = activePartyColor;
        }
        else
        {
            slotImage.color = reservePartyColor;
        }
    }
}
