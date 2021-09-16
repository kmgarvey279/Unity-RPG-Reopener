using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentSlot : MonoBehaviour
{
    PlayableCharacterInfo playableCharacterInfo;
    [Header("Equipment Type")]
    public EquipmentType equipmentType;
    [SerializeField] private string emptyText;
    [SerializeField] private Sprite emptySprite;
    [Header("UI Display")]
    [SerializeField] private Image slotImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [Header("Equipment In Slot")]
    [SerializeField] private ItemObject item;
    [SerializeField] private GameObject equipmentPopup;

    private void Start()
    {
        playableCharacterInfo = GetComponentInParent<CharacterInfoUI>().playableCharacterInfo;
        AssignSlot(playableCharacterInfo.equipmentDict[equipmentType]);
    }

    //assign equipment SO to this slot
    public void AssignSlot(ItemObject newItem)
    {
        if(newItem != null)
        {
            item = newItem;
            slotImage.sprite = item.icon;
            nameText.text = item.name;
        }
        else
        {
            slotImage.sprite = emptySprite;  
            nameText.text = emptyText;  
        }
    }

    //remove equipment SO from this slot
    public void ClearSlot()
    {
        item = null;
        slotImage.sprite = emptySprite;
        nameText.text = emptyText; 
    }

    //bring up equipment change screen
    public void DisplayEquipOptions()
    {
        if(!equipmentPopup.activeInHierarchy)
        {
            equipmentPopup.SetActive(true);
        }
        EquipmentSelect equipmentSelect = equipmentPopup.GetComponent<EquipmentSelect>();
        equipmentSelect.PopulateList(this);
    }
}
