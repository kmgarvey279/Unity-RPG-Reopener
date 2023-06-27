using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentSlot : MonoBehaviour
{
    //PlayableCharacterInfo playableCharacterInfo;
    //[Header("Equipment Type")]
    //public EquipmentType EquipmentType { get; private set; }
    //[SerializeField] private string emptyText;
    //[SerializeField] private Image emptyImage;
    //[Header("UI Display")]
    //[SerializeField] private Image slotImage;
    //[SerializeField] private TextMeshProUGUI nameText;
    //[Header("Equipment In Slot")]
    //[SerializeField] private Item item;
    //[SerializeField] private GameObject equipmentPopup;

    //private void Start()
    //{
    //    playableCharacterInfo = GetComponentInParent<CharacterInfoUI>().playableCharacterInfo;
    //    AssignSlot(playableCharacterInfo.equipmentDict[equipmentType]);
    //}

    ////assign equipment SO to this slot
    //public void AssignSlot(ItemObject newItem)
    //{
    //    if(newItem != null)
    //    {
    //        item = newItem;
    //        slotImage = item.icon;
    //        nameText.text = item.name;
    //    }
    //    else
    //    {
    //        slotImage = emptyImage;  
    //        nameText.text = emptyText;  
    //    }
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
