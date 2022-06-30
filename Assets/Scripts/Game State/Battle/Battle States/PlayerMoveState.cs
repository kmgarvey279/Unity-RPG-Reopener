using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StateMachineNamespace;
using UnityEngine.EventSystems;

[System.Serializable]
public class PlayerMoveState : BattleState
{
    private Tile selectedTile;
    private bool moving = false;

    public override void OnEnter()
    {
        base.OnEnter();
        // onCameraZoomOut.Raise();

        //display tiles
        gridManager.DisplaySelectableTiles(CombatantType.Player, turnData.combatant.tile);
    }

    public override void StateUpdate()
    {
        if(Input.GetButtonDown("Select") && selectedTile != null)
        {
            StartCoroutine(MoveCo(selectedTile));
            
            gridManager.HideTiles();
            selectedTile = null;
        } 
        else if(Input.GetButtonDown("Cancel"))
        {
            gridManager.HideTiles();
            selectedTile = null;

            stateMachine.ChangeState((int)BattleStateType.Menu);
        }
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public void OnSelectTile(GameObject tileObject)
    {     
        selectedTile = tileObject.GetComponent<Tile>();     

        List<Tile> path = gridManager.GetPath(turnData.combatant.tile, selectedTile, CombatantType.Player);
        gridManager.DisplayPaths(new List<List<Tile>>(){path}, CombatantType.Player); 
    }

    private IEnumerator MoveCo(Tile tile)
    {
        StartCoroutine(turnData.combatant.ChangeTile(tile, "Move"));
        yield return new WaitUntil(() => !turnData.combatant.moving);
        turnData.hasMoved = true;

        stateMachine.ChangeState((int)BattleStateType.Menu);
    }
}
