using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using UnityEngine.EventSystems;

[System.Serializable]
public class TargetSelectState : BattleState
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private AttackSwitch attackSwitch;

    private TurnData turnData;

    private List<Combatant> availableTargets = new List<Combatant>();
    private Combatant selectedTarget;

    [Header("Events (Signals)")]
    [SerializeField] private SignalSenderGO onCameraZoomIn;

    public override void OnEnter()
    {
        base.OnEnter();
        turnData = battleManager.turnData;

        if(turnData.action.actionType == ActionType.Attack)
            attackSwitch.gameObject.SetActive(true);

        SetTargetableCombatants();
    }

    private void SetTargetableCombatants()
    {
        //add allies and/or enemies to list of possible targets
        if(turnData.action.targetFriendly)
        {
            availableTargets.AddRange(battleManager.allyParty.combatants);
        }
        if(turnData.action.targetHostile)
        {
            availableTargets.AddRange(battleManager.enemyParty.combatants);
        }
        foreach(Combatant target in availableTargets)
        {
            target.targetIcon.ToggleButton(true);
        }
        SetInitialTarget();
    }

    private void SetInitialTarget()
    {   
        GameObject initialTarget;
        //set default target
        if(availableTargets.Contains(turnData.combatant))
        {
            initialTarget = turnData.combatant.gameObject.GetComponentInChildren<TargetIcon>().gameObject;
        }
        else
        {
            initialTarget = GetNearestTarget().GetComponentInChildren<TargetIcon>().gameObject;
        }
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(initialTarget);
    }

    private GameObject GetNearestTarget()
    {
        Combatant closestTarget = null;
        float smallestDistance = Mathf.Infinity;
        
        foreach(Combatant target in availableTargets)
        {
            float thisDistance = Vector3.Distance(turnData.combatant.transform.position, target.transform.position);

            if(thisDistance < smallestDistance)
            {
                smallestDistance = thisDistance;
                closestTarget = target;
            }
        }
        return closestTarget.gameObject;
    }

    public override void StateUpdate()
    {
        if(Input.GetButtonDown("Select"))
        {
            turnData.targets.Add(selectedTarget);
            turnData.targetedTile = selectedTarget.tile;
            stateMachine.ChangeState((int)BattleStateType.Execute);
        }
        else if(Input.GetButtonDown("Cancel"))
        {
            battleManager.turnData.action = null;
            stateMachine.ChangeState((int)BattleStateType.Menu);
        }

        if(Input.GetButtonDown("Change"))
        {
            Debug.Log("test");
            ChangeAttackType();
        }
 
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();

        //clear tiles & targets
        gridManager.HideTiles();
        foreach(Combatant target in availableTargets)
        {
            target.targetIcon.ToggleButton(false);
        }
        availableTargets.Clear();
        if(selectedTarget != null)
            selectedTarget = null;
        //hide attack display
        if(attackSwitch.gameObject.activeInHierarchy)
            attackSwitch.gameObject.SetActive(false);
    }

    public void OnTargetChange()
    {
        Combatant selectedTarget = EventSystem.current.currentSelectedGameObject.GetComponentInParent<Combatant>();
        gridManager.DisplayAOE(selectedTarget.tile, turnData.action.aoe);

        if(turnData.action.actionType == ActionType.Attack)
        {
            Action meleeAttack = turnData.combatant.battleStats.meleeAttack;
            Action rangedAttack = turnData.combatant.battleStats.rangedAttack;

            if(meleeAttack && meleeAttack.range >= gridManager.GetMoveCost(turnData.combatant.tile, selectedTarget.tile))
            {
                turnData.action = meleeAttack;
                attackSwitch.ChangeSelectedIcon(AttackType.Melee);
            }
            else if(rangedAttack)
            {
                turnData.action = rangedAttack;
                attackSwitch.ChangeSelectedIcon(AttackType.Gun);
            }
            else
            {
                Debug.Log("No targets in range");
            }
        }
    }

    public void ChangeAttackType()
    {
        Action meleeAttack = turnData.combatant.battleStats.meleeAttack;
        Action rangedAttack = turnData.combatant.battleStats.rangedAttack;

        if(turnData.action == meleeAttack && rangedAttack != null)
        {
            turnData.action = rangedAttack;
            attackSwitch.ChangeSelectedIcon(AttackType.Gun);
        }
        else if(turnData.action == rangedAttack && meleeAttack != null)
        {
            turnData.action = meleeAttack;
            attackSwitch.ChangeSelectedIcon(AttackType.Melee);
        }
        else 
        {
            Debug.Log("Cannot switch attack type");
        }
    }
}
