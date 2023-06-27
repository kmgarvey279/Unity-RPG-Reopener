using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq.Expressions;

public class BattlePartyPanel : MonoBehaviour
{
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

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void AssignCombatant(PlayableCombatant _playableCombatant)
    {
        playableCombatant = _playableCombatant;
        //set up name 
        characterName.SetText(playableCombatant.CharacterName);
        //set up icon
        icon.sprite = playableCombatant.TurnIcon;
        //set hp bar
        hpBar.SetInitialValue(playableCombatant.HP.GetValue(), playableCombatant.HP.CurrentValue);
        hpNum.SetText(playableCombatant.HP.CurrentValue.ToString());
        //set barrier 
        barrierBar.SetInitialValue(playableCombatant.Barrier.GetValue(), 0);
        //set mp bar
        mpBar.SetInitialValue(playableCombatant.MP.GetValue(), playableCombatant.MP.CurrentValue);
        mpNum.SetText(playableCombatant.MP.CurrentValue.ToString());
    }

    public void UpdateHP()
    {
        hpNum.SetText(playableCombatant.HP.CurrentValue.ToString());
        hpBar.DisplayChange(playableCombatant.HP.CurrentValue);
        barrierBar.DisplayChange(playableCombatant.Barrier.CurrentValue);
    }

    public void ResolveHP()
    {
        hpBar.ResolveChange(playableCombatant.HP.CurrentValue);
        barrierBar.ResolveChange(playableCombatant.Barrier.CurrentValue);
    }

    public void UpdateMP()
    {
        mpBar.DisplayChange(playableCombatant.MP.CurrentValue);
    }

    public void ResolveMP()
    {
        mpBar.ResolveChange(playableCombatant.MP.CurrentValue);
    }

    public void Highlight(bool isHighlighted)
    {
        if(isActive != isHighlighted)
        {
            if (isHighlighted)
            {
                panel.color = highlightedColor;
                animator.SetTrigger("Out");
            }
            else
            {
                panel.color = defaultColor;
                animator.SetTrigger("In");
            }
            isActive = isHighlighted;
        }
    }

    public void OnKO()
    {
        icon.material = greyscaleMaterial;
    }

    public void OnRevive()
    {
        icon.material = defaultMaterial;
    }
}
