using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoPopupManager : MonoBehaviour
{
    [SerializeField] private GameObject vulnDisplay;

    [SerializeField] private GameObject display;
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private TextMeshProUGUI typeTMP;

    [SerializeField] private TextBox textBox;
    
    public void OnEnable()
    {
        Hide();
    }

    public void DisplayInfo(string name, string type, string description, string secondaryDescription)
    {
        ClearAll();

        vulnDisplay.SetActive(false);
        display.SetActive(true);

        nameTMP.SetText(name);
        typeTMP.SetText(type);
        textBox.SetText(description, secondaryDescription);

        //foreach (string addText in addTexts)
        //{
        //    GameObject textboxObject = Instantiate(textboxPrefab, new Vector3(0, 0), Quaternion.identity);
        //    textboxObject.transform.SetParent(textListParent.transform, false);
        //    addTextboxes.Add(textboxObject);

        //    TextMeshProUGUI tmp = textboxObject.GetComponentInChildren<TextMeshProUGUI>();
        //    if (tmp != null)
        //    {
        //        tmp.text = addText;
        //    }
        //}
    }

    //public void DisplayStatusInfo(List<StatusEffect> statusEffects)
    //{
    //    traitInfo.Clear();
    //    //statusInfo.DisplayStatusEffects(statusEffects);
    //}

    public void Hide()
    {
        ClearAll();

        display.SetActive(false);
        //partyStatus.SetActive(true);
        vulnDisplay.SetActive(true);
    }

    private void ClearAll()
    {
        nameTMP.SetText("");
        typeTMP.SetText("");
        textBox.Clear();
    }
}
