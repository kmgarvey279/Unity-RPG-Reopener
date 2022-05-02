using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatant : Combatant
{
    public List<Action> lastActions = new List<Action>();
    public bool isBoss = false;
    public bool optimizeTargetsInAOE = false;

    public override void Awake()
    {
        base.Awake();   
        defaultDirection = new Vector2(-1, 0);
        SetDirection(defaultDirection);
    }

    public override void SetBattleStats()
    {
        battleStatDict.Add(BattleStatType.Attack, new Stat(characterInfo.statDict[StatType.Attack].GetValue() + level + 5));
        battleStatDict.Add(BattleStatType.Defense, new Stat(characterInfo.statDict[StatType.Defense].GetValue() + level + 5));

        battleStatDict.Add(BattleStatType.MagicAttack, new Stat(characterInfo.statDict[StatType.MagicAttack].GetValue() + level + 5));
        battleStatDict.Add(BattleStatType.MagicDefense, new Stat(characterInfo.statDict[StatType.MagicDefense].GetValue() + level + 5));

        battleStatDict.Add(BattleStatType.CritRate, new Stat(Mathf.FloorToInt(characterInfo.statDict[StatType.Skill].GetValue() / 3)));
        battleStatDict.Add(BattleStatType.Accuracy, new Stat(Mathf.FloorToInt(characterInfo.statDict[StatType.Skill].GetValue() / 2)));

        battleStatDict.Add(BattleStatType.Speed, new Stat(characterInfo.statDict[StatType.Agility].GetValue()));
        battleStatDict.Add(BattleStatType.Evasion, new Stat(Mathf.FloorToInt(characterInfo.statDict[StatType.Agility].GetValue() / 2)));
    }

    public override IEnumerator KOCo()
    {
        animator.SetTrigger("KO");
        yield return new WaitForSeconds(0.5f);
        tile.UnassignOccupier();
        Destroy(this.gameObject);
    }
}
