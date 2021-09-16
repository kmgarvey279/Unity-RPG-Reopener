using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BattleStatMultiplier
{
    public BattleStatType statToMultiply;
    public float multiplier;
}

public class StatusEffect : ScriptableObject
{
    [Header("Effect info")]
    public bool isBuff;
    public string effectName;
    public string effectInfo;
    [Header("Icon + Animation")]
    public Image icon;
    [SerializeField] private GameObject effectObject;
    [Header("Duration")]
    [SerializeField] private int turnDuration;
    [SerializeField] private int turnCounter = 0;
    [Header("Effects")]
    [SerializeField] private List<BattleStatMultiplier> battleStatMultipliers = new List<BattleStatMultiplier>();
    [SerializeField] private bool changeHealth;
    [SerializeField] private int healthChangeAmount;
    [SerializeField] private bool endOnHit;

    public void ApplyStatusEffect(Combatant combatant)
    {
        foreach (BattleStatMultiplier statmultiplier in battleStatMultipliers)
        {
            Stat stat = combatant.battleStatDict[statmultiplier.statToMultiply];
            stat.AddMultiplier(statmultiplier.multiplier);   
        }
    }

    public void OnTurnStart(Combatant combatant)
    {
        turnCounter++;
        if(turnCounter >= turnDuration)
        {
            RemoveStatusEffect(combatant);
        }
        else 
        {
            if(changeHealth)
            {
                //spawn animation effect
                combatant.ChangeHealth(healthChangeAmount);
            }
        }
    }

    public void OnHit(Combatant combatant)
    {
        if(endOnHit)
        {
            RemoveStatusEffect(combatant);
        }
    }

    public void RemoveStatusEffect(Combatant combatant)
    {
        foreach (BattleStatMultiplier statmultiplier in battleStatMultipliers)
        {
            Stat stat = combatant.battleStatDict[statmultiplier.statToMultiply];
            stat.RemoveMultiplier(statmultiplier.multiplier);   
        }
    }
}

