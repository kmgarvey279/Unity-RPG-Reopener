using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq.Expressions;

public class BattlePartyPanel : MonoBehaviour
{
    private int index;
    //[Header("Character Icon")]
    //[SerializeField] private Image icon;
    [SerializeField] protected Material defaultMaterial;
    [SerializeField] protected Material greyscaleMaterial;
    [Header("Panel Display")]
    [SerializeField] private GameObject glow;
    [SerializeField] private GameObject activeBorder;
    [SerializeField] private Animator animator;
    [field: Header("Character Info")]
    public PlayableCombatant PlayableCombatant { get; private set; }
    [SerializeField] private OutlinedText characterName;
    [field: Header("Character Icon")]
    [SerializeField] private Image characterIcon;
    [SerializeField] private Image characterBorder;
    [Header("HP Bar")]
    [SerializeField] private OutlinedText hpNum;
    [SerializeField] private AnimatedBar hpBar;
    [SerializeField] private AnimatedBar barrierBar;
    [Header("MP Bar")]
    [SerializeField] private OutlinedText mpNum;
    [SerializeField] private AnimatedBar mpBar;
    //[Header("Guard Stacks")]
    //[SerializeField] private List<GameObject> guardSlots = new List<GameObject>();
    //[SerializeField] private List<GuardIcon> guardIcons = new List<GuardIcon>();
    [Header("Intervention")]
    [SerializeField] private InterventionPointsDisplay interventionPointsDisplay;
    [SerializeField] private List<InterventionReadyIcon> interventionIcons = new List<InterventionReadyIcon>();

    //[SerializeField] private OutlinedText interventionButtonPrompt;
    //private string button1 = "1";
    //private string button2 = "2";
    //private string button3 = "3";
    //private List<string> buttons = new List<string>();


    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }

    public void SetIndex(int _index)
    {
        index = _index;
        //interventionButtonPrompt.SetText(buttons[index]);
    }

    public void AssignCombatant(PlayableCombatant _playableCombatant)
    {
        PlayableCombatant = _playableCombatant;
        //set up name 
        characterName.SetText(PlayableCombatant.CharacterName);
        //set up icon
        characterIcon.sprite = PlayableCombatant.TurnIcon;
        //set hp bar
        hpBar.SetInitialValue(PlayableCombatant.MaxHP, PlayableCombatant.HP);
        hpNum.SetText(PlayableCombatant.HP.ToString());
        //set barrier 
        barrierBar.SetInitialValue(PlayableCombatant.MaxHP, 0);
        //set mp bar
        mpBar.SetInitialValue(PlayableCombatant.MaxMP, PlayableCombatant.MP);
        mpNum.SetText(PlayableCombatant.MP.ToString());

        interventionPointsDisplay.UpdatePoints(PlayableCombatant.IP, false, false);
    }

    public void DisplayChanges()
    {
        barrierBar.DisplayChange(PlayableCombatant.Barrier);
        barrierBar.ResolveChange(PlayableCombatant.Barrier);

        hpNum.SetText(PlayableCombatant.HP.ToString());
        hpBar.DisplayChange(PlayableCombatant.HP);

        mpNum.SetText(PlayableCombatant.MP.ToString());
        mpBar.DisplayChange(PlayableCombatant.MP);
    }

    public void ResolveChanges()
    {
        hpBar.ResolveChange(PlayableCombatant.HP);
        mpBar.ResolveChange(PlayableCombatant.MP);
    }

    public void DisplayInterventionChanges()
    {
        interventionPointsDisplay.UpdatePoints(PlayableCombatant.IP, PlayableCombatant.InterventionQueued, PlayableCombatant.InterventionSpent);
    }

    //public void UpdateGuard()
    //{
    //    for (int i = 0; i < guardIcons.Count; i++)
    //    {
    //        if (PlayableCombatant.Guard.CurrentValue > i)
    //        {
    //            guardIcons[i].GetComponent<GuardIcon>().ToggleActive(true);
    //        }
    //        else
    //        {
    //            guardIcons[i].GetComponent<GuardIcon>().ToggleActive(false);
    //        }
    //    }
    //}

    public void UpdateInterventions()
    {
        //for (int i = 0; i < interventionIcons.Count; i++)
        //{
        //    if (PlayableCombatant.AvailableInterventions.CurrentValue > i)
        //    {
        //        interventionIcons[i].GetComponent<InterventionReadyIcon>().ToggleActive(true);
        //    }
        //    else
        //    {
        //        interventionIcons[i].GetComponent<InterventionReadyIcon>().ToggleActive(false);
        //    }

        //    if (PlayableCombatant.QueuedInterventions.CurrentValue > i)
        //    {
        //        interventionIcons[i].GetComponent<InterventionReadyIcon>().ToggleQueued(true);
        //    }
        //    else
        //    {
        //        interventionIcons[i].GetComponent<InterventionReadyIcon>().ToggleQueued(false);
        //    }
        //}
    }


    public void OnTurnStart()
    {
        activeBorder.SetActive(true);
        //characterName.SetTextColor(highlightedColor);
        //panel.color = highlightedColor;
        //animator.SetTrigger("Out");
    }

    public void OnTurnEnd()
    {
        activeBorder.SetActive(false);
        //characterName.SetTextColor(new Color(0, 0, 0));
        //panel.color = defaultColor;
        //animator.SetTrigger("In");
    }

    public void OnTarget()
    {
        //if (!targetedBorder.activeInHierarchy)
        //{
            //targetedBorder.SetActive(true);
            //glow.SetActive(true);
        //}
    }

    public void OnUntarget()
    {
        //if (targetedBorder.activeInHierarchy)
        //{
        //    targetedBorder.SetActive(false);
            //glow.SetActive(false);
        //}
    }

    public void OnKO()
    {
        characterIcon.material = greyscaleMaterial;
        //triggerUnavailable.SetActive(true);
    }

    public void OnRevive()
    {
        characterIcon.material = defaultMaterial;
        //triggerUnavailable.SetActive(false);
    }

    //public void LockInterventionTriggerIcon(bool isLocked)
    //{
    //    triggerUnavailable.SetActive(isLocked);
    //}
}
