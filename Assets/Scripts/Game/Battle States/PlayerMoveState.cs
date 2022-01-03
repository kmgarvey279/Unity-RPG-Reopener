using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StateMachineNamespace;
// using UnityEngine.EventSystems;

[System.Serializable]
public class PlayerMoveState : BattleState
{
    [SerializeField] private GridManager gridManager;

    private Tile selectedTile;
    private List<Tile> path = new List<Tile>();

    public override void OnEnter()
    {
        base.OnEnter();
        // onCameraZoomOut.Raise();

        //checks if player is canceling a move
        if(turnData.combatant.tile != turnData.startingTile)
        {
            battleManager.SetMoveCost(0);
            turnData.combatant.transform.position = turnData.startingTile.transform.position;
            turnData.combatant.SetDirection(new Vector2(turnData.startingDirection.x, turnData.startingDirection.y));
        }
        
        int range = turnData.combatant.battleStatDict[BattleStatType.MoveRange].GetValue();
        gridManager.DisplayTilesInRange(turnData.combatant.tile, range, true);
    }

    public override void StateUpdate()
    {
        if(Input.GetButtonDown("Select"))
        {
            int moveCost = gridManager.GetMoveCost(selectedTile, turnData.combatant.tile);      
            battleManager.SetMoveCost(moveCost);

            gridManager.HideTiles();

            turnData.combatant.gridMovement.Move(path, MovementType.Move); 
        }
    }


    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
        selectedTile = null; 
    }

    public void OnSelectTile(GameObject tileObject)
    {     
        selectedTile = tileObject.GetComponent<Tile>();
        path = gridManager.GetPath(turnData.combatant.tile, selectedTile);
        gridManager.DisplayPath(path);
    }

    public void OnMoveComplete()
    {
        battleManager.stateMachine.ChangeState((int)BattleStateType.Menu);
    }
}
