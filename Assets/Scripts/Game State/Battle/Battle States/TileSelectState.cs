using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using UnityEngine.EventSystems;

[System.Serializable]
public class TileSelectState : BattleState
{
    private Tile selectedTile;
    private List<Combatant> selectedTargets;
    private TargetType targetType;
    private bool flip;
    [Header("Events")]
    [SerializeField] private SignalSender onCameraZoomOut;

    public override void OnEnter()
    {
        base.OnEnter();
        //set defaults
        selectedTile = null;
        selectedTargets = new List<Combatant>();
        flip = false;
        // onCameraZoomOut.Raise();
        //display tiles
        targetType = TargetType.TargetEnemy;
        if(turnData.action.targetFriendly)
        {
            targetType = TargetType.TargetPlayer;
        }

        if(turnData.action.isFixedTarget)
        {
            gridManager.DisplaySelectableTargets(turnData.combatant.tile, targetType, turnData.action.isMelee);
        }
        else
        {   
            gridManager.DisplaySelectableTiles(turnData.action, targetType, turnData.combatant.tile);
            if(turnData.action.isFixedAOE)
            {
                SpawnAOEs();
            }
        }
    }

    public override void StateUpdate()
    {
        if(Input.GetButtonDown("Select"))
        {
            if(selectedTargets.Count > 0 || turnData.action.actionType == ActionType.Move) 
            {
                battleManager.SetTargets(selectedTile, selectedTargets, targetType, flip);
                stateMachine.ChangeState((int)BattleStateType.Execute); 
            }
            else 
            {
                Debug.Log("No target selected");
            }
        } 
        else if(Input.GetButtonDown("Cancel"))
        {
            battleManager.CancelAction();
            stateMachine.ChangeState((int)BattleStateType.Menu);
        }
        else if(Input.GetButtonDown("Switch") && turnData.action.canFlip)
        {
            flip = !flip;
            SpawnAOEs();
        }
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
        //clear tiles
        gridManager.HideTiles();
    }

    public void OnSelectTile(GameObject tileObject)
    {     
        selectedTile = tileObject.GetComponent<Tile>();     
        // selectedTargets = gridManager.GetTargetsInRange(selectedTile, turnData.action.aoe, turnData.action.targetFriendly, turnData.action.targetHostile);   
        // if(turnData.action.knockback)
        // {
        //     Vector2 direction = (selectedTile.transform.position - turnData.combatant.tile.transform.position).normalized;
        //     List<Tile> path = new List<Tile>();
            // if(selectedTile.occupier && selectedTile.occupier is EnemyCombatant)
            //     path = gridManager.GetRow(selectedTile, direction);
            // gridManager.DisplayPath(path);
        // } 
        if(turnData.action.aoes.Count > 0)
        {
            SpawnAOEs();
        }
        if(turnData.action.actionType == ActionType.Move)
        {
            List<Tile> path = gridManager.GetPath(turnData.combatant.tile, selectedTile, TargetType.TargetPlayer);
            bool swap = false;
            if(path.Count > 0 && path[path.Count - 1].occupier)
            {
                swap = true;
            }
            gridManager.DisplayPaths(new List<List<Tile>>(){path}, TargetType.TargetPlayer, swap); 
        } 
    }

    private void SpawnAOEs()
    {
        if(selectedTargets.Count > 0)
        {
            foreach(Combatant target in selectedTargets)
            {
                target.ToggleHPBar(false);
            }
        }
        List<Tile> aoeTiles = gridManager.GetAOETiles(selectedTile, turnData.action, targetType, flip);
        gridManager.DisplayAOE(aoeTiles, targetType);
        
        selectedTargets = gridManager.GetTargetsInAOE(aoeTiles, targetType);
        foreach(Combatant target in selectedTargets)
        {
            target.ToggleHPBar(true);
        }

        if(turnData.action.knockback.doKnockback)
        {
            List<List<Tile>> knockbackPaths = new List<List<Tile>>();
            foreach(Combatant target in selectedTargets)
            {
                Vector2Int knockbackDestination = turnData.action.knockback.GetKnockbackDestination(target.tile);
                Tile newTile = gridManager.GetTileArray(TargetType.TargetEnemy)[knockbackDestination.x, knockbackDestination.y];
                if(newTile != target.tile && !newTile.occupier)
                {
                    knockbackPaths.Add(gridManager.GetPath(target.tile, newTile, TargetType.TargetEnemy));
                }
            }
            gridManager.DisplayPaths(knockbackPaths, TargetType.TargetEnemy, false);
        }
    }
    
}
