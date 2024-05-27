using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TraitDisplay : MonoBehaviour
{
    public List<TraitIcon> Icons { get; private set; } = new List<TraitIcon>();
    [SerializeField] private GameObject iconPrefab;

    public void CreateList(List<Trait> traits)
    {
        ClearIcons();
        foreach (Trait trait in traits)
        {
            AddIcon(trait);
        }
    }

    public void AddIcon(Trait trait)
    {
        GameObject iconObject = Instantiate(iconPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        iconObject.transform.SetParent(this.transform, false);
        iconObject.GetComponent<TraitIcon>().AssignTrait(trait);
        Icons.Add(iconObject.GetComponent<TraitIcon>());
    }

    public void ClearIcons()
    {
        for (int i = Icons.Count - 1; i >= 0; i--)
        {
            TraitIcon thisIcon = Icons[i];
            Icons.Remove(thisIcon);
            Destroy(thisIcon.gameObject);
        }
    }

    public void ToggleInteractivity(bool isInteractive)
    {
        foreach (TraitIcon traitIcon in Icons)
        {
            traitIcon.ToggleButton(isInteractive);
        }
    }
}
