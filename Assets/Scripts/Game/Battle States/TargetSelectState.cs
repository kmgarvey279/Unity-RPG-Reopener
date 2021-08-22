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
    //attack data
    public class AttackOptions
    {
        public Action selectedAttack;
        public Action attack1;
        public Action attack2;
        public bool canSwitch = false;

        public AttackOptions(Action attack1, Action attack2)
        {
            this.attack1 = attack1;
            this.attack2 = attack2;
        }
    }
    public AttackOptions attackOptions;
    //possible targets
    private List<Tile> occupiedTiles = new List<Tile>();
    private List<Tile> targetableTiles = new List<Tile>();
    //temp action data
    private Tile selectedTile;
    [SerializeField] private List<Combatant> selectedTargets;
    [Header("Events (Signals)")]
    [SerializeField] private SignalSenderGO onCameraZoomIn;

    public override void OnEnter()
    {
        base.OnEnter();
        turnData = battleManager.turnData;
        //temp target data
        selectedTargets = new List<Combatant>();
        selectedTile = null;
        //check max range
        int maxRange;
        if(turnData.action.actionType == ActionType.Attack)
        {
            attackButtons.gameObject.SetActive(true);
            attackOptions = new AttackOptions(turnData.combatant.battleStats.attack1, turnData.combatant.battleStats.attack2);

            maxRange = Mathf.Max(attackOptions.attack1.range, attackOptions.attack2.range);
        }
        else
        {
            maxRange = turnData.action.range;
        }

        SetTargetableCombatants(maxRange);
    }

    private void SetTargetableCombatants(int maxRange)
    {
        //go though each combatant...
        foreach(Combatant combatant in battleManager.allyParty.combatants)
        {
            //get tile they are on & add to list
            Tile tile = combatant.tile;
            occupiedTiles.Add(tile);
            //make targetable/untargetable
            if(turnData.action.targetFriendly && maxRange >= gridManager.GetMoveCost(turnData.combatant.tile, tile))
            {
                tile.ChangeTargetability(Targetability.Targetable);
                targetableTiles.Add(combatant.tile);
            }
            else if(combatant != turnData.combatant)
            {
                tile.ChangeTargetability(Targetability.Untargetable);
            }
        }
        //repeat for enemies
        foreach(Combatant combatant in battleManager.enemyParty.combatants)
        {
            Tile tile = combatant.tile;
            occupiedTiles.Add(tile);
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
        //if targets are available
        if(targetableTiles.Count > 0)
        {
            //find the nearest one
            Tile nearestTargetableTile = GetNearestTargetableTile();
            //face it
            if(nearestTargetableTile != turnData.combatant.tile)
            {
                turnData.combatant.FaceTarget(nearestTargetableTile.transform);
            }
            //select it
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
            if(turnData.action.actionType == ActionType.Attack && attackOptions.selectedAttack != null)
            {
                battleManager.SetAction(attackOptions.selectedAttack);
            }
            battleManager.SetTargets(selectedTile, selectedTargets);
            stateMachine.ChangeState((int)BattleStateType.Execute);
        }     
        else if(Input.GetButtonDown("Switch"))
        {
            if(turnData.action.actionType == ActionType.Attack && attackOptions.canSwitch)
            {
                if(attackOptions.selectedAttack == attackOptions.attack1)
                {
                    attackOptions.selectedAttack = attackOptions.attack2;
                }
                else 
                {
                    attackOptions.selectedAttack = attackOptions.attack1;
                }
            }
        }
        else if(Input.GetButtonDown("Cancel"))                
        {
            battleManager.CancelAction();
            stateMachine.ChangeState((int)BattleStateType.Menu);
        }
 
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();

        //clear list of targetable tiles
        foreach (Tile tile in occupiedTiles)
        {
            tile.ChangeTargetability(Targetability.Default);
        }
        occupiedTiles.Clear();
        targetableTiles.Clear();
        
        //clear attack data
        if(attackButtons.gameObject.activeInHierarchy)
            attackButtons.gameObject.SetActive(false);
        attackOptions = null;
    }

    public void OnSelectTile(GameObject tileObject)
    {   
        //clear previous target  
        if(selectedTargets.Count > 0)
            selectedTargets.Clear();
        //get new target
        selectedTile = tileObject.GetComponent<Tile>();   
        selectedTargets.Add(selectedTile.occupier.GetComponent<Combatant>()); 
        //set attack options
        if(turnData.action.actionType == ActionType.Attack)
        {
            SetAttacks();  
        //     battleManager.DisplayActionPreview(attackOptions.selectedAttack, selectedTargets);
        }   
        // else
        // {
        //     battleManager.DisplayActionPreview(turnData.action, selectedTargets);
        // }
    }

    public void OnConfirmTile()
    {                 
        // turnData.targetedTile = selectedTile;
        // stateMachine.ChangeState((int)BattleStateType.Execute);
    }

    private void SetAttacks()
    {
        bool attack1Usable = false;
        bool attack2Usable = false;

        //check if either attack type can be used
        if(attackOptions.attack1 && attackOptions.attack1.range >= gridManager.GetMoveCost(turnData.combatant.tile, selectedTile))
        {
            attack1Usable = true;
        }
        if(attackOptions.attack2 && attackOptions.attack2.range >= gridManager.GetMoveCost(turnData.combatant.tile, selectedTile))
        {
            attack2Usable = true;
        }
        //update buttons to reflect available attacks
        attackButtons.UpdateButtons(attack1Usable, attack2Usable);
        //set current attack
        if(attack1Usable)
        {
            attackOptions.selectedAttack = attackOptions.attack1;
        } 
        else if(attack2Usable)
        {
            attackOptions.selectedAttack = attackOptions.attack2;
        }
        //allow for attacks to be switched when both are available
        if(attack1Usable && attack2Usable)
        {
            attackOptions.canSwitch = true;
        }
    }
}
