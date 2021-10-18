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
        public Action meleeAttack;
        public Action rangedAttack;
        public bool canSwitch = false;

        public AttackOptions(Action meleeAttack, Action rangedAttack)
        {
            this.meleeAttack = meleeAttack;
            this.rangedAttack = rangedAttack;
        }
    }
    public AttackOptions attackOptions;
    //possible targets
    private List<Tile> occupiedTiles = new List<Tile>();
    private List<Tile> targetableTiles = new List<Tile>();
    //temp action data
    private Tile selectedTile;
    private List<Combatant> selectedTargets;
    [Header("Events (Signals)")]
    [SerializeField] private SignalSenderGO onCameraZoomIn;

    public override void OnEnter()
    {
        base.OnEnter();
        turnData = battleManager.turnData;
        selectedTile = null;
        selectedTargets = new List<Combatant>();
        //check max range
        int maxRange;
        // if(turnData.action.actionType == ActionType.Attack)
        // {
        //     attackButtons.gameObject.SetActive(true);
        //     AllyCombatant combatant = (AllyCombatant)turnData.combatant;
        //     attackOptions = new AttackOptions(combatant.meleeAttack, combatant.rangedAttack);

        //     maxRange = Mathf.Max(attackOptions.meleeAttack.range, attackOptions.rangedAttack.range);
        // }
        // else
        // {
            maxRange = turnData.action.range;
        // }

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
            // if(turnData.action.actionType == ActionType.Attack && attackOptions.selectedAttack != null)
            // {
            //     battleManager.SetAction(attackOptions.selectedAttack);
            // }
            battleManager.SetTargets(selectedTile, selectedTargets);
            stateMachine.ChangeState((int)BattleStateType.Execute);
        }     
        else if(Input.GetButtonDown("Switch"))
        {
            if(turnData.action.actionType == ActionType.Attack && attackOptions.canSwitch)
            {
                if(attackOptions.selectedAttack == attackOptions.meleeAttack)
                {
                    attackOptions.selectedAttack = attackOptions.rangedAttack;
                }
                else 
                {
                    attackOptions.selectedAttack = attackOptions.meleeAttack;
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
        EventSystem.current.SetSelectedGameObject(null);
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
        Debug.Log("select");
        //clear previous target  
        if(selectedTargets.Count > 0)
            selectedTargets.Clear();
        //get new target
        selectedTile = tileObject.GetComponent<Tile>();   
        selectedTargets.Add(selectedTile.occupier); 
        //set attack options
        if(turnData.action.actionType == ActionType.Attack)
        {
            // SetAttacks();  
        //     battleManager.DisplayActionPreview(attackOptions.selectedAttack, selectedTargets);
        }
        if(turnData.action.knockback > 0)
        {
            Debug.Log("Knockback");
            Vector2 direction = (selectedTile.transform.position - turnData.combatant.tile.transform.position).normalized;
            List<Tile> path = gridManager.GetRow(selectedTile, direction, turnData.action.knockback, true);
            gridManager.DisplayPath(path);
        }   
        // else
        // {
        //     battleManager.DisplayActionPreview(turnData.action, selectedTargets);
        // }
    }

    private void SetAttacks()
    {
        bool meleeAttackUsable = false;
        bool rangedAttackUsable = false;

        //check if either attack type can be used
        if(attackOptions.meleeAttack && attackOptions.meleeAttack.range >= gridManager.GetMoveCost(turnData.combatant.tile, selectedTile))
        {
            meleeAttackUsable = true;
        }
        if(attackOptions.rangedAttack && attackOptions.rangedAttack.range >= gridManager.GetMoveCost(turnData.combatant.tile, selectedTile))
        {
            rangedAttackUsable = true;
        }
        //update buttons to reflect available attacks
        attackButtons.UpdateButtons(meleeAttackUsable, rangedAttackUsable);
        //set current attack
        if(meleeAttackUsable)
        {
            attackOptions.selectedAttack = attackOptions.meleeAttack;
        } 
        else if(rangedAttackUsable)
        {
            attackOptions.selectedAttack = attackOptions.rangedAttack;
        }
        //allow for attacks to be switched when both are available
        if(meleeAttackUsable && rangedAttackUsable)
        {
            attackOptions.canSwitch = true;
        }
    }
}
