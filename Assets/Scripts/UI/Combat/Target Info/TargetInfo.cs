using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class TargetInfo : MonoBehaviour
{
    private enum TargetDisplayState
    {
        None,
        Traits,
        StatusEffects
    }

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TraitDisplay traitDisplay;
    [SerializeField] private StatusEffectDisplay statusEffectDisplay;
    [SerializeField] private GameObject display;
    [SerializeField] private GameObject statusDisplay;

    public void Hide()
    {
        display.SetActive(false);
    }

    public void CycleTraits(int direction)
    {

    }

    public void AlternateInfo()
    {

    }

    public void OnSelectCombatant(GameObject gameObject)
    {
        Combatant combatant = gameObject.GetComponent<Combatant>();
        if(combatant != null)
        {
            nameText.text = combatant.CharacterName + " " + combatant.CharacterLetter;
            statusEffectDisplay.CreateList(combatant);
            traitDisplay.CreateList(combatant);
            if (!display.activeInHierarchy)
            {
                display.SetActive(true);
            }
        }
    }
}
