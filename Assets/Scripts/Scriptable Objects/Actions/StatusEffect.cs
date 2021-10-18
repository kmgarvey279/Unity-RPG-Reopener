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

[System.Serializable]
[CreateAssetMenu(fileName = "New Status Effect", menuName = "Status Effect")]
public class StatusEffect : ScriptableObject
{
    [Header("Effect info")]
    public bool isBuff;
    public string effectName;
    public string effectInfo;
    [Header("Icon + Animation")]
    public Image icon;
    public string animatorTrigger;
    [SerializeField] private GameObject effectObject;
    [Header("Duration")]
    [SerializeField] private int turnDuration;
    private int turnCounter;
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
        turnCounter = turnDuration;
    }

    public void OnTurnStart(Combatant combatant)
    {
        turnCounter--;
        Debug.Log("test test test");
        if(turnCounter <= 0)
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
        combatant.RemoveStatusEffect(this);
    }
}

