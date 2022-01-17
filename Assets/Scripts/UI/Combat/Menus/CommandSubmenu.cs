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

    public void DisplaySkills(List<Action> skills)
    {
        isDisplayed = true;
        for (int i = 0; i < skills.Count; i++)
        {
            GameObject skillSlotObject = Instantiate(skillSlotPrefab, Vector3.zero, Quaternion.identity);   
            skillSlotObject.transform.SetParent(listParent.transform, false);      
            skillSlotObject.GetComponent<BattleSkillSlot>().AssignSlot(skills[i]); 
            selectableList.Add(skillSlotObject);
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
