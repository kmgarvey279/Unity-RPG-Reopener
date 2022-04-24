using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StateMachineNamespace;
using UnityEngine.EventSystems;

[System.Serializable]
public class PlayerMoveState : BattleState
{
    // [SerializeField] private GridManager gridManager;
    // // [SerializeField] private SignalSender onCameraZoomOut;

    // private Tile selectedTile;
    // private List<Tile> path = new List<Tile>();

    // public override void OnEnter()
    // {
    //     base.OnEnter();
    //     // onCameraZoomOut.Raise();

    //     // int range = turnData.combatant.battleStatDict[BattleStatType.MoveRange].GetValue();
    //     gridManager.DisplaySelectableTiles(true, false);
        
    //     EventSystem.current.SetSelectedGameObject(null);
    //     EventSystem.current.SetSelectedGameObject(turnData.combatant.tile.gameObject);
    // }

    // public override void StateUpdate()
    // {
    //     if(Input.GetButtonDown("Select"))
    //     {
    //         if(selectedTile == turnData.combatant.tile)
    //         {
    //             battleManager.CancelAction();
    //             stateMachine.ChangeState((int)BattleStateType.Menu);
    //         }
    //         else
    //         { 
    //             battleManager.UpdateActionCost(1);
                
    //             if(selectedTile.occupier)
    //             {
    //                 List<Tile> reversePath = new List<Tile>();
    //                 reversePath.AddRange(path);
    //                 reversePath.Reverse();
    //                 selectedTile.occupier.Move(reversePath, MovementType.Move);
    //             }
    //             turnData.combatant.Move(path, MovementType.Move); 
    //         }
    //     }
    //     else if(Input.GetButtonDown("Cancel"))
    //     {
    //         battleManager.CancelAction();
    //         stateMachine.ChangeState((int)BattleStateType.Menu);
    //     }
    // }


    // public override void StateFixedUpdate()
    // {

    // }

    // public override void OnExit()
    // {
    //     base.OnExit();
    //     selectedTile = null; 
    //     gridManager.HideTiles();
    // }

    // public void OnSelectTile(GameObject tileObject)
    // {     
    //     selectedTile = tileObject.GetComponent<Tile>();
    //     path = gridManager.GetPath(turnData.combatant.tile, selectedTile);
    //     gridManager.DisplayPath(path);
    // }

    // public void OnMoveComplete()
    // {
    //     battleManager.stateMachine.ChangeState((int)BattleStateType.Menu);
    // }
}
