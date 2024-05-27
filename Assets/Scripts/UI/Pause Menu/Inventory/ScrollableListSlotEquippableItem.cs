using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquippableItemInstance
{
    public EquipmentItem EquipmentItem { get; private set; }
    public bool IsEquipped { get; private set; } = false;
    public PlayableCharacterID EquippedCharacterID { get; private set; }
    public int Count { get; private set; }

    public EquippableItemInstance(EquipmentItem equipmentItem, int count)
    {
        EquipmentItem = equipmentItem;
        Count = count;
    }
    public EquippableItemInstance(EquipmentItem equipmentItem, int count, PlayableCharacterID playableCharacterID)
    {
        EquipmentItem = equipmentItem;
        Count = count;
        IsEquipped = true;
        EquippedCharacterID = playableCharacterID;
    }
}

public class ScrollableListSlotEquippableItem : ScrollableListSlot
{
    [field: SerializeReference] public EquippableItemInstance EquippableItemInstance { get; private set; }

    [SerializeField] private TextMeshProUGUI nameValue;
    [SerializeField] private TextMeshProUGUI amountValue;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image characterIcon;
    [SerializeField] private SignalSenderGO onSelectEquipmentItem;
    [SerializeField] private SignalSenderGO onClickEquipmentItem;

    [SerializeField] private Image panel;
    [SerializeField] private Color emptyColor;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        onSelectEquipmentItem.Raise(this.gameObject);
    }

    public override void OnClick()
    {
        onClickEquipmentItem.Raise(this.gameObject);
    }

    public void AssignItem(EquippableItemInstance equippableItemInstance)
    {
        EquippableItemInstance = equippableItemInstance;

        if (equippableItemInstance == null)
        {
            nameValue.text = "------";
            panel.color = emptyColor;
            characterIcon.enabled = false;
            amountValue.text = "";
        }
        else
        {
            nameValue.text = equippableItemInstance.EquipmentItem.ItemName;
            itemIcon.sprite = equippableItemInstance.EquipmentItem.ItemIcon;
            if (equippableItemInstance.IsEquipped)
            {
                characterIcon.enabled = true;
                characterIcon.sprite = SaveManager.Instance.LoadedData.GetPlayableCombatantRuntimeData(equippableItemInstance.EquippedCharacterID).StaticInfo.TurnIcon;
                characterIcon.gameObject.SetActive(true);
                amountValue.text = "";
            }
            else
            {
                characterIcon.enabled = false;
                amountValue.text = "x" + equippableItemInstance.Count.ToString();
            }
        }
    }
}
