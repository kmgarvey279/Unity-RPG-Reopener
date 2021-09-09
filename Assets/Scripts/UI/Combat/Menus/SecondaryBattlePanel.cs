using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SecondaryBattlePanel : MonoBehaviour
{
    [SerializeField] private GameObject display; 
    [SerializeField] private GameObject skillSlotPrefab;
    [SerializeField] private GameObject listParent;
    private List<GameObject> selectableList = new List<GameObject>();

    public void DisplayItems()
    {
        display.SetActive(true);
    }

    public void DisplaySkills(Combatant combatant)
    {
        List<Action> skills = combatant.skills;
        display.SetActive(true);
        for (int i = 0; i < skills.Count; i++)
        {
            GameObject skillSlotObject = Instantiate(skillSlotPrefab, Vector3.zero, Quaternion.identity);   
            skillSlotObject.transform.SetParent(listParent.transform, false);      
            skillSlotObject.GetComponent<BattleSkillSlot>().AssignSlot(skills[i]); 
            selectableList.Add(skillSlotObject);
        } 
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectableList[0]);
    }

    public void Hide()
    {
        display.SetActive(false);
        selectableList.Clear(); 
    }
}
