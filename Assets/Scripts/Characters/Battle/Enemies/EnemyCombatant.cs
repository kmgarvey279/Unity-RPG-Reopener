using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatant : Combatant
{
    public Tile preferredTile;
    public List<Action> lastActions = new List<Action>();
    public bool isBoss = false;
    public bool optimizeTargetsInAOE = false;

    public override void Awake()
    {
        base.Awake();
        combatantType = CombatantType.Enemy;   
        defaultDirection = new Vector2(-1, 0);
        SetDirection(defaultDirection);
    }

    public override void SetCharacterData(CharacterInfo characterInfo)
    {
        base.SetCharacterData(characterInfo);

        battleStatDict.Add(BattleStatType.Attack, new Stat(characterInfo.statDict[StatType.Attack].GetValue() + level + 5));
        battleStatDict.Add(BattleStatType.Defense, new Stat(characterInfo.statDict[StatType.Defense].GetValue() + level + 5));

        battleStatDict.Add(BattleStatType.MagicAttack, new Stat(characterInfo.statDict[StatType.MagicAttack].GetValue() + level + 5));
        battleStatDict.Add(BattleStatType.MagicDefense, new Stat(characterInfo.statDict[StatType.MagicDefense].GetValue() + level + 5));
        
        isLoaded = true;
    }

    public override IEnumerator KOCo()
    {
        animator.SetTrigger("KO");
        yield return new WaitForSeconds(0.5f);
        tile.UnassignOccupier(this);
        Destroy(this.gameObject);
    }
}
