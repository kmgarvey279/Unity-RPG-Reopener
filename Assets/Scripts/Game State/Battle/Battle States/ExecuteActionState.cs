using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class ExecuteActionState : BattleState
{
    [SerializeField] private ActionPopup actionPopup;
    [SerializeField] private SignalSender onInterventionEnd;
    [SerializeField] private Inventory inventory;
    private ActionEvent actionEventToExecute;
    //track events
    [SerializeField] private BattleEventQueue battleEventQueue;
    private bool actorDidMove;
    private WaitForSeconds waitZeroPointTwoFive = new WaitForSeconds(0.25f);
    private WaitForSeconds waitZeroPointFive = new WaitForSeconds(0.5f);
    [SerializeField] private StatusEffect castingStatus;

    private void Start()
    {
        battleStateType = BattleStateType.Execute;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //copy current action event
        actionEventToExecute = new ActionEvent(battleTimeline.CurrentTurn.Actor, battleTimeline.CurrentTurn.Action, battleTimeline.CurrentTurn.Targets);
        if(actionEventToExecute.Action.HasCastTime && battleTimeline.CurrentTurn.TurnType != TurnType.Intervention && battleTimeline.CurrentTurn.TurnType != TurnType.Cast)
        {
            actionEventToExecute.Actor.AddStatusEffect(castingStatus);
            StartCoroutine(EndActionPhase());
        }
        else
        {
            if (battleTimeline.CurrentTurn.TurnType == TurnType.Cast)
            {
                battleTimeline.CurrentTurn.Actor.RemoveStatusEffect(castingStatus);
            }
            StartCoroutine(PreparationPhase());
        }
    }

    public override void StateUpdate()
    {
        if (Input.GetButtonDown("Intervention"))
        {
            battleTimeline.AddTurn(TurnType.Intervention, battleManager.GetCombatants(CombatantType.Player)[0]);
        }
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public IEnumerator PreparationPhase()
    {
        //display name of action
        actionPopup.Display(actionEventToExecute.Action.ActionName);
        //move to execute position
        actorDidMove = false;
        if (actionEventToExecute.Action.ExecutePosition == ExecutePosition.TargetRow)
        {
            actorDidMove = true;
            yield return StartCoroutine(actionEventToExecute.Actor.Move(gridManager.CenterTiles[actionEventToExecute.Actor.CombatantType][actionEventToExecute.Targets[0].Tile.Y].transform, "Idle", 3.5f));
        }
        else if (actionEventToExecute.Action.ExecutePosition == ExecutePosition.FrontCenter)
        {
            actorDidMove = true;
            yield return StartCoroutine(actionEventToExecute.Actor.Move(gridManager.CenterTiles[actionEventToExecute.Actor.CombatantType][1].transform, "Idle", 3.5f));
        }
        
        //"unpause" time
        onInterventionEnd.Raise();

        //start action
        StartCoroutine(ExecuteActionPhase());
    }

    private IEnumerator ExecuteActionPhase()
    {
        List<ActionSubevent> actionSubevents = new List<ActionSubevent>();
        for (int i = 0; i < actionEventToExecute.Targets.Count; i++)
        {
            ActionSubevent actionSubevent = new ActionSubevent(actionEventToExecute.Action, actionEventToExecute.Actor, actionEventToExecute.Targets[i], i);
            actionSubevents.Add(actionSubevent);
        }
        int hitCount = actionSubevents[0].Action.HitCount;
        //execute max number of hits
        for (int i = 0; i < hitCount; i++)
        {
            List<ActionSubevent> actionSubeventsToExecute = actionSubevents;

            //if targeting is random, pick a random target from list of potential targets
            if (actionEventToExecute.Action.HitRandomTarget)
            {
                int roll = Mathf.FloorToInt(Random.Range(0, actionSubeventsToExecute.Count));
                actionSubeventsToExecute = new List<ActionSubevent>() { actionSubeventsToExecute[roll] };
            }
            //otherwise, apply to all targets (if hitcount >= this hit)
            foreach (ActionAnimationData animationData in actionEventToExecute.Action.ActionAnimations)
            {
                //skip non-repeating animations (ex: spell casts) or skills w/ different multiple hit animations
                if (animationData.OnlyPlayOnXHit && animationData.XHit != i)
                {
                    continue;
                }

                //trigger actor sprite animation
                if (animationData.BattleAnimatorTrigger != BattleAnimatorTrigger.None)
                {
                    if (animationData.BattleAnimatorTrigger == BattleAnimatorTrigger.Custom)
                    {
                        actionEventToExecute.Actor.TriggerAnimation(animationData.CustomAnimatorTrigger, false);
                    }
                    else
                    {
                        actionEventToExecute.Actor.TriggerAnimation(animationData.BattleAnimatorTrigger.ToString(), false);
                    }
                    yield return waitZeroPointTwoFive;
                }

                //spawn animation visual effects
                if (animationData.VFXPrefab != null)
                {
                    //spawn multiple animations at each target
                    if (animationData.SpawnForEachTarget)
                    {
                        //foreach (Combatant target in actionEventToExecute.Targets)
                        foreach (ActionSubevent actionSubevent in actionSubeventsToExecute)
                        {
                            Transform vfxSpawnPoint = GetVFXSpawnPoint(animationData, actionEventToExecute.Actor, actionSubevent.Target);

                            GameObject effectObject = Instantiate(animationData.VFXPrefab, vfxSpawnPoint.position, animationData.VFXPrefab.transform.rotation);
                            VFXParent actionVFX = effectObject.GetComponent<VFXParent>();
                            if (actionVFX)
                            {
                                //flip animation?
                                bool shouldFlip = false;
                                if (battleTimeline.CurrentTurn.Actor.CombatantType == CombatantType.Enemy)
                                {
                                    shouldFlip = true;
                                }

                                //bind vfx to spawn point?
                                if(animationData.BindVFXToSpawnPoint)
                                {
                                    //effectObject.transform.parent = vfxSpawnPoint;
                                }

                                //start animation
                                actionVFX.TriggerAnimation(shouldFlip);

                                //start movement if projectile
                                if (animationData.IsProjectile)
                                {
                                    Vector2 destination = actionSubevent.Target.CombatantBindPositions[CombatantBindPosition.Center].transform.position;
                                    StartCoroutine(actionVFX.MoveCo(destination));
                                }
                            }

                        }
                    }
                    //spawn single animation
                    else
                    {
                        Transform vfxSpawnPoint = GetVFXSpawnPoint(animationData, actionEventToExecute.Actor, actionSubevents[0].Target);
                        //vfxSpawnPoints.Add(vfxSpawnPoint, actionSubevents[0].Target);

                        GameObject effectObject = Instantiate(animationData.VFXPrefab, vfxSpawnPoint.position, animationData.VFXPrefab.transform.rotation);
                        VFXParent actionVFX = effectObject.GetComponent<VFXParent>();
                        if (actionVFX)
                        {
                            //flip animation?
                            bool shouldFlip = false;
                            if (battleTimeline.CurrentTurn.Actor.CombatantType == CombatantType.Enemy)
                            {
                                shouldFlip = true;
                            }

                            //bind vfx to spawn point?
                            if (animationData.BindVFXToSpawnPoint)
                            {
                                //effectObject.transform.parent = vfxSpawnPoint;
                            }

                            //start animation
                            actionVFX.TriggerAnimation(shouldFlip);

                            //start movement if projectile
                            if (animationData.IsProjectile)
                            {
                                Vector2 destination = actionSubevents[0].Target.CombatantBindPositions[CombatantBindPosition.Center].transform.position;
                                StartCoroutine(actionVFX.MoveCo(destination));
                            }
                        }
                    }
                }
                //wait for animations to finish
                yield return new WaitForSeconds(animationData.Duration);
            }
            //apply action to target(s)
            foreach (ActionSubevent actionSubevent in actionSubevents)
            {
                Debug.Log("Calling on event to execute");
                actionSubevent.Execute();
            }
            yield return waitZeroPointFive;
            //trigger per hit effects
            yield return StartCoroutine(TriggerEffects(actionSubevents, TriggerFrequency.PerHit));
        }
        yield return StartCoroutine(TriggerEffects(actionSubevents, TriggerFrequency.PerAction));

        //move to next phase
        StartCoroutine(ResolveActionPhase());
    }
    private IEnumerator ResolveActionPhase()
    {
        actionPopup.Hide();

        //return to original position
        if (actorDidMove)
        {
            yield return actionEventToExecute.Actor.Move(actionEventToExecute.Actor.Tile.transform, "Idle", 3f); ;
        }
        //complete animations
        for (int i = battleManager.Combatants.Count - 1; i >= 0; i--)
        {
            //resolve health change
            battleManager.Combatants[i].ResolveHealthChange();
        }
        yield return waitZeroPointFive;

        for (int i = battleManager.Combatants.Count - 1; i >= 0; i--)
        {
            if (battleManager.Combatants[i].HP.CurrentValue <= 0)
            {
                battleManager.KOCombatant(battleManager.Combatants[i]);
                if (battleManager.Combatants[i] == battleTimeline.CurrentTurn.Actor)
                {
                    battleTimeline.CurrentTurn.SetIsDead(true);
                }
            }
            else
            {
                battleManager.Combatants[i].ReturnToDefaultAnimation();
            }
        }

        //check actor status and move to next phase
        StartCoroutine(EndActionPhase());
    }

    public IEnumerator EndActionPhase()
    {
        Debug.Log("action complete!");

        //apply costs of action
        battleTimeline.CurrentTurn.Actor.ApplyActionCost(battleTimeline.CurrentTurn.Action.ActionCostType, battleTimeline.CurrentTurn.Action.Cost);
        //consume item and destroy runtime only scriptable object
        if (battleTimeline.CurrentTurn.Action is ActionUseItem)
        {
            ActionUseItem actionUseItem = (ActionUseItem)battleTimeline.CurrentTurn.Action;
            Debug.Log(actionUseItem.UsableItem.ItemName);
            inventory.RemoveItem(actionUseItem.UsableItem);
            Destroy(actionUseItem);
        }
        //change to turn end state
        yield return waitZeroPointTwoFive;
        stateMachine.ChangeState((int)BattleStateType.TurnEnd);
    }

    /////////////////////////////////////////////////////////

    public bool Roll(float chance)
    {
        int roll = Random.Range(1, 100);
        if(roll <= chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator TriggerEffects(List<ActionSubevent> actionSubevents, TriggerFrequency triggerFrequency)
    {
        //effects tied to action
        foreach (TriggerableBattleEffect thisEffect in actionEventToExecute.Action.TriggerableBattleEffects)
        {
            if (thisEffect.TriggerFrequency != triggerFrequency)
            {
                break;
            }
            Dictionary<Combatant, float> targets = new Dictionary<Combatant, float>();
            //go through each actor/target subevent
            foreach (ActionSubevent actionSubevent in actionSubevents)
            {
                if (thisEffect.TriggerCheck(actionSubevent))
                {
                    targets.Add(actionSubevent.Target, actionSubevent.HealthEffectSum);
                }
            }
            BattleEvent battleEvent = new BattleEvent(actionEventToExecute.Actor, targets, thisEffect);
            battleEventQueue.AddEvent(battleEvent);
        }
        yield return StartCoroutine(battleEventQueue.ExhaustQueueCo());

        //triggerable effects tied to actor
        yield return StartCoroutine(actionEventToExecute.Actor.TriggerBattleEffectsCo(BattleEventType.Acting, actionSubevents, triggerFrequency));
        yield return StartCoroutine(battleEventQueue.ExhaustQueueCo());

        //triggerable effects tied to targets
        foreach (ActionSubevent actionSubevent in actionSubevents)
        {
            yield return StartCoroutine(actionSubevent.Target.TriggerBattleEffectsCo(BattleEventType.Targeted, new List<ActionSubevent>() { actionSubevent }, triggerFrequency));
        }
        yield return StartCoroutine(battleEventQueue.ExhaustQueueCo());
    }

    private Transform GetVFXSpawnPoint(ActionAnimationData actionAnimationData, Combatant actor, Combatant target)
    {
        Transform spawnPoint = target.CombatantBindPositions[actionAnimationData.CombatantBindPosition].transform;
        
        switch (actionAnimationData.VFXSpawnPosition)
        {
            case ActionVFXPosition.Actor:
                spawnPoint = actor.CombatantBindPositions[actionAnimationData.CombatantBindPosition].transform;
                break;
            case ActionVFXPosition.TargetedGridCenter:
                spawnPoint = gridManager.GetTileArray(target.CombatantType)[1, 1].transform;
                break;
            default:
                break;
        }
        return spawnPoint;
    }
}