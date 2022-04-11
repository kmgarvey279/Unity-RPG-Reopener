using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatant : Combatant
{
    [SerializeField] private EnemyBattleAI enemyBattleAI;

    public override void Awake()
    {
        base.Awake();   
        enemyBattleAI = GetComponentInChildren<EnemyBattleAI>();
        SetDirection(new Vector2(-1, 0));
    }

    public override void SetBattleStats()
    {
        battleStatDict.Add(BattleStatType.MeleeAttack, new Stat(characterInfo.statDict[StatType.Attack].GetValue() + level + 5));
        battleStatDict.Add(BattleStatType.RangedAttack, new Stat(characterInfo.statDict[StatType.Attack].GetValue() + level + 5));
        battleStatDict.Add(BattleStatType.MagicAttack, new Stat(characterInfo.statDict[StatType.Magic].GetValue() + level + 5));

        battleStatDict.Add(BattleStatType.PhysicalDefense, new Stat(characterInfo.statDict[StatType.Defense].GetValue() + level + 5));
        battleStatDict.Add(BattleStatType.MagicDefense, new Stat(characterInfo.statDict[StatType.MagicDefense].GetValue() + level + 5));

        battleStatDict.Add(BattleStatType.Accuracy, new Stat(Mathf.FloorToInt(characterInfo.statDict[StatType.Skill].GetValue() + characterInfo.statDict[StatType.Agility].GetValue() / 2)));
        battleStatDict.Add(BattleStatType.Evasion, new Stat(Mathf.FloorToInt(characterInfo.statDict[StatType.Skill].GetValue() + characterInfo.statDict[StatType.Agility].GetValue() / 2)));

        battleStatDict.Add(BattleStatType.CritRate, new Stat(Mathf.FloorToInt(characterInfo.statDict[StatType.Skill].GetValue() / 3)));
        battleStatDict.Add(BattleStatType.Speed, new Stat(characterInfo.statDict[StatType.Agility].GetValue()));

        battleStatDict.Add(BattleStatType.MoveRange, new Stat(characterInfo.statDict[StatType.MoveRange].GetValue()));
    }

    public void CreateAggroList(List<Combatant> targets)
    {
        foreach(Combatant target in targets)
        {
            AddTarget(target);
        }
    }

    public void AddTarget(Combatant combatant)
    {
        enemyBattleAI.AddTargetToAggroList(combatant);
    }

    public void RemoveTarget(Combatant combatant)
    {
        enemyBattleAI.RemoveTargetFromAggroList(combatant);
    }

    public void UpdateAggro(Combatant combatant, int aggroChange)
    {
        enemyBattleAI.UpdateAggro(combatant, aggroChange);
    }

    public override void EvadeAttack(Combatant attacker)
    {
        base.EvadeAttack(attacker);
        enemyBattleAI.UpdateAggro(attacker, attacker.level * 10);
    }

    public override void Damage(int damage, Combatant attacker, bool isCrit)
    {
        base.Damage(damage, attacker, isCrit);
        enemyBattleAI.UpdateAggro(attacker, damage);
    }

    public override void OnTurnEnd()
    {
        enemyBattleAI.TriggerAggroFalloff();
        base.OnTurnEnd();
    }

    public PotentialAction GetTurnAction()
    {
        return enemyBattleAI.GetTurnAction();
    }

    public override IEnumerator KO()
    {
        animator.SetTrigger("KO");
        yield return new WaitForSeconds(1f);
        tile.UnassignOccupier();
        Destroy(this.gameObject);
    }
}
