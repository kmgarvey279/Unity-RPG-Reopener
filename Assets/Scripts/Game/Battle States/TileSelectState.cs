using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using UnityEngine.EventSystems;

[System.Serializable]
public class TileSelectState : StateMachine.State
{
    private BattleManager battleManager;
    private TurnData turnData;
    public Battlefield battlefield;

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

        int range = battleManager.turnData.action.range;
        int aoe = battleManager.turnData.action.aoe;
        battlefield.gridManager.DisplayTilesInRange(battleManager.turnData.combatant.tile, range, aoe);

    }

    public override void StateUpdate()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            battleManager.turnData.action = null;
            nextState = "BattleMenu";
        }
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
        turnData.targets.AddRange(battlefield.gridManager.gridDisplay.GetTargetsInAOE());
        turnData.targetedTile = tileObject.GetComponent<Tile>();
        nextState = "ExecuteAction";
        // actionData.tileDestination = tileObject.GetComponent<Tile>(); 
        // nextState = "PlayerActionState";
    }
}
