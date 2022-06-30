using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StateMachineNamespace;
using System.Linq;

public class PotentialAction
{
    public Action action;
    public int baseWeight;
    public int weightModifiers = 0;
    public int cumulativeWeight;
    public Combatant primaryTarget;
    public List<Combatant> targets;
    public bool flip;
    public CombatantType combatantType;
    public PotentialAction(Action action, int baseWeight, Combatant primaryTarget, List<Combatant> targets)
    {
        this.action = action;
        this.baseWeight = baseWeight;
        this.primaryTarget = primaryTarget;
        this.targets = targets;
        this.combatantType = CombatantType.Player;
        if(action.targetingType == TargetingType.TargetFriendly)
        {
            combatantType = CombatantType.Enemy;
        }
    }
    public int GetWeight()
    {
        return baseWeight + weightModifiers;
    }
}

[System.Serializable]
public class EnemyTurnState : BattleState
{
    [SerializeField] private Action wait;
    // [SerializeField] GridManager gridManager;

    public override void OnEnter()
    {
        base.OnEnter();
        // onCameraZoomOut.Raise();
        StartCoroutine(SetActionCo());
    }

    public override void StateUpdate()
    {
        
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private IEnumerator SetActionCo()
    {
        EnemyCombatant thisEnemy = (EnemyCombatant)turnData.combatant;
        if(!turnData.hasMoved)
        {
            if(thisEnemy.tile != thisEnemy.preferredTile && thisEnemy.preferredTile.occupiers.Count < 3)
            {
                thisEnemy.ChangeTile(thisEnemy.preferredTile, "Move");
                yield return new WaitUntil(() => !turnData.combatant.moving);
            }
        }
        PotentialAction potentialAction = GetAction();
        battleManager.SetAction(potentialAction.action);
        battleManager.SetTargets(potentialAction.primaryTarget, potentialAction.targets);
    
        stateMachine.ChangeState((int)BattleStateType.Execute); 
    }

    public PotentialAction GetAction()
    { 
        EnemyCombatant thisEnemy = (EnemyCombatant)turnData.combatant;
        EnemyInfo enemyInfo = (EnemyInfo)thisEnemy.characterInfo;
        //get all enemy actions and use them to create a list of potential actions (with specific targets)
        List<PotentialAction> potentialActions = new List<PotentialAction>();
        foreach(WeightedAction weightedAction in enemyInfo.weightedActions)
        {
            CombatantType combatantType = CombatantType.Player;
            if(weightedAction.action.targetingType == TargetingType.TargetFriendly)
            {
                combatantType = CombatantType.Enemy;
            }
            List<Combatant> targets = battleManager.GetCombatants(combatantType);

            List<PotentialAction> tempPotentialActions = new List<PotentialAction>();
            //simulate aoe on each target, check total number of targets hit 
            foreach(Combatant target in targets)
            {
                //is the selected target valid?
                if(ActionCheck(weightedAction.action, target))
                {
                    List<Combatant> targetsInAOE = GetTargets(weightedAction.action, target, combatantType);
                    tempPotentialActions.Add(new PotentialAction(weightedAction.action, weightedAction.BaseWeight(), target, targetsInAOE));
                }
                if(tempPotentialActions.Count > 0)
                {
                    if(thisEnemy.optimizeTargetsInAOE)
                    {
                        tempPotentialActions.Sort((a,b)=>a.targets.Count.CompareTo(b.targets.Count));
                        potentialActions.Add(tempPotentialActions[0]);         
                    }
                    else
                    {
                        int roll = Mathf.FloorToInt(Random.Range(0, tempPotentialActions.Count));
                        potentialActions.Add(tempPotentialActions[roll]);         
                    }  
                } 
            }
        }
        int totalWeight = 0;
        if(potentialActions.Count > 0)
        {
            foreach(PotentialAction potentialAction in potentialActions)
            {
                int actionWeight = potentialAction.GetWeight();
                //if action was used one turn ago...
                if(thisEnemy.lastActions.Count > 0 && thisEnemy.lastActions[0] == potentialAction.action)
                {
                    actionWeight = Mathf.RoundToInt((float)actionWeight / 4f);
                }
                //if action was used two turns ago...
                if(thisEnemy.lastActions.Count > 1 && thisEnemy.lastActions[1] == potentialAction.action)
                {
                    actionWeight = Mathf.RoundToInt((float)actionWeight / 2f);
                }
                totalWeight += actionWeight;
                potentialAction.cumulativeWeight = totalWeight;
            }
            potentialActions = potentialActions.OrderBy(potentialAction => potentialAction.cumulativeWeight).ToList();
            int roll = Random.Range(1, totalWeight);
            for(int i = 0; i < potentialActions.Count; i++)
            {
                Debug.Log(potentialActions[i].action.actionName + " " + potentialActions[i].cumulativeWeight + " " + roll);
                if(roll <= potentialActions[i].cumulativeWeight)
                {
                    Debug.Log("action: " + potentialActions[i].action.actionName);
                    return potentialActions[i];
                }
            }
        }
        return new PotentialAction(wait, 0, thisEnemy, new List<Combatant>());
    }


    private List<Combatant> GetTargets(Action action, Combatant target, CombatantType combatantType)
    {
        List<Tile> aoeTiles = gridManager.GetAOETiles(target.tile, action, combatantType);
        List<Combatant> targetsInAOE = gridManager.GetTargetsInAOE(aoeTiles, combatantType);
        for(int i = targetsInAOE.Count - 1; i >= 0; i--)
        {
            //if target should not be targeted, remove it from aoe list
            if(!ActionCheck(action, targetsInAOE[i]))
            {
                targetsInAOE.RemoveAt(i);
            }
        }
        return targetsInAOE;
    }

    //checks if action is applicable to current situation
    private bool ActionCheck(Action action, Combatant target)
    {
        bool useAction = false;
        // switch (action.actionType)
        // {
        // case ActionType.Attack:
        //     useAction = AttackCheck(action, target);
        //     break;
        // case ActionType.Heal:
        //     useAction = HealCheck(action, target);
        //     break;
        // case ActionType.AddStatusEffect:
        //     useAction = AddStatusEffectCheck(action, target);
        //     break;
        // case ActionType.RemoveStatusEffects:
        //     useAction = RemoveStatusEffectsCheck(action, target);
        //     break;
        // case ActionType.Other:
        //     useAction = true;
        //     break;
        // default:
        //     useAction = false;
        //     break;
        // }
        return useAction;
    }

     private bool AttackCheck(Action action, Combatant target)
    {
        List<int> meleeRows = new List<int> {2, 1, 0};
        List<int> rowsInRange = meleeRows;
        for(int i = rowsInRange.Count - 1; i >= 0; i--)
        {
            if(i > turnData.combatant.tile.x)
            {
                rowsInRange.RemoveAt(i);
            }
        }
        if(!action.isMelee || rowsInRange.Contains(target.tile.x))
        {   
            return true;
        }
        return false;
    }

    private bool HealCheck(Action action, Combatant target)
    {
        //only heal if target at less than 75% of max hp
        if(target.hp.GetCurrentValue() < Mathf.FloorToInt((float)target.hp.GetValue() * 0.75f))
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    private bool AddStatusEffectCheck(Action action, Combatant target)
    {
        // foreach(StatusEffectInstance statusEffectInstance in target.statusEffectInstances)
        // {
        //     if(statusEffectInstance.statusEffectSO == action.statusEffectSO)
        //     {
        //         return false;
        //     }
        // }
        return true;
    }

    private bool RemoveStatusEffectsCheck(Action action, Combatant target)
    {
        // if(target.statusEffectInstances.Count > 0)
        // {
        //     foreach(StatusEffectInstance statusEffectInstance in target.statusEffectInstances)
        //     {
        //         if(target is PlayableCombatant && statusEffectInstance.statusEffectSO.isBuff)
        //         {
        //             return true;
        //         }
        //         else if(target is EnemyCombatant && !statusEffectInstance.statusEffectSO.isBuff)
        //         {
        //             return true;
        //         }
        //     }
        // }
        return false;
    }
}

