using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PotentialAction
{
    public Action action;
    public int baseWeight;
    public int totalAggro;
    public int cumulativeWeight;
    public Tile startingTile;
    public Tile targetedTile;
    public Tile destinationTile;
    public List<Combatant> targets;
    public PotentialAction(Action action, int baseWeight, int aggroTotal, Tile startingTile, Tile destinationTile, Tile targetedTile, List<Combatant> targets)
    {
        this.action = action;
        this.baseWeight = baseWeight;
        this.totalAggro = totalAggro;
        this.startingTile = startingTile;
        this.destinationTile = destinationTile;
        this.targetedTile = targetedTile;
        this.targets = targets;
    }

    public int GetWeight()
    {
        return baseWeight + totalAggro;
    }
}

public class EnemyBattleAI : MonoBehaviour
{
    private EnemyCombatant thisEnemy;
    private BattleManager battleManager;
    private Dictionary<Combatant, int> aggroDict = new Dictionary<Combatant, int>();
    //actions taken during previous two turns. decrease chance of being used again
    private List<Action> lastActions = new List<Action>();
    //used when there are no actions an enemy can take
    [SerializeField] private Action wait;

    private void Start()
    {
        thisEnemy = GetComponentInParent<EnemyCombatant>();
        battleManager = GetComponentInParent<BattleManager>();
    }

    public void AddTargetToAggroList(Combatant target)
    {
        int startingAggro = 1;
        if(aggroDict.Count > 0)
        {
            startingAggro = GetAggroRange()[0];
        }
        aggroDict.Add(target, startingAggro);
    }

    public void RemoveTargetFromAggroList(Combatant target)
    {
        aggroDict.Remove(target);
    }

    public void PutTargetOnTopOfAggroList(Combatant target)
    {
        int highestAggro = 1;
        if(aggroDict.Count > 0)
        {
            highestAggro = GetAggroRange()[1];
        }
        aggroDict[target] = highestAggro + Mathf.CeilToInt((float)highestAggro / 4f);
    }

    public void UpdateAggro(Combatant combatant, int aggroChange)
    {
        aggroDict[combatant] += aggroChange;
    }

    private int[] GetAggroRange()
    {
        int min = 0;
        int max = 0;
        foreach(KeyValuePair<Combatant, int> aggroItem in aggroDict)
        {
            if(aggroItem.Value > min || min == 0)
            {
                min = aggroItem.Value;
            }
            if(aggroItem.Value < max)
            {
                max = aggroItem.Value;
            }
        }
        return new int[2]{min, max};
    }

    //reduce aggro by 50% at the end of each turn
    public void TriggerAggroFalloff()
    {
        foreach(KeyValuePair<Combatant, int> aggroItem in aggroDict)
        {
            aggroDict[aggroItem.Key] = Mathf.CeilToInt((float)aggroItem.Value / 2); 
        }
    }

    public PotentialAction GetTurnAction()
    { 
        //get all enemy actions and use them to create a list of potential actions (with specific targets)
        // List<PotentialAction> potentialActions = new List<PotentialAction>();
        // EnemyInfo enemyInfo = (EnemyInfo)thisEnemy.characterInfo;
        // foreach(WeightedAction weightedAction in enemyInfo.weightedActions)
        // {
        //     //get all walkable tiles
        //     List<Tile> walkableTiles = battleManager.gridManager.GetTilesInRange(thisEnemy.tile, thisEnemy.battleStatDict[BattleStatType.MoveRange].GetValue(), true, false);
        //     Dictionary<Tile, List<Tile>> tileDict = new Dictionary<Tile, List<Tile>>();
        //     //get all targetable tiles within range of each walkable tile
        //     foreach(Tile walkableTile in walkableTiles)
        //     {
        //         tileDict.Add(walkableTile, battleManager.gridManager.GetTilesInRange(thisEnemy.tile, weightedAction.action.range, false, false));
        //     }
        //     //get targets within aoe for each tile, save tile w/ most targets
        //     Tile tileToTarget = thisEnemy.tile;
        //     Tile tileToMoveTo = thisEnemy.tile;
        //     int highestTotalAggro = 0;
        //     List<Combatant> targets = new List<Combatant>();
            
        //     Dictionary<Tile, List<Combatant>> checkedTiles = new Dictionary<Tile, List<Combatant>>();
        //     Dictionary<Combatant, bool> checkedTargets = new Dictionary<Combatant, bool>();
        //     foreach(KeyValuePair<Tile, List<Tile>> tileEntry in tileDict)
        //     {
        //         List<Combatant> targetsTemp = new List<Combatant>();
        //         //get all targets in aoe range
        //         foreach(Tile tileInRange in tileEntry.Value)
        //         {
        //             List<Combatant> targetsInAOE = new List<Combatant>();
        //             //if the tile has already been checked, use existing target list
        //             if(checkedTiles.ContainsKey(tileInRange))
        //             {
        //                 targetsInAOE = checkedTiles[tileInRange];
        //             }
        //             //otherwise, generate new list
        //             else
        //             {
        //                 targetsInAOE = battleManager.gridManager.GetTargetsInRange(tileInRange, weightedAction.action.aoe, weightedAction.action.targetHostile, weightedAction.action.targetFriendly);                
        //                 //filter list, add checked combatants to dict
        //                 for(int i = targetsInAOE.Count - 1; i >= 0; i--)
        //                 {
        //                     //if target has not yet been checked, check it & add it to list
        //                     if(!checkedTargets.ContainsKey(targetsInAOE[i]))
        //                     {
        //                         bool actionCheck = ActionCheck(weightedAction.action, targetsInAOE[i]);
        //                         checkedTargets.Add(targetsInAOE[i], actionCheck);
        //                     }
        //                     //if target should not be targeted, remove it from aoe list
        //                     if(checkedTargets[targetsInAOE[i]] == false)
        //                     {
        //                         targetsInAOE.RemoveAt(i);
        //                     } 
        //                 }
        //             }
        //             //if aoe includes more targets than previous highest target count or count is the same but the tile is closer
        //             int aggroTemp = 0;
        //             foreach(Combatant target in targetsInAOE)
        //             {
        //                 if(target is EnemyCombatant)
        //                 {
        //                     aggroTemp += target.level * 10;
        //                 }
        //                 else
        //                 {
        //                     aggroTemp += aggroDict[target];
        //                 }
        //             }
        //             if(aggroTemp > highestTotalAggro || aggroTemp == highestTotalAggro && battleManager.gridManager.GetMoveCost(thisEnemy.tile, tileEntry.Key) < battleManager.gridManager.GetMoveCost(thisEnemy.tile, tileToMoveTo))
        //             {
        //                 tileToTarget = tileInRange;
        //                 tileToMoveTo = tileEntry.Key;
        //                 targets = targetsInAOE;
        //                 highestTotalAggro = aggroTemp;
        //             }
        //         }
        //     }
        //     if(highestTotalAggro > 0)
        //     {
        //         potentialActions.Add(new PotentialAction(weightedAction.action, weightedAction.BaseWeight(), highestTotalAggro, thisEnemy.tile, tileToMoveTo, tileToTarget, targets));
        //     }
        // }
        // //roll on weighted potential actions
        // int totalWeight = 0;
        // if(potentialActions.Count > 0)
        // {
        //     foreach(PotentialAction potentialAction in potentialActions)
        //     {
        //         int actionWeight = potentialAction.GetWeight();
        //         //if action was used one turn ago...
        //         if(lastActions.Count > 0 && lastActions[0] == potentialAction.action)
        //         {
        //             actionWeight = Mathf.RoundToInt((float)actionWeight / 4f);
        //         }
        //         //if action was used two turns ago...
        //         if(lastActions.Count > 1 && lastActions[1] == potentialAction.action)
        //         {
        //             actionWeight = Mathf.RoundToInt((float)actionWeight / 2f);
        //         }
        //         totalWeight += actionWeight;
        //         potentialAction.cumulativeWeight = totalWeight;
        //     }
        //     potentialActions = potentialActions.OrderBy(potentialAction => potentialAction.cumulativeWeight).ToList();
        //     int roll = Random.Range(1, totalWeight);
        //     for(int i = 0; i < potentialActions.Count; i++)
        //     {
        //         Debug.Log(potentialActions[i].action.actionName + " " + potentialActions[i].cumulativeWeight + " " + roll);
        //         if(roll <= potentialActions[i].cumulativeWeight)
        //         {
        //             Debug.Log("action: " + potentialActions[i].action.actionName);
        //             return potentialActions[i];
        //         }
        //     }
        // }
        Debug.Log("action: wait");
        Tile destinationTemp = thisEnemy.gridManager.GetClosestTileInRange(thisEnemy.tile, GetRandomTarget(true).tile, thisEnemy.battleStatDict[BattleStatType.MoveRange].GetValue());
        return new PotentialAction(wait, 0, 0, thisEnemy.tile, destinationTemp, destinationTemp, new List<Combatant>());
    }

    private Combatant GetRandomTarget(bool weighBasedOnAggro)
    {
        int totalWeight = 0;
        if(aggroDict.Count > 1)
        {
            Dictionary<int, Combatant> weightDict = new Dictionary<int, Combatant>();
            foreach(KeyValuePair<Combatant, int> aggroEntry in aggroDict)
            {
                int targetWeight = 1;
                if(weighBasedOnAggro)
                {
                    targetWeight = aggroEntry.Value;
                } 
                totalWeight += targetWeight;
                weightDict.Add(targetWeight, aggroEntry.Key);
            }
            List<int> weightList = weightDict.Keys.ToList();
            weightList.Sort();
            int roll = Random.Range(1, totalWeight);
            for(int i = 0; i < weightList.Count; i++)
            {
                if(roll <= weightList[i])
                {
                    return weightDict[i];
                }
            }
        }
        return aggroDict.FirstOrDefault().Key;
    }

        //checks if action is applicable to current situation
    private bool ActionCheck(Action action, Combatant target)
    {
        // if(target.ko)
        // {
        //     return false;
        // }
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
        return true;
    }

    private bool HealCheck(Action action, Combatant target)
    {
        //only heal if target at less than 50% of max hp
        if(target.hp.GetCurrentValue() < Mathf.FloorToInt((float)target.hp.GetValue() / 2f))
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
        foreach(StatusEffectInstance statusEffectInstance in target.statusEffects)
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
        if(target.statusEffects.Count > 0)
        {
            foreach(StatusEffectInstance statusEffectInstance in target.statusEffects)
            {
                //if ally has a negative status effect...
                if(action.targetFriendly && !statusEffectInstance.statusEffectSO.isBuff)
                {
                    return true;
                }
                //if playablable character had a positive status effect
                else if(action.targetHostile && statusEffectInstance.statusEffectSO.isBuff)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
