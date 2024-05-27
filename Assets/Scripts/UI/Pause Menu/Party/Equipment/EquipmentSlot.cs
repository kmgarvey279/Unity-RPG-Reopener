using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    //PlayableCharacterInfo playableCharacterInfo;
    [field: Header("Equipment Type")]
    [field: SerializeField] public EquipmentType EquipmentType { get; private set; }
    [SerializeField] private Color assignedColor;
    [SerializeField] private Color emptyColor;
    [SerializeField] private Image panel;
    [SerializeField] private GameObject border;
    [SerializeField] private GameObject glow;
    private Button button;
    [Header("UI Display")]
    //[SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameValue;
    [field: Header("Equipment In Slot")]
    [SerializeField] public EquipmentItem EquipmentItem { get; private set; }
    //[SerializeField] private GameObject equipmentPopup;
    private PauseMenuStateEquip pauseMenuEquip;

    [SerializeField] private ItemInfo itemInfo;

    private void Awake()
    {
        button = GetComponent<Button>();
        pauseMenuEquip = GetComponentInParent<PauseMenuStateEquip>();
    }


    public void AssignSlot (EquipmentItem equipmentItem)
    {
        if (equipmentItem != null)
        {
            nameValue.text = equipmentItem.ItemName;
            panel.color = assignedColor;
        }
        else
        {
            nameValue.text = "-----";
            panel.color = emptyColor;

        }
        EquipmentItem = equipmentItem;
    }

    public void OnSelect(BaseEventData eventData)
    {
        //if (pauseMenuEquip != null)
        //{
        //    pauseMenuEquip.OnSelectEquipmentSlot(this);
        //}
        border.SetActive(true);

        itemInfo.DisplayItem(EquipmentItem);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        border.SetActive(false);
    }

    public void OnClick()
    {
        pauseMenuEquip.OnClickEquipmentSlot(this);
    }

    public void ToggleButton(bool isActive)
    {
        button.interactable = isActive;
    }

    //public void SetAsOpenSlot(bool isOpenSlot)
    //{
    //    glow.SetActive(isOpenSlot);
    //}

    ////remove equipment SO from this slot
    //public void ClearSlot()
    //{
    //    item = null;
    //    slotImage = emptyImage;
    //    nameText.text = emptyText; 
    //}

    ////bring up equipment change screen
    //public void DisplayEquipOptions()
    //{
    //    if(!equipmentPopup.activeInHierarchy)
    //    {
    //        equipmentPopup.SetActive(true);
    //    }
    //    EquipmentSelect equipmentSelect = equipmentPopup.GetComponent<EquipmentSelect>();
    //    equipmentSelect.PopulateList(this);
    //}
}
