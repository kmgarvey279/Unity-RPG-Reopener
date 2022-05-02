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
    public Tile targetedTile;
    public List<Combatant> targets;
    public bool flip;
    public TargetType targetType;
    public PotentialAction(Action action, int baseWeight, Tile targetedTile, List<Combatant> targets, bool flip)
    {
        this.action = action;
        this.baseWeight = baseWeight;
        this.targetedTile = targetedTile;
        this.targets = targets;
        this.flip = flip;
        this.targetType = TargetType.TargetPlayer;
        if(action.targetFriendly)
        {
            targetType = TargetType.TargetEnemy;
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
        SetActionPhase();
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

    private void SetActionPhase()
    {
        PotentialAction potentialAction = GetAction();
        battleManager.SetAction(potentialAction.action);
        battleManager.SetTargets(potentialAction.targetedTile, potentialAction.targets, potentialAction.targetType, potentialAction.flip);
        
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
            if(weightedAction.action.apCost > turnData.actionPoints)
            {
                break;
            }

            TargetType targetType = TargetType.TargetPlayer;
            List<Combatant> targets = battleManager.PlayableCombatants;
            if(weightedAction.action.targetFriendly)
            {
                targetType = TargetType.TargetEnemy;
                targets = battleManager.EnemyCombatants;
            }

            if(weightedAction.action.actionType == ActionType.Move)
            {

            }
            else
            {
                List<PotentialAction> tempPotentialActions = new List<PotentialAction>();
                //simulate aoe on each target, check total number of targets hit 
                foreach(Combatant target in targets)
                {
                    //is the selected target valid?
                    if(weightedAction.action.isFixedAOE || ActionCheck(weightedAction.action, target))
                    {
                        List<Combatant> targetsInAOE = GetTargets(weightedAction.action, target, targetType, false);
                        tempPotentialActions.Add(new PotentialAction(weightedAction.action, weightedAction.BaseWeight(), target.tile, targetsInAOE, false));
                        if(weightedAction.action.canFlip)
                        {
                            targetsInAOE = GetTargets(weightedAction.action, target, targetType, true);
                            tempPotentialActions.Add(new PotentialAction(weightedAction.action, weightedAction.BaseWeight(), target.tile, targetsInAOE, true));
                        }
                    }
                    if(weightedAction.action.isFixedAOE)
                    {
                        break;
                    }
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
        return new PotentialAction(wait, 0, thisEnemy.tile, new List<Combatant>(), false);
    }


    private List<Combatant> GetTargets(Action action, Combatant target, TargetType targetType, bool flip)
    {
        List<Tile> aoeTiles = gridManager.GetAOETiles(target.tile, action, targetType, flip);
        List<Combatant> targetsInAOE = gridManager.GetTargetsInAOE(aoeTiles, targetType);
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
        switch (action.actionType)
        {
        case ActionType.Attack:
            useAction = AttackCheck(action, target);
            break;
        case ActionType.Heal:
            useAction = HealCheck(action, target);
            break;
        case ActionType.AddStatusEffect:
            useAction = AddStatusEffectCheck(action, target);
            break;
        case ActionType.RemoveStatusEffects:
            useAction = RemoveStatusEffectsCheck(action, target);
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

    private List<Tile> MoveCheck(int direction)
    {
        int newColumn = turnData.combatant.tile.y + direction;
        List<Tile> unoccupiedTiles = new List<Tile>();
        Tile[,] tileArray = battleManager.gridManager.GetTileArray(TargetType.TargetEnemy);

        for(int i = 0; i < 2; i++)
        {
            Tile tile = tileArray[i, newColumn];
            if(!tile.occupier)
            {
                unoccupiedTiles.Add(tile);
            }
        }
        return unoccupiedTiles;
    }

    private bool AddStatusEffectCheck(Action action, Combatant target)
    {
        foreach(StatusEffectInstance statusEffectInstance in target.statusEffectInstances)
        {
            if(statusEffectInstance.statusEffectSO == action.statusEffectSO)
            {
                return false;
            }
        }
        return true;
    }

    private bool RemoveStatusEffectsCheck(Action action, Combatant target)
    {
        if(target.statusEffectInstances.Count > 0)
        {
            foreach(StatusEffectInstance statusEffectInstance in target.statusEffectInstances)
            {
                //if ally has a negative status effect...
                if(target is PlayableCombatant && statusEffectInstance.statusEffectSO.isBuff)
                {
                    return true;
                }
                //if playablable character had a positive status effect
                else if(target is EnemyCombatant && !statusEffectInstance.statusEffectSO.isBuff)
                {
                    return true;
                }
            }
        }
        return false;
    }
}

