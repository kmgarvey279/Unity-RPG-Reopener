using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class TargetSelectState : BattleState
{
    [SerializeField] private CommandMenuManager commandMenuManager;
    [SerializeField] private TargetInfo targetInfo;
    [SerializeField] private SignalSender onCameraUnfollow;
    [SerializeField] private StatusEffect coldStatus;
    [SerializeField] private StatusEffect freezeStatus;
    //temp values
    private Turn currentTurn;
    private Combatant selectedTarget;
    private List<Combatant> validTargets = new List<Combatant>();
    private List<Combatant> targets = new List<Combatant>();
    private bool targetInfoSelected;

    private void EnableListeners()
    {
        InputManager.Instance.OnPressSubmit.AddListener(Submit);
        InputManager.Instance.OnPressCancel.AddListener(Cancel);
        InputManager.Instance.OnPressTabUI.AddListener(TabUI);
        InputManager.Instance.OnPressTabTarget.AddListener(TabTarget);
    }

    private void DisableListeners()
    {
        InputManager.Instance.OnPressSubmit.RemoveListener(Submit);
        InputManager.Instance.OnPressCancel.RemoveListener(Cancel);
        InputManager.Instance.OnPressTabUI.RemoveListener(TabUI);
        InputManager.Instance.OnPressTabTarget.RemoveListener(TabTarget);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("entering target select state");

        //onCameraUnfollow.Raise

        EnableListeners();

        //set temp values
        currentTurn = battleTimeline.CurrentTurn;
        selectedTarget = null;
        validTargets = new List<Combatant>();
        targets = new List<Combatant>();

        //store copy of relative positions
        battleTimeline.TakeSnapshot();

        //activate buttons on valid targets and select default target
        validTargets = gridManager.GetSelectableTargets(currentTurn.Action.TargetingType, currentTurn.Actor, currentTurn.Action.IsMelee, currentTurn.Action.IsBackAttack);
        if (validTargets.Count > 0)
        {
            validTargets[0].Select();
        }
    }

    private void Submit(bool isPressed)
    {
        Debug.Log("On Click Submit");
        if (targets.Count > 0)
        {
            currentTurn.SetTargets(targets);
            battleTimeline.RemoveTempTurnModifier(currentTurn.Actor);

            //reset previews
            ClearEffectPreviews();

            //reset menu "memory"
            commandMenuManager.ResetMenu();

            //move to execution state
            stateMachine.ChangeState((int)BattleStateType.Execute);
        }
        else
        {
            Debug.Log("No target selected");
        }
    }

    private void Cancel(bool isPressed)
    {
        CancelAction();

        stateMachine.ChangeState((int)BattleStateType.PlayerTurn);
    }

    private void TabTarget(bool isPressed)
    {
        Debug.Log("Cycle Targets Pressed");
        CycleTargets();
    }

    private void TabUI(bool isPressed)
    {
        ToggleTargetInfo();
    }

    public override void StateUpdate()
    {
    }
       
    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();

        DisableListeners();

        //unhighlight targets and make them unselectable
        foreach (Combatant target in battleManager.GetCombatants(CombatantType.All))
        {
            target.ToggleHighlight(false);
            battleTimeline.UnhighlightTarget(target);

            target.ChangeSelectState(CombatantTargetState.Default);
        }
        EventSystem.current.SetSelectedGameObject(null);

        //hide target info
        targetInfo.Hide();

        battleTimeline.ClearSnapshot();
    }

    private void ToggleTargetInfo()
    {
        //deselect info ui
        if (targetInfoSelected)
        {
            targetInfo.Deselect();
            targetInfoSelected = false;

            //reactivate target buttons
            gridManager.GetSelectableTargets(currentTurn.Action.TargetingType, currentTurn.Actor, currentTurn.Action.IsMelee, currentTurn.Action.IsBackAttack);
            
            //reselect target
            EventSystem.current.SetSelectedGameObject(null);
            if (selectedTarget != null)
            {
                selectedTarget.Select();
            }
            else if (validTargets.Count > 0)
            {
                validTargets[0].Select();
            }
        }
        //select target info ui
        else
        {
            SelectTargetInfo();
        }
    }

    private void SelectTargetInfo()
    {
        targetInfo.Select();
        targetInfoSelected = true;

        //unhighlight all targets apart from the selected one
        foreach (Combatant target in validTargets)
        {
            target.ChangeSelectState(CombatantTargetState.Default);
        }
        foreach (Combatant target in targets)
        {
            if (selectedTarget && target != selectedTarget)
            {
                target.ToggleHighlight(false);
                battleTimeline.UnhighlightTarget(target);
            }
        }
    }

    private void CycleTargets()
    {
        Debug.Log("Cycle Targets Triggered");
        if (validTargets.Count > 1 && selectedTarget != null && validTargets.Contains(selectedTarget))
        {
            int targetIndex = validTargets.IndexOf(selectedTarget) + 1;
            if (targetIndex >= validTargets.Count)
            {
                targetIndex = 0;
            }
            validTargets[targetIndex].Select();

            if (targetInfoSelected)
            {
                SelectTargetInfo();
            }
        }
    }

    private void CancelAction()
    {
        //remove actor's turn cost modifier
        battleTimeline.RemoveTempTurnModifier(currentTurn.Actor);
        
        //reset action effects previews
        ClearEffectPreviews();

        battleTimeline.CurrentTurn.CancelAction();

        //hide health bars
        battleManager.HideHealthBars();
    }

    public void OnSelectTarget(GameObject combatantObject)
    {
        //get new selected target
        Combatant previousTarget = selectedTarget;
        Combatant newTarget = combatantObject.GetComponent<Combatant>();
        
        //compare to previous selected target
        bool didChangeTarget = false;
        if (previousTarget != newTarget)
            didChangeTarget = true;

        if (didChangeTarget)
        {
            //don't update previews for AoE skills when cycling primary targets
            if (previousTarget != null && currentTurn.Action.AOEType == AOEType.All)
            {
                battleTimeline.UnselectTarget(selectedTarget);
                battleTimeline.SelectTarget(newTarget);
                selectedTarget = newTarget;
                return;
            }

            //clear old previews
            if (targets.Count > 0)
            {
                foreach (Combatant target in targets)
                {
                    target.HideHealthInfo();

                    //if (target is EnemyCombatant)
                    //{
                    //    EnemyCombatant enemyCombatant = (EnemyCombatant)target;
                    //    enemyCombatant.HideVulnerability();
                    //}
                }
                ClearEffectPreviews();
            }

            if (newTarget != null)
            {
                if (currentTurn.Action.TargetingType == TargetingType.TargetHostile)
                {
                    targets = gridManager.GetEnemyTargets(newTarget.Tile, currentTurn.Action.AOEType, currentTurn.Action.IsMelee);
                }
                else
                {
                    if (currentTurn.Action.AOEType == AOEType.Single)
                    {
                        targets = new List<Combatant>() { newTarget };
                    }
                    else
                    {
                        targets = validTargets;
                    }
                }
            }
            
            //reassign selected target
            selectedTarget = newTarget;
            if (selectedTarget != null && didChangeTarget)
            {
                DisplayPreviews();
            }
        }
    }

    private void DisplayPreviews()
    {
        if (targets.Count <= 0)
        {
            Debug.Log("No targets!");
            return;
        }

        battleTimeline.SelectTarget(selectedTarget);
        
        ActionSummary actionSummary = new ActionSummary(currentTurn.Action, currentTurn.IsIntervention);

        //actor turn modifier preview
        if (currentTurn.Action.ActorConditionalModifier.TurnModifier != 0)
        {
            foreach (Combatant target in targets)
            {
                if (currentTurn.Action.ActorConditionalModifier.ConditionsCheck(currentTurn.Actor, target, actionSummary))
                {
                    battleTimeline.ApplyTurnModifier(currentTurn.Actor, currentTurn.Action.ActorConditionalModifier.TurnModifier, true, currentTurn.Action.TargetConditionalModifier.ApplyToNextTurnOnly);
                    break;
                }
            }
        }

        //target previews
        foreach (Combatant target in targets)
        {
            //highlight sprites
            target.ToggleHighlight(true);
            target.DisplayHealthInfo();
            //highlight turn panels
            battleTimeline.HighlightTarget(target);

            if (currentTurn.Action.TargetConditionalModifier.TurnModifier != 0 && currentTurn.Action.TargetConditionalModifier.ConditionsCheck(currentTurn.Actor, target, actionSummary))
            {
                battleTimeline.ApplyTurnModifier(target, currentTurn.Action.TargetConditionalModifier.TurnModifier, true, currentTurn.Action.TargetConditionalModifier.ApplyToNextTurnOnly);
            }

            if (actionSummary.Action is Attack && target is EnemyCombatant)
            {
                Attack attack = (Attack)actionSummary.Action;
                EnemyCombatant enemyCombatant = (EnemyCombatant)target;
                EnemyInfo enemyInfo = enemyCombatant.EnemyInfo;

                bool isRevealed = false;
                if (SaveManager.Instance.LoadedData.PlayerData.EnemyLog.EnemyEntries.ContainsKey(enemyInfo.EnemyID))
                {
                    if (SaveManager.Instance.LoadedData.PlayerData.EnemyLog.EnemyEntries[enemyInfo.EnemyID].RevealedElements.Contains(attack.ElementalProperty))
                    {
                        isRevealed = true;
                    }
                }
                enemyCombatant.DisplayVulnerability(isRevealed, attack.ElementalProperty);
            }
        }
        battleTimeline.DisplayTurnOrder();
    }

    private void ClearEffectPreviews()
    {
        //clear selected target
        if (selectedTarget != null)
        {
            battleTimeline.UnselectTarget(selectedTarget);
        }

        //remove turn modifiers
        battleTimeline.RemoveTempTurnModifier(currentTurn.Actor);

        foreach (Combatant target in targets)
        {
            //remove turn modifiers
            battleTimeline.RemoveTempTurnModifier(target);

            //unhighlight sprites
            target.ToggleHighlight(false);

            //unhighlight turn panels
            battleTimeline.UnhighlightTarget(target);
        }
        battleTimeline.LoadSnapshot();
    }
}
