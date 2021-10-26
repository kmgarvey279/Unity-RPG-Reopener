using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionData
{
    public Action action;
    public Tile destinationTile;
    public Tile targetedTile;
    public List<Combatant> targets;
    public EnemyActionData(Action action, Tile startingTile)
    {
        this.action = action;
        this.destinationTile = startingTile;
        this.targetedTile = startingTile;
        this.targets = new List<Combatant>();
    }

}

public class EnemyCombatant : Combatant
{
    private Dictionary<Action, int> cooldownTimers = new Dictionary<Action, int>();
    private Combatant mostRecentAttacker;
    private Combatant tauntUser;
    //used when there are no actions an enemy can take
    [SerializeField] private Action wait;

    public override void Awake()
    {
        base.Awake();        
        foreach (Action action in skills)
        {
            cooldownTimers.Add(action, action.cooldown);
        }
        SetLookDirection(new Vector2(-1, 0));
    }

    public override void SetBattleStats(Dictionary<StatType, Stat> statDict)
    {
        battleStatDict.Add(BattleStatType.MeleeAttack, new Stat(statDict[StatType.Attack].GetValue() + level));
        battleStatDict.Add(BattleStatType.RangedAttack, new Stat(statDict[StatType.Attack].GetValue() + level));
        battleStatDict.Add(BattleStatType.MagicAttack, new Stat(statDict[StatType.Magic].GetValue() + level));

        battleStatDict.Add(BattleStatType.PhysicalDefense, new Stat(statDict[StatType.Defense].GetValue() + level));
        battleStatDict.Add(BattleStatType.MagicDefense, new Stat(statDict[StatType.MagicDefense].GetValue() + level));

        battleStatDict.Add(BattleStatType.Accuracy, new Stat(Mathf.FloorToInt(statDict[StatType.Skill].GetValue() + statDict[StatType.Agility].GetValue() / 2)));
        battleStatDict.Add(BattleStatType.Evasion, new Stat(Mathf.FloorToInt(statDict[StatType.Skill].GetValue() + statDict[StatType.Agility].GetValue() / 2)));

        battleStatDict.Add(BattleStatType.CritRate, new Stat(Mathf.FloorToInt(statDict[StatType.Skill].GetValue() / 3)));
        battleStatDict.Add(BattleStatType.Speed, new Stat(statDict[StatType.Agility].GetValue()));

        battleStatDict.Add(BattleStatType.MoveRange, new Stat(statDict[StatType.MoveRange].GetValue()));
    }

    public EnemyActionData GetActionData()
    {
        //set default action (wait)
        EnemyActionData actionData = new EnemyActionData(wait, tile);
        actionData.targetedTile = GetTargetedTile(wait, gridManager.GetTargetsInRange(tile, 99, true, false));
        actionData.destinationTile = gridManager.GetClosestTileInRange(tile, actionData.targetedTile, battleStatDict[BattleStatType.MoveRange].GetValue());

        //get all actions that can be used this turn
        List<Action> readyActions = GetReadyActions();
        //create dictionary of all possible targets for each action (and remove any with no targets)
        Dictionary<Action, List<Combatant>> targetsInRange = new Dictionary<Action, List<Combatant>>();
        foreach (Action action in readyActions)
        {
            int maxRange = battleStatDict[BattleStatType.MoveRange].GetValue() + action.range + action.aoe;
            List<Combatant> targetsTemp = gridManager.GetTargetsInRange(tile, maxRange, action.targetHostile, action.targetFriendly);
            targetsInRange.Add(action, targetsTemp);
        }
        //check if actions are applicable
        foreach (Action action in readyActions)
        {
            if(targetsInRange[action].Count > 0 && ActionCheck(action, targetsInRange[action]))
            {
                actionData.action = action;
                actionData.targetedTile = GetTargetedTile(action, targetsInRange[action]);
                actionData.destinationTile = gridManager.GetClosestTileInRange(tile, actionData.targetedTile, battleStatDict[BattleStatType.MoveRange].GetValue());
                actionData.targets = gridManager.GetTargetsInRange(actionData.targetedTile, action.aoe, action.targetHostile, action.targetFriendly);
                //reset cooldown timer
                cooldownTimers[action] = action.cooldown;
                break;
            }
        }
        return actionData;
    }

    public Tile GetTargetedTile(Action action, List<Combatant> targetsInRange)
    {
        if(action.targetHostile && tauntUser != null)
        {
            return tauntUser.tile;
        }
        Combatant selectedTarget = targetsInRange[0];
        int shortestDistance = gridManager.GetMoveCost(tile, targetsInRange[0].tile);
        foreach (Combatant target in targetsInRange)
        {
            int tempDistance = gridManager.GetMoveCost(tile, target.tile);
            if(tempDistance < shortestDistance)
            {
                selectedTarget = target;
                shortestDistance = tempDistance;
            }
            else if(tempDistance == shortestDistance)
            {
                if(target == mostRecentAttacker)
                {
                    selectedTarget = target;
                    shortestDistance = tempDistance;
                }
            }
        }
        return selectedTarget.tile;
    }

    private List<Action> GetReadyActions()
    {
        List<Action> readyActions = new List<Action>();
        //check cooldowns
        foreach (Action action in skills)
        {
            //check if action is on cooldown, if so decerase by 1
            if(cooldownTimers[action] > 0)
            {
                cooldownTimers[action] = cooldownTimers[action] - 1;
                //if action is now off cooldown, add to list of actions
                if(cooldownTimers[action] == 0)
                {
                    readyActions.Add(action);
                }
            }
            else
            {
                readyActions.Add(action);
            }
        }
        return readyActions;
    }

    //checks if action is applicable to current situation
    private bool ActionCheck(Action action, List<Combatant> targets)
    {
        bool useAction = false;
        switch (action.actionType)
        {
        case ActionType.Attack:
            useAction = AttackCheck(action, targets);
            break;
        case ActionType.Heal:
            useAction = HealCheck(action, targets);
            break;
        case ActionType.CureAilment:
            useAction = CureAilmentCheck(action, targets);
            break;
        case ActionType.Buff:
            useAction = BuffCheck(action, targets);
            break;
        case ActionType.Debuff:
            useAction = DebuffCheck(action, targets);
            break;
        case ActionType.Other:
            useAction = true;
            break;
        default:
            useAction = false;
            break;
        }
        return useAction;
    }

    private bool AttackCheck(Action attack, List<Combatant> targets)
    {
        if(tauntUser != null && !targets.Contains(tauntUser))
        {
            return false;
        }
        return true;
    }

    private bool HealCheck(Action heal, List<Combatant> targets)
    {
        bool validTarget = false;
        if(targets.Count > 0)
        {
            foreach (Combatant target in targets)
            {
                if(target.hp.GetCurrentValue() < Mathf.FloorToInt((float)target.hp.GetValue() / 4f))
                {
                    validTarget = true;
                }
            }
        }
        return validTarget;
    }

    private bool BuffCheck(Action buff, List<Combatant> targets)
    {
        bool validTarget = false;

        if(targets.Count > 0)
        {
            foreach (Combatant target in targets)
            {
                //if target is not currently buffed
            }
        }
        return validTarget;
    }

    private bool DebuffCheck(Action debuff, List<Combatant> targets)
    {
        bool validTarget = false;
        int maxRange = battleStatDict[BattleStatType.MoveRange].GetValue() + debuff.range + debuff.aoe;
        if(targets.Count > 0)
        {
            foreach (Combatant target in targets)
            {
                //if target is not currently debuffed
            }
        }
        //exclude attacks that can't reach taunt user if the enemy is taunted
        if(tauntUser != null && !targets.Contains(tauntUser))
        {
            validTarget = false;
        }
        return validTarget;
    }

    private bool CureAilmentCheck(Action cure, List<Combatant> targets)
    {
        bool validTarget = false;
        int maxRange = battleStatDict[BattleStatType.MoveRange].GetValue() + cure.range + cure.aoe;
        if(targets.Count > 0)
        {
            foreach (Combatant target in targets)
            {
                //if target is debuffed
            }
        }
        return validTarget;
    }

    // private Tile GetAOEPosition(Action action)
    // {
    //     int mostTargets = 0;
    //     int shortestRange = 99;
    //     Tile tileWithMostTargets;
    //     foreach (GameObject tileObject in battlefield.gridManager.tileArray)
    //     {
    //         Tile tileToCheck = tileObject.GetComponent<Tile>();
    //         //check all reachable squares (movement and attack ranged)
    //         if(battlefield.gridManager.GetMoveCost(tileToCheck, tile) <= (GetStatValue(StatType.MoveRange) + action.range))
    //         {
    //             List<Combatant> targetsInAOE = battlefield.gridManager.GetTargetsInRange(tile, action.aoe, action.targetPlayer, action.targetEnemy);
    //             //if aoe effects more or targets than in previous position...
    //             if(targetsInAOE.Count > mostTargets 
    //                 || targetsInAOE.Count == mostTargets && battlefield.gridManager.GetMoveCost(tileToCheck, tile) <= shortestRange)
    //             {
    //                 if(tauntUser == null || tauntUser != null && targetsInAOE.Contains(tauntUser))
    //                 {
    //                     mostTargets = targetsInAOE.Count;
    //                     shortestRange = battlefield.gridManager.GetMoveCost(tileToCheck, tile);
    //                     tileWithMostTargets = tileToCheck;
    //                 }
    //             }
    //         }
    //     }
    //     return tileWithMostTargets;
    // }
}
