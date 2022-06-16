using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CommandSubmenu : MonoBehaviour
{
    public bool isDisplayed;
    [SerializeField] private GameObject display; 
    [SerializeField] private GameObject skillSlotPrefab;
    [SerializeField] private GameObject listParent;
    [SerializeField] private ScrollRect scrollRect;
    private List<GameObject> selectableList = new List<GameObject>();

    private void Update() 
    {
        if(display.activeInHierarchy && Input.GetButtonDown("Cancel"))
        {
            Hide();
        }
        float moveY = Input.GetAxisRaw("Vertical");
        if(!Mathf.Approximately(moveY, 0.0f))
        {
            scrollRect.verticalNormalizedPosition = scrollRect.verticalNormalizedPosition + moveY;
        }
    }

    public void DisplayItems()
    {
        isDisplayed = true;
        display.SetActive(true);
    }

    public void DisplaySkills(PlayableCombatant combatant)
    {
        isDisplayed = true;
        for(int i = 0; i < combatant.skills.Count; i++)
        {
            GameObject skillSlotObject = Instantiate(skillSlotPrefab, Vector3.zero, Quaternion.identity);   
            skillSlotObject.transform.SetParent(listParent.transform, false);      
            BattleSkillSlot battleSkillSlot = skillSlotObject.GetComponent<BattleSkillSlot>();
            battleSkillSlot.AssignSlot(combatant.skills[i]); 
            selectableList.Add(skillSlotObject);
            if(combatant.skills[i].mpCost > combatant.mp.GetCurrentValue()
                || combatant.skills[i].costsHP && combatant.skills[i].mpCost > combatant.hp.GetCurrentValue())
            {
                skillSlotObject.GetComponent<Button>().interactable = false;
            }
        } 
        display.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectableList[0]);
    }

    public void Hide()
    {
        isDisplayed = false;
        display.SetActive(false);
        for(int i = 0; i < selectableList.Count; i++)
        {
            Destroy(selectableList[i]);
        }
        selectableList.Clear(); 
    }
}
