using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TraitDisplay : MonoBehaviour
{
    public List<TraitIcon> icons = new List<TraitIcon>();
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private TextMeshProUGUI infoName;
    [SerializeField] private TextMeshProUGUI infoText;

    public void CreateList(Combatant combatant)
    {
        ClearIcons();
        foreach (TraitInstance traitInstance in combatant.TraitInstances)
        {
            AddIcon(traitInstance);
        }
    }

    public void AddIcon(TraitInstance traitInstance)
    {
        GameObject iconObject = Instantiate(iconPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        iconObject.transform.SetParent(this.transform, false);
        iconObject.GetComponent<TraitIcon>().AssignTrait(traitInstance);
        icons.Add(iconObject.GetComponent<TraitIcon>());
    }

    public void ClearIcons()
    {
        for (int i = icons.Count - 1; i >= 0; i--)
        {
            TraitIcon thisIcon = icons[i];
            icons.Remove(thisIcon);
            Destroy(thisIcon.gameObject);
        }
    }

    public void SelectIcon(TraitIcon icon)
    {
        icon.OnSelect();
        infoName.text = icon.TraitInstance.Trait.TraitName;
        infoText.text = icon.TraitInstance.Trait.TraitInfo;
    }

    public void DeselectIcon(StatusIcon icon)
    {
        icon.OnDeselect();
        infoName.text = "";
        infoText.text = "";
    }
}
