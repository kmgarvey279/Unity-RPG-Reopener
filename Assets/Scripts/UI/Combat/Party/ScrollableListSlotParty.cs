using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollableListSlotParty : ScrollableListSlot
{
    [Header("Display")]
    [SerializeField] private Image icon;
    [SerializeField] private Image border;
    [SerializeField] private TextMeshProUGUI nameValue;
    [field: Header("Character Data")]
    public PlayableCombatant PlayableCombatant { get; private set; }
    [Header("Signals")]
    [SerializeField] private SignalSenderGO onSelectReserveCharacter;
    [SerializeField] private SignalSenderGO onClickReserveCharacter;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        onSelectReserveCharacter.Raise(this.gameObject);
    }

    public override void OnClick()
    {
        onClickReserveCharacter.Raise(this.gameObject);
    }

    public void AssignCharacter(PlayableCombatant playableCombatant)
    {
        PlayableCombatant = playableCombatant;
        nameValue.text = playableCombatant.CharacterName;
        icon.sprite = playableCombatant.TurnIcon;
        border.color = playableCombatant.CharacterColor;
    }
}
