using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[System.Serializable]
public class TargetSelectState : BattleState
{
    [SerializeField] private TargetInfo targetInfo;
    [SerializeField] private SignalSender onCameraUnfollow;
    //temp values
    private Turn currentTurn;
    private Turn castTemp = null;
    private List<Combatant> targets = new List<Combatant>();    

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("entering target select state");
        //set temp values
        currentTurn = battleTimeline.CurrentTurn;
        targets = new List<Combatant>();

        //display cast preview
        if (currentTurn.TurnType != TurnType.Intervention && currentTurn.TurnType != TurnType.Cast && currentTurn.Action.HasCastTime)
        {
            castTemp = battleTimeline.AddCastToQueue(currentTurn.Actor, currentTurn.Action, new List<Combatant>());
            currentTurn.Actor.OnCastStart();
        }

        //display time cost preview (ignore if intervention, don't apply to own casts)
        if (currentTurn.TurnType != TurnType.Intervention && currentTurn.Action.ActorTurnModifier != 0)
        {
            bool ignoreCasts = false;
            if (castTemp != null)
            {
                ignoreCasts = true;
            }
            battleTimeline.ApplyTurnModifier(currentTurn.Actor, currentTurn.Action.ActorTurnModifier, true, ignoreCasts, 0);
        }

        //store copy of relative positions
        battleTimeline.TakeSnapshot();

        //activate buttons on valid targets and select default target
        gridManager.DisplaySelectableTargets(currentTurn.Action.TargetingType, currentTurn.Actor, currentTurn.Action.IsMelee);
    }

    public override void StateUpdate()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (targets.Count > 0)
            {
                //don't apply modifier to own cast
                bool ignoreCasts = false;
                if (castTemp != null)
                {
                    battleTimeline.UpdateTurnTargets(castTemp, targets);
                    castTemp = null;
                    ignoreCasts = true;
                }
                else
                {
                    battleTimeline.UpdateTurnTargets(currentTurn, targets);
                }

                //publish changes to actor's turn cost (ignore if intervention)
                if (currentTurn.TurnType != TurnType.Intervention && currentTurn.Action.ActorTurnModifier != 0)
                {
                    battleTimeline.RemoveTempTurnModifier(currentTurn.Actor);
                    battleTimeline.ApplyTurnModifier(currentTurn.Actor, currentTurn.Action.ActorTurnModifier, false, ignoreCasts, 0);
                }

                //reset previews
                ClearEffectPreviews();
                
                //move to execution state
                stateMachine.ChangeState((int)BattleStateType.Execute);
            }
            else
            {
                Debug.Log("No target selected");
            }
        }
        else if (Input.GetButtonDown("Cancel"))
        {
            CancelAction();
            
            stateMachine.ChangeState((int)BattleStateType.Menu);
        }
        else if (Input.GetButtonDown("QueueIntervention1"))
        {
            if (battleManager.InterventionCheck(0))
            {
                if (Input.GetButton("Shift"))
                {
                    battleTimeline.RemoveLastIntervention(battleManager.PlayableCombatants[0]);
                }
                else
                {
                    battleTimeline.AddInterventionToQueue(battleManager.PlayableCombatants[0]);
                }
            }
        }
        else if (Input.GetButtonDown("QueueIntervention2"))
        {
            if (battleManager.InterventionCheck(1))
            {
                if (Input.GetButton("Shift"))
                {
                    battleTimeline.RemoveLastIntervention(battleManager.PlayableCombatants[1]);
                }
                else
                {
                    battleTimeline.AddInterventionToQueue(battleManager.PlayableCombatants[1]);
                }
            }
        }
        else if (Input.GetButtonDown("QueueIntervention3"))
        {
            if (battleManager.InterventionCheck(2))
            {
                if (Input.GetButton("Shift"))
                {
                    battleTimeline.RemoveLastIntervention(battleManager.PlayableCombatants[2]);
                }
                else
                {
                    battleTimeline.AddInterventionToQueue(battleManager.PlayableCombatants[2]);
                }
            }
        }
    }
       
    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();

        //unhighlight targets and make them unselectable
        foreach (Combatant target in battleManager.GetCombatants(CombatantType.All))
        {
            target.ToggleTargeted(false);
            battleTimeline.HighlightTarget(target, false);

            target.ChangeSelectState(CombatantTargetState.Default);
        }
        EventSystem.current.SetSelectedGameObject(null);

        //hide target info
        targetInfo.Hide();

        battleTimeline.ClearSnapshot();
    }

    private void CancelAction()
    {
        //remove actor's turn cost modifier
        if (currentTurn.TurnType != TurnType.Intervention && currentTurn.Action.ActorTurnModifier != 0)
        {
            battleTimeline.RemoveTempTurnModifier(currentTurn.Actor);
        }

        //remove cast temp
        if (castTemp != null)
        {
            battleTimeline.RemoveTurn(castTemp, false);
            currentTurn.Actor.OnCastEnd();
        }

        //reset action effects previews
        ClearEffectPreviews();

        battleTimeline.CancelAction(currentTurn);

        //hide health bars
        foreach (Combatant target in targets)
        {
            target.DisplayHealthBar(false);
        }
    }

    public void OnSelectTarget(GameObject combatantObject)
    {
        //clear previous target previews
        if (targets.Count > 0)
        {
            foreach (Combatant target in targets)
            {
                target.DisplayHealthBar(false);
            }
            ClearEffectPreviews();
        }
        
        Combatant selectedTarget = combatantObject.GetComponent<Combatant>();

        //set targets
        targets = gridManager.GetTargets(selectedTarget.Tile, currentTurn.Action.AOEType, currentTurn.Action.IsMelee, currentTurn.TargetedCombatantType);

        //display preview
        DisplayPreviews();
    }

    private void DisplayPreviews()
    {
        if (targets.Count <= 0)
        {
            Debug.Log("No targets!");
            return;
        }

        foreach (Combatant target in targets)
        {
            //highlight sprites
            target.ToggleTargeted(true);
            target.DisplayHealthBar(true);

            //highlight turn panels
            battleTimeline.HighlightTarget(target, true);

            //display turn changes
            if (currentTurn.Action.TargetTurnModifier != 0)
            {
                int actionIndex = 0;
                if (castTemp != null)
                {
                    actionIndex = battleTimeline.TurnQueue.IndexOf(castTemp);
                }
                battleTimeline.ApplyTurnModifier(target, currentTurn.Action.TargetTurnModifier, true, false, actionIndex);
            }
        }

        //action preview
        if (castTemp != null)
        {
            battleTimeline.DisplayActionPreview(castTemp, castTemp.Action, targets, false);
        }
        else
        {
            battleTimeline.DisplayActionPreview(currentTurn, currentTurn.Action, targets, false);
        }
    }

    private void ClearEffectPreviews()
    {
        foreach (Combatant target in targets)
        {
            if (currentTurn.Action.TargetTurnModifier != 0)
            {
                //remove turn modifiers
                battleTimeline.RemoveTempTurnModifier(target);
            }
            
            //unhighlight sprites
            target.ToggleTargeted(false);

            //unhighlight turn panels
            battleTimeline.HighlightTarget(target, false);
        }
        battleTimeline.LoadSnapshot();
    }
    
}
