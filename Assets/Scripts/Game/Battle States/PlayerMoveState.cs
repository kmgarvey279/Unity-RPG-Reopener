using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using UnityEngine.EventSystems;

[System.Serializable]
public class PlayerMoveState : StateMachine.State
{
    private BattleManager battleManager;
    public Battlefield battlefield;

    private TurnData turnData;

    // Start is called before the first frame update
    void Start()
    {
        battleManager = GetComponentInParent<BattleManager>();
    }

    public override void OnEnter()
    {
        turnData = battleManager.turnData;

        //zoom out camera to entire battlefield (or just partway?)
        battlefield.cameraManager.ZoomOut();

        int moveRange = turnData.combatant.GetStatValue(StatType.MoveRange);
        battlefield.gridManager.DisplayTilesInRange(turnData.combatant.tile, moveRange, -1);
    }

    public override void StateUpdate()
    {
    }

    public override void StateFixedUpdate()
    {

    }

    public override string CheckConditions()
    {
        return nextState;
    }

    public override void OnExit()
    {
        battlefield.gridManager.HideTiles();
    }

    public void OnSelectTile(GameObject tileObject)
    {     
        turnData.combatant.Move(tileObject.GetComponent<Tile>());
        // actionData.tileDestination = tileObject.GetComponent<Tile>(); 
        // nextState = "PlayerActionState";
    }

    public void OnMoveComplete()
    {
        turnData.hasMoved = true;
        if(turnData.action == null)
        {
            nextState = "BattleMenu";
        }
        else
        {
            if(turnData.action.fixedTarget)
            {
                nextState = "TargetSelect";
            }
            else
            {
                nextState = "TileSelect";
            }
        }
    }


}
