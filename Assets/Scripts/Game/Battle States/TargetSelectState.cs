using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using UnityEngine.EventSystems;

[System.Serializable]
public class TargetSelectState : BattleState
{
    [SerializeField] private GridManager gridManager;

    private TurnData turnData;
    //standard atttack options
    [SerializeField] private AttackButtons attackButtons;
    private Action attack1;
    private bool attack1Usable = false;
    private Action attack2;
    private bool attack2Usable = false;
    //targets
    private List<Tile> targetableTiles = new List<Tile>();
    private List<Tile> targetableTilesMeleeRange = new List<Tile>();
    //selected tile
    private Tile selectedTile;
    [Header("Events (Signals)")]
    [SerializeField] private SignalSenderGO onCameraZoomIn;

    public override void OnEnter()
    {
        base.OnEnter();
        turnData = battleManager.turnData;

        int maxRange;
        //display attack options if "attack" command was selected
        if(turnData.action.actionType == ActionType.Attack)
        {
            attackButtons.gameObject.SetActive(true);

            attack1 = turnData.combatant.battleStats.attack1;
            attack2 = turnData.combatant.battleStats.attack2;

            maxRange = Mathf.Max(attack1.range, attack2.range);
        }
        else
        {
            maxRange = turnData.action.range;
        }

        SetTargetableCombatants(maxRange);
    }

    private void SetTargetableCombatants(int maxRange)
    {
        //go though each allied/hostile combatant, grab their associated tile, and set as targetable or not targetable
        foreach(Combatant combatant in battleManager.allyParty.combatants)
        {
            Tile tile = combatant.tile;
            if(turnData.action.targetFriendly && maxRange >= gridManager.GetMoveCost(turnData.combatant.tile, tile))
            {
                tile.ChangeTargetability(Targetability.Targetable);
                targetableTiles.Add(combatant.tile);
            }
            else
            {
                tile.ChangeTargetability(Targetability.Untargetable);
            }
        }
        foreach(Combatant combatant in battleManager.enemyParty.combatants)
        {
            Tile tile = combatant.tile;
            if(turnData.action.targetHostile && maxRange >= gridManager.GetMoveCost(turnData.combatant.tile, tile))
            {
                tile.ChangeTargetability(Targetability.Targetable);
                targetableTiles.Add(combatant.tile);
            }
            else
            {
                tile.ChangeTargetability(Targetability.Untargetable);
            }
        }
        if(targetableTiles.Count > 0)
        {
            Tile nearestTargetableTile = GetNearestTargetableTile();

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(nearestTargetableTile.gameObject);
        }
    }

    private Tile GetNearestTargetableTile()
    {
        Tile closestTargetableTile = null;
        float smallestDistance = Mathf.Infinity;
        
        foreach(Tile tile in targetableTiles)
        {
            float thisDistance = gridManager.GetMoveCost(tile, turnData.combatant.tile);

            if(thisDistance < smallestDistance)
            {
                smallestDistance = thisDistance;
                closestTargetableTile = tile;
            }
        }
        return closestTargetableTile;
    }

    public override void StateUpdate()
    {
        if(Input.GetButtonDown("Select"))
        {
            if(selectedTile == null)
            {
                Debug.Log("No target!");
            }
            else
            {
                if(selectedTile.occupier)
                    turnData.targets.Add(selectedTile.occupier.GetComponent<Combatant>());
                
                turnData.targetedTile = selectedTile;
                stateMachine.ChangeState((int)BattleStateType.Execute);
            }
        }
        else if(Input.GetButtonDown("Cancel"))
        {
            battleManager.turnData.action = null;
            stateMachine.ChangeState((int)BattleStateType.Menu);
        }
 
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();

        //clear tiles
        selectedTile = null;
        gridManager.HideTiles();
        //clear list of targetable tiles
        targetableTiles.Clear();
        //hide attack display
        if(attackButtons.gameObject.activeInHierarchy)
            attackButtons.gameObject.SetActive(false);
        attack1 = null;
        attack1Usable = false;
        attack2 = null;
        attack2Usable = false;
    }

    public void OnSelectTile(GameObject tileObject)
    {     
        Debug.Log("Test");
        selectedTile = tileObject.GetComponent<Tile>();     
        if(turnData.action.actionType == ActionType.Attack)
        {
            if(attack1 && attack1.range >= gridManager.GetMoveCost(turnData.combatant.tile, selectedTile))
            {
                attack1Usable = true;
            }
            else
            {
                attack1Usable = false;
            }
            if(attack2 && attack2.range >= gridManager.GetMoveCost(turnData.combatant.tile, selectedTile))
            {
                attack2Usable = true;
            }
            else
            {
                attack2Usable = false;
            }
            attackButtons.UpdateButtons(attack1Usable, attack2Usable);
        }   
    }
}
