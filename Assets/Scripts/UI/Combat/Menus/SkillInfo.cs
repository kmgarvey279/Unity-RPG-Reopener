using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;

    public void DisplayInfo(GameObject skillSlot)
    {
        descriptionText.text = skillSlot.GetComponent<BattleSkillSlot>().action.description;    
    }
}
