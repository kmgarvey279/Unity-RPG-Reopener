using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq.Expressions;

public class BattlePartyPanel : MonoBehaviour
{
    private int index;
    [Header("Character Icon")]
    [SerializeField] private Image icon;
    [SerializeField] protected Material defaultMaterial;
    [SerializeField] protected Material greyscaleMaterial;
    [Header("Panel Display")]
    private bool isActive = false;
    [SerializeField] private Image panel;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color highlightedColor;
    [SerializeField] private Animator animator;
    [Header("Character Info")]
    private PlayableCombatant playableCombatant;
    [SerializeField] private OutlinedText characterName;
    [Header("HP Bar")]
    [SerializeField] private OutlinedText hpNum;
    [SerializeField] private AnimatedBar hpBar;
    [SerializeField] private AnimatedBar barrierBar;
    [Header("MP Bar")]
    [SerializeField] private OutlinedText mpNum;
    [SerializeField] private AnimatedBar mpBar;
    [Header("Intervention Trigger")]
    [SerializeField] private GameObject triggerAvailable;
    [SerializeField] private OutlinedText availableButtonPrompt;
    [SerializeField] private GameObject triggerUnavailable;
    [SerializeField] private OutlinedText unavailableButtonPrompt;
    private string button1 = "1";
    private string button2 = "2";
    private string button3 = "3";
    private List<string> buttons = new List<string>();
    [Header("Linked Party Member")]
    [SerializeField] private BattlePartyPanel linkPanel;

    private void OnEnable()
    {
        buttons.Add(button1);
        buttons.Add(button2);
        buttons.Add(button3);
        animator = GetComponent<Animator>();
    }

    public void SetIndex(int _index)
    {
        index = _index;
        availableButtonPrompt.SetText(buttons[index]);
        unavailableButtonPrompt.SetText(buttons[index]);
    }

    public void AssignCombatant(PlayableCombatant _playableCombatant)
    {
        playableCombatant = _playableCombatant;
        //set up name 
        characterName.SetText(playableCombatant.CharacterName);
        //set up icon
        icon.sprite = playableCombatant.TurnIcon;
        //set hp bar
        hpBar.SetInitialValue(playableCombatant.HP.MaxValue, playableCombatant.HP.Value);
        hpNum.SetText(playableCombatant.HP.Value.ToString());
        //set barrier 
        barrierBar.SetInitialValue(playableCombatant.Barrier.MaxValue, 0);
        //set mp bar
        mpBar.SetInitialValue(playableCombatant.MP.MaxValue, playableCombatant.MP.Value);
        mpNum.SetText(playableCombatant.MP.Value.ToString());
    }

    public void UpdateHP()
    {
        hpNum.SetText(playableCombatant.HP.Value.ToString());
        hpBar.DisplayChange(playableCombatant.HP.Value);
        barrierBar.DisplayChange(playableCombatant.Barrier.Value);
    }

    public void ResolveHP()
    {
        hpBar.ResolveChange(playableCombatant.HP.Value);
        barrierBar.ResolveChange(playableCombatant.Barrier.Value);
    }

    public void UpdateMP()
    {
        mpBar.DisplayChange(playableCombatant.MP.Value);
    }

    public void ResolveMP()
    {
        mpBar.ResolveChange(playableCombatant.MP.Value);
    }

    public void Highlight(bool isHighlighted)
    {
        if(isActive != isHighlighted)
        {
            if (isHighlighted)
            {
                panel.color = highlightedColor;
                //animator.SetTrigger("Out");
            }
            else
            {
                panel.color = defaultColor;
                //animator.SetTrigger("In");
            }
            isActive = isHighlighted;
        }
    }

    public void OnKO()
    {
        icon.material = greyscaleMaterial;
        triggerUnavailable.SetActive(true);
    }

    public void OnRevive()
    {
        icon.material = defaultMaterial;
        triggerUnavailable.SetActive(false);
    }

    public void LockInterventionTriggerIcon(bool isLocked)
    {
        triggerUnavailable.SetActive(isLocked);
    }
}
