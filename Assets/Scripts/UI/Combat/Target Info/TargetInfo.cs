using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.ParticleSystem;

public class TargetInfo : MonoBehaviour
{
    //private enum TargetDisplayState
    //{
    //    None,
    //    Traits,
    //    StatusEffects
    //}

    [SerializeField] private GameObject display;
    [SerializeField] private TextMeshProUGUI nameText;
    //status effects
    [SerializeField] private StatusEffectDisplay statusEffectDisplay;
    private List<StatusEffectInstance> targetStatusEffectInstances = new List<StatusEffectInstance>();
    //traits
    [SerializeField] private TraitDisplay traitDisplay;
    private List<Trait> targetTraits = new List<Trait>();

    [SerializeField] private WeaknessDisplay weaknessDisplay;

    //info boxes
    [SerializeField] private InfoPopupManager infoPopupManager;
    //private TargetDisplayState targetDisplayState; 
    //private int displayedItemIndex = 0;

    //private int displayedTabIndex = 0;

    private void OnEnable()
    {
        Hide();
    }

    public void Hide()
    {
        display.SetActive(false);
        infoPopupManager.Hide();
    }

    public void Select()
    {
        Debug.Log("target info active");

        traitDisplay.ToggleInteractivity(true);
        statusEffectDisplay.ToggleInteractivity(true);

        if (traitDisplay.Icons.Count > 0) 
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(traitDisplay.Icons[0].gameObject);
        }
        else if (statusEffectDisplay.Icons.Count > 0) 
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(statusEffectDisplay.Icons[0].gameObject);
        }
        else
        {
            Deselect();
        }
    }

    public void Deselect()
    {
        traitDisplay.ToggleInteractivity(false);
        statusEffectDisplay.ToggleInteractivity(false);

        infoPopupManager.Hide();
    }

    //public void CycleInfoType(int direction)
    //{
    //    int currentEnum = (int)targetDisplayState;
    //    int newEnum = currentEnum + direction;
    //    if (newEnum < 0)
    //        newEnum = System.Enum.GetValues(typeof(TargetDisplayState)).Length - 1;
    //    if (newEnum > System.Enum.GetValues(typeof(TargetDisplayState)).Length - 1)
    //        newEnum = 0;
    //    targetDisplayState = (TargetDisplayState)newEnum;

    //    DisplayInfoBox();
    //}

    //public void CycleDisplayedItem(int direction)
    //{
    //    int listCount = 0;
    //    if (targetDisplayState == TargetDisplayState.Traits)
    //    {
    //        listCount = targetTraitInstances.Count;
    //    }
    //    else if (targetDisplayState == TargetDisplayState.StatusEffects)
    //    {
    //        listCount = targetStatusEffectInstances.Count;
    //    }
    //    if (listCount > 0)
    //    {
    //        int newItemIndex = displayedItemIndex + direction;
    //        if (newItemIndex < 0)
    //            newItemIndex = listCount - 1;
    //        if (newItemIndex > listCount - 1)
    //            newItemIndex = 0;
    //        displayedItemIndex = newItemIndex;
            
    //        DisplayInfoBox();
    //    }
    //}

    //private void DisplayInfoBox()
    //{
    //    switch (targetDisplayState)
    //    {
    //        case TargetDisplayState.Traits:
    //            List<Trait> traits = new List<Trait>();
    //            if (targetTraitInstances.Count > displayedItemIndex)
    //                traits.Add(targetTraitInstances[displayedItemIndex].Trait);
    //            infoPopupManager.DisplayTraitInfo(traits);
    //            break;
    //        case TargetDisplayState.StatusEffects:
    //            List<StatusEffect> statusEffects = new List<StatusEffect>();
    //            if (targetStatusEffectInstances.Count > displayedItemIndex)
    //                statusEffects.Add(targetStatusEffectInstances[displayedItemIndex].StatusEffect);
    //            infoPopupManager.DisplayStatusInfo(statusEffects);
    //            break;
    //        default:
    //            break;
    //    }
    //}

    public void OnSelectCombatant(GameObject gameObject)
    {
        //display ui element
        if (!display.activeInHierarchy)
        {
            display.SetActive(true);
        }

        Combatant combatant = gameObject.GetComponent<Combatant>();
        if (combatant != null)
        {
            string combatantName = combatant.CharacterName;
            if (combatant.CharacterLetter != "")
            {
                combatantName += " (" + combatant.CharacterLetter + ")";
            }
            nameText.text = combatantName;
            
            //status effects
            targetStatusEffectInstances = new List<StatusEffectInstance>(combatant.StatusEffectInstances);
            statusEffectDisplay.CreateList(targetStatusEffectInstances);
            
            //traits + weaknesses
            if (combatant is EnemyCombatant)
            {
                targetTraits = new List<Trait>(combatant.Traits);
                traitDisplay.CreateList(targetTraits);

                EnemyCombatant enemyCombatant = (EnemyCombatant)combatant;
   
                weaknessDisplay.gameObject.SetActive(true);
                weaknessDisplay.DisplayWeaknesses(enemyCombatant.EnemyInfo); 
            }
            else
            {
                traitDisplay.ClearIcons();
                weaknessDisplay.gameObject.SetActive(false);
            }
        }
    }

    public void OnSelectTraitIcon(GameObject gameObject)
    {
        TraitIcon traitIcon = gameObject.GetComponent<TraitIcon>();
        if (traitIcon != null)
        {
            Trait trait = traitIcon.Trait;
            if (trait != null)
            {
                infoPopupManager.DisplayInfo(trait.TraitName, trait.TraitType, trait.Description, trait.SecondaryDescription);
            }
        }
    }

    public void OnSelectStatusIcon(GameObject gameObject)
    {
        StatusIcon statusIcon = gameObject.GetComponent<StatusIcon>();
        if (statusIcon != null) 
        {
            StatusEffect statusEffect = statusIcon.StatusEffectInstance.StatusEffect;
            if (statusEffect != null)
            {
                infoPopupManager.DisplayInfo(statusEffect.EffectName, statusEffect.StatusEffectType.ToString(), statusEffect.Description, statusEffect.SecondaryDescription);
            }
        }
    }
}
