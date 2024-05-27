using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollableListSlotTrait : ScrollableListSlot
{
    public Trait Trait { get; private set; } 

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image icon;
    [SerializeField] private SignalSenderGO onSelectTrait;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        onSelectTrait.Raise(this.gameObject);
    }

    public void AssignTrait(Trait trait)
    {
        Trait = trait;
        nameText.text = trait.TraitName;
        icon.sprite = trait.Icon;
    }
}
