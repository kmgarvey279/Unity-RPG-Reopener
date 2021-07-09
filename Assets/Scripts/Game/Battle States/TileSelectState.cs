using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using UnityEngine.EventSystems;

[System.Serializable]
public class TileSelectState : BattleState
{
    private BattleManager battleManager;
    private TurnData turnData;
    public GridManager gridManager;
    [Header("Events")]
    [SerializeField] private SignalSender onCameraZoomOut;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        battleManager = GetComponentInParent<BattleManager>();
    }

    public override void OnEnter()
    {
        turnData = battleManager.turnData;

        onCameraZoomOut.Raise();

        int range = battleManager.turnData.action.range;
        int aoe = battleManager.turnData.action.aoe;
        gridManager.DisplayTilesInRange(battleManager.turnData.combatant.tile, range, aoe);

    }

    public override void StateUpdate()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            turnData.action = null;
            stateMachine.ChangeState((int)BattleStateType.Menu);
        }
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        gridManager.HideTiles();
    }

    public void OnSelectTile(GameObject tileObject)
    {                 
        turnData.targets.AddRange(gridManager.gridDisplay.GetTargetsInAOE());
        turnData.targetedTile = tileObject.GetComponent<Tile>();
        stateMachine.ChangeState((int)BattleStateType.Execute);
    }
}
