using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PartySlotPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image slotImage;
    [SerializeField] private Color activePartyColor;
    [SerializeField] private Color reservePartyColor;
    [SerializeField] private SignalSender onActivePartyChange;

    public void AssignSlot(PlayableCharacterInfo playableCharacterInfo, int orderNum)
    {
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
