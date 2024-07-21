using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PreemptiveTriggerContainer
{
    public PreemptiveBattleEventTrigger PreemptiveBattleEventTrigger { get; private set; }
    public Combatant Actor { get; private set; }
    public List<Combatant> Targets { get; private set; } = new List<Combatant>();

    public PreemptiveTriggerContainer(PreemptiveBattleEventTrigger preemptiveBattleEventTrigger, Combatant actor, List<Combatant> targets)
    {
        PreemptiveBattleEventTrigger = preemptiveBattleEventTrigger;
        Actor = actor;
        Targets = targets;
    }
}

[System.Serializable]
public class ExecuteActionState : BattleState
{
    [SerializeField] private ActionPopup actionPopup;
    //[SerializeField] private ActionPopup actionPopupEnemy;

    [SerializeField] private SignalSender onInterventionEnd;
    [SerializeField] private ActionEvent actionEventToExecute;
    //track events
    [SerializeField] private List<BattleEventTrigger> universalTriggers = new List<BattleEventTrigger>();
    [SerializeField] private BattleEventQueue battleEventQueue;
    private WaitForSeconds wait025 = new WaitForSeconds(0.25f);
    private WaitForSeconds wait05 = new WaitForSeconds(0.5f);
    private WaitForSeconds wait075 = new WaitForSeconds(0.75f);
    private WaitForSeconds wait1 = new WaitForSeconds(1f);
    //[SerializeField] private StatusEffect castingStatus;

    private void Start()
    {
        battleStateType = BattleStateType.Execute;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //create action event
        actionEventToExecute = new ActionEvent(battleTimeline.CurrentTurn.Actor, battleTimeline.CurrentTurn.Action, battleTimeline.CurrentTurn.Targets, battleTimeline.CurrentTurn.IsIntervention);
        StartCoroutine(PreparationPhase());
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
    }

    #region Battle Loop
    public IEnumerator PreparationPhase()
    {
        //start next phase
        if (actionEventToExecute.Actor is EnemyCombatant)
        {
            StartCoroutine(PreemptivePhase());
        }
        else
        {
            StartCoroutine(ExecuteActionPhase());
        }
        yield return null;
    }

    private IEnumerator PreemptivePhase()
    {
        List<PreemptiveTriggerContainer> normalTriggers = new List<PreemptiveTriggerContainer>();
        List<PreemptiveTriggerContainer> protectTriggers = new List<PreemptiveTriggerContainer>();

        foreach (Combatant combatant in battleManager.GetCombatants(CombatantType.Player))
        {
            Debug.Log("preemptive trigger check 1");
            foreach (PreemptiveBattleEventTrigger trigger in combatant.PreemptiveBattleEventTriggers)
            {
                Debug.Log("preemptive trigger check 2");
                if (trigger.TriggerCheck())
                {
                    Debug.Log("preemptive trigger roll success");
                    List<Combatant> targets = trigger.GetTargets(actionEventToExecute, combatant);
                    if (targets.Count > 0) 
                    {
                        Debug.Log("preemptive trigger targets found");
                        PreemptiveTriggerContainer newContainer = new PreemptiveTriggerContainer(trigger, combatant, targets);
                        if (trigger.IsProtect)
                        {
                            protectTriggers.Add(newContainer);
                        }
                        else
                        {
                            normalTriggers.Add(newContainer);
                        }
                    }
                    else
                    {
                        Debug.Log("no preemptive trigger targets found");
                    }
                }
                else
                {
                    Debug.Log("preemptive trigger roll failure");
                }
            }
        }

        //remove all protect events w/ lower priority
        if (protectTriggers.Count > 1) 
        {
            List<PreemptiveTriggerContainer> triggerPriority = protectTriggers.OrderByDescending(trigger => trigger.PreemptiveBattleEventTrigger.ProtectPriority).ToList();
            int highestPriority = triggerPriority[0].PreemptiveBattleEventTrigger.ProtectPriority;
            for (int i = triggerPriority.Count - 1; i > 0; i--)
            {
                if (triggerPriority[i].PreemptiveBattleEventTrigger.ProtectPriority < highestPriority)
                {
                    triggerPriority.RemoveAt(i);
                }
            }
            protectTriggers = triggerPriority;
        }

        List<BattleEventTarget> battleEventTargets = new List<BattleEventTarget>();

        //roll if two or more w/ equal priority
        if (protectTriggers.Count > 0)
        {
            int roll = Random.Range(0, protectTriggers.Count);
            PreemptiveTriggerContainer selectedTrigger = protectTriggers[roll];

            //apply modifiers and queue selected protect event
            actionEventToExecute = selectedTrigger.PreemptiveBattleEventTrigger.ApplyModifiers(actionEventToExecute, selectedTrigger.Actor, selectedTrigger.Targets);

            foreach (Combatant target in selectedTrigger.Targets)
            {
                battleEventTargets.Add(new BattleEventTarget(target, 0));
            }
            BattleEvent battleEvent = new BattleEvent(BattleEventType.Targeted, selectedTrigger.Actor, battleEventTargets, selectedTrigger.PreemptiveBattleEventTrigger.TriggerableEffectContainers, selectedTrigger.PreemptiveBattleEventTrigger.EventName, selectedTrigger.PreemptiveBattleEventTrigger.ActionAnimations, selectedTrigger.PreemptiveBattleEventTrigger.TriggerableEffectType);
            battleEventQueue.AddEvent(battleEvent);

            //move to execute position
            if (selectedTrigger.PreemptiveBattleEventTrigger.ExecutePosition == ExecutePosition.TargetRow 
                || selectedTrigger.PreemptiveBattleEventTrigger.ExecutePosition == ExecutePosition.TargetProximity)
            {
                yield return StartCoroutine(selectedTrigger.Actor.Move(selectedTrigger.Targets[0].ProximityTile.transform, "Idle"));
            }
            else if (selectedTrigger.PreemptiveBattleEventTrigger.ExecutePosition == ExecutePosition.FrontCenter)
            {
                yield return StartCoroutine(selectedTrigger.Actor.Move(gridManager.CenterTiles[selectedTrigger.Actor.CombatantType][1].transform, "Idle"));
            }

            //trigger 
            yield return StartCoroutine(battleEventQueue.ExhaustQueueCo());
        }

        //queue all other (non-protect) preemptive events
        battleEventTargets = new List<BattleEventTarget>();
        foreach (PreemptiveTriggerContainer trigger in normalTriggers)
        {
            //apply mods
            actionEventToExecute = trigger.PreemptiveBattleEventTrigger.ApplyModifiers(actionEventToExecute, trigger.Actor, trigger.Targets);

            //queue events
            foreach (Combatant target in trigger.Targets)
            {
                battleEventTargets.Add(new BattleEventTarget(target, 0));
            }
            BattleEvent battleEvent = new BattleEvent(BattleEventType.Targeted, trigger.Actor, battleEventTargets, trigger.PreemptiveBattleEventTrigger.TriggerableEffectContainers, trigger.PreemptiveBattleEventTrigger.EventName, trigger.PreemptiveBattleEventTrigger.ActionAnimations, trigger.PreemptiveBattleEventTrigger.TriggerableEffectType);
            battleEventQueue.AddEvent(battleEvent);
        }

        //trigger events
        yield return StartCoroutine(battleEventQueue.ExhaustQueueCo());

        StartCoroutine(ExecuteActionPhase());
    }

    private IEnumerator ExecuteActionPhase()
    {
        actionPopup.Display(actionEventToExecute.Action.ActionName, actionEventToExecute.Action.Icon);

        //move to execute position
        if (actionEventToExecute.Action.ExecutePosition == ExecutePosition.TargetRow)
        {
            //Tile tile = gridManager.GetRowAttackPosition(actionEventToExecute.ActionSubevents);
            yield return StartCoroutine(actionEventToExecute.Actor.Move(gridManager.CenterTiles[actionEventToExecute.Actor.CombatantType][actionEventToExecute.ActionSubevents[0].Target.Tile.Y].transform, "Idle"));
        }
        else if (actionEventToExecute.Action.ExecutePosition == ExecutePosition.TargetProximity)
        {
            //Tile tile = gridManager.GetRowAttackPosition(actionEventToExecute.ActionSubevents);
            yield return StartCoroutine(actionEventToExecute.Actor.Move(actionEventToExecute.ActionSubevents[0].Target.ProximityTile.transform, "Idle"));
        }
        else if (actionEventToExecute.Action.ExecutePosition == ExecutePosition.FrontCenter)
        {
            yield return StartCoroutine(actionEventToExecute.Actor.Move(gridManager.CenterTiles[actionEventToExecute.Actor.CombatantType][1].transform, "Idle"));
        }

        //"unpause" time
        onInterventionEnd.Raise();

        //execute all hits
        int hitCount = actionEventToExecute.Action.HitCount;
        for (int i = 0; i < hitCount; i++)
        {
            //create copy of subevents each hit (to remove or add targets if needed)
            List<ActionSubevent> actionSubeventsToExecute = new List<ActionSubevent>(actionEventToExecute.ActionSubevents);
            
            //if targeting is random, pick a random target from list of potential targets
            if (actionEventToExecute.Action.AOEType == AOEType.Random)
            {
                int roll = Random.Range(0, actionSubeventsToExecute.Count);
                actionSubeventsToExecute = new List<ActionSubevent>() { actionSubeventsToExecute[roll] };
            }
            //otherwise, apply to all targets (if hitcount >= this hit)
            foreach (ActionAnimationData animationData in actionEventToExecute.Action.ActionAnimations)
            {
                //skip non-repeating animations (ex: spell casts) or skills w/ different multiple hit animations
                if (animationData.AnimationTriggerFrequency == AnimationTriggerFrequency.XHit && animationData.XHit != i 
                    || animationData.AnimationTriggerFrequency == AnimationTriggerFrequency.Even && i % 2 != 0
                    || animationData.AnimationTriggerFrequency == AnimationTriggerFrequency.Odd && i % 2 == 0)
                {
                    continue;
                }

                //wait to trigger? (pause doesn't count towards animation duration)
                yield return new WaitForSeconds(animationData.Delay);

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
                    if (animationData.ChangeActorOpacity)
                    {
                        StartCoroutine(actionEventToExecute.Actor.ChangeOpacityCo(animationData.NewActorOpacity, false));
                    }
                    //yield return wait05;
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
                            Vector2 vfxSpawnPoint = GetVFXPosition(animationData, actionEventToExecute.Actor, actionSubevent.Target);

                            GameObject effectObject = Instantiate(animationData.VFXPrefab, vfxSpawnPoint, animationData.VFXPrefab.transform.rotation);
                            
                            //bind vfx to spawn point?
                            if (animationData.BindVFXToCombatant)
                            {
                                if (animationData.BattlefieldSpawnPosition == BattlefieldSpawnPosition.Actor)
                                {
                                    effectObject.transform.parent = actionSubevent.Actor.CombatantSpawnPositions[CombatantSpawnPosition.Center];
                                }
                                else if (animationData.BattlefieldSpawnPosition == BattlefieldSpawnPosition.Target)
                                {
                                    effectObject.transform.parent = actionSubevent.Target.CombatantSpawnPositions[CombatantSpawnPosition.Center];
                                }
                            }

                            VFXParent actionVFX = effectObject.GetComponent<VFXParent>();
                            if (actionVFX)
                            {
                                //flip animation?
                                bool shouldFlip = false;
                                if (battleTimeline.CurrentTurn.Actor.CombatantType == CombatantType.Enemy)
                                {
                                    shouldFlip = true;
                                }

                                //start animation
                                actionVFX.TriggerAnimation(shouldFlip);

                                //start movement if projectile
                                if (animationData.IsProjectile)
                                {
                                    Vector2 destination = actionSubevent.Target.CombatantSpawnPositions[CombatantSpawnPosition.Center].transform.position;
                                    if (animationData.ReverseProjectile)
                                    {
                                        destination = actionSubevent.Actor.CombatantSpawnPositions[CombatantSpawnPosition.Center].transform.position;
                                    }

                                    StartCoroutine(actionVFX.MoveCo(destination));
                                }
                            }

                        }
                    }
                    //spawn single animation
                    else
                    {
                        Vector2 vfxSpawnPoint = GetVFXPosition(animationData, actionEventToExecute.Actor, actionSubeventsToExecute[0].Target);
                        //vfxSpawnPoints.Add(vfxSpawnPoint, actionSubevents[0].Target);

                        GameObject effectObject = Instantiate(animationData.VFXPrefab, vfxSpawnPoint, animationData.VFXPrefab.transform.rotation);
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
                            if (animationData.BindVFXToCombatant)
                            {
                                if (animationData.BattlefieldSpawnPosition == BattlefieldSpawnPosition.Actor)
                                {
                                    effectObject.transform.parent = actionEventToExecute.Actor.CombatantSpawnPositions[CombatantSpawnPosition.Center];
                                }
                                else if (animationData.BattlefieldSpawnPosition == BattlefieldSpawnPosition.Target)
                                {
                                    effectObject.transform.parent = actionSubeventsToExecute[0].Target.CombatantSpawnPositions[CombatantSpawnPosition.Center];
                                }
                            }

                            //start animation
                            actionVFX.TriggerAnimation(shouldFlip);

                            //start movement if projectile
                            if (animationData.IsProjectile)
                            {
                                Vector2 destination = actionSubeventsToExecute[0].Target.CombatantSpawnPositions[CombatantSpawnPosition.Center].transform.position;
                                if (animationData.ReverseProjectile)
                                {
                                    destination = actionEventToExecute.Actor.CombatantSpawnPositions[CombatantSpawnPosition.Center].transform.position;
                                }

                                StartCoroutine(actionVFX.MoveCo(destination));
                            }
                        }
                    }
                }
                //wait for animations to finish
                yield return new WaitForSeconds(animationData.Duration);
            }
            //pause if action has no animations
            //if (actionEventToExecute.Action.ActionAnimations.Count == 0)
            //{
            //    yield return wait05;
            //}

            //apply damage/heal effect to target(s)
            foreach (ActionSubevent actionSubevent in actionSubeventsToExecute)
            {
                //apply primary effect
                actionSubevent.Execute();
                
                //update enemy weaknesses (only check on first hit)
                if (actionEventToExecute.Action is Attack
                    && actionSubevent.Target is EnemyCombatant
                    && actionSubevent.ActionSummary.Hits == 1)
                {
                    Attack attack = (Attack)actionEventToExecute.Action;
                    EnemyCombatant enemyCombatant = (EnemyCombatant)actionSubevent.Target;
                    EnemyInfo enemyInfo = enemyCombatant.EnemyInfo;

                    //update log
                    SaveManager.Instance.LoadedData.PlayerData.EnemyLog.AddRevealedElement(enemyInfo, attack.ElementalProperty);

                    //display
                    enemyCombatant.DisplayVulnerability(true, attack.ElementalProperty);
                }
            }
            yield return wait025;
        }

        //trigger guard effects
        TriggerGuardEffects();

        //reset any changes to actor sprite opacity
        actionEventToExecute.Actor.SetOpacityToDefault();

        //yield return wait05;
        battleManager.ResetCombatantAnimations();

        //resolve bar changes
        yield return wait025;
        yield return StartCoroutine(battleManager.ResolveBarChanges());

        //trigger other effects
        actionPopup.Hide();
        yield return wait025;
        yield return StartCoroutine(TriggerSecondaryEffects());

        //yield return wait025;
        battleManager.ResetCombatantAnimations();

        //resolve bar changes
        yield return StartCoroutine(battleManager.ResolveBarChanges());

        //move to next phase
        StartCoroutine(ResolveActionPhase());
    }
    private IEnumerator ResolveActionPhase()
    {
        actionPopup.Hide();
        //actionPopupEnemy.Hide();

        //apply HP cost of action
        if (battleTimeline.CurrentTurn.Action.HPPercentCost > 0)
        {
            battleTimeline.CurrentTurn.Actor.ApplyActionHPCost(battleTimeline.CurrentTurn.Action.HPPercentCost);
        }
       
        //apply MP cost of action
        if (battleTimeline.CurrentTurn.Actor is PlayableCombatant && battleTimeline.CurrentTurn.Action.MPCost > 0)
        {
            PlayableCombatant playableCombatant = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;
            if (battleTimeline.CurrentTurn.Action.ConsumeAllMP)
            {
                playableCombatant.OnSpendMana(playableCombatant.MP);
            }
            else
            {
                int modifiedMPCost = ApplyMPCostModifiers(battleTimeline.CurrentTurn.Action, battleTimeline.CurrentTurn.Actor, battleTimeline.CurrentTurn.IsIntervention);
                playableCombatant.OnSpendMana(modifiedMPCost);
            }
        }

        //regen MP when using normal attack or defending
        if (battleTimeline.CurrentTurn.Actor is PlayableCombatant)
        {
            PlayableCombatant playableCombatant = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;
            if (battleTimeline.CurrentTurn.Action == playableCombatant.Attack
                || battleTimeline.CurrentTurn.Action == playableCombatant.Defend)
            {
                float mpPercentageGained = BattleConsts.BaseMPPercentRegen;
                if (battleTimeline.CurrentTurn.IsIntervention)
                {
                    mpPercentageGained = BattleConsts.InterventionMPPercentRegen;
                }
                int mpGained = Mathf.CeilToInt(mpPercentageGained * (float)playableCombatant.MaxMP);

                foreach (float modifier in playableCombatant.UniversalModifiers[BattleEventType.Acting][UniversalModifierType.MPRegen])
                {
                    mpGained = Mathf.CeilToInt(modifier * (float)mpGained);
                }
                playableCombatant.OnGainMana(mpGained);
            }
        }

        //gain intervention points
        if (battleTimeline.CurrentTurn.Actor is PlayableCombatant && battleTimeline.CurrentTurn.IsIntervention)
        {
            PlayableCombatant playableCombatant = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;

            int points = BattleConsts.BaseInterventionPointsGeneration;
            if (battleTimeline.CurrentTurn.Action == playableCombatant.Defend)
            {
                points = BattleConsts.DefendInterventionPointsGeneration;
            }
            playableCombatant.GainInterventionPoints(points);
        }

        yield return StartCoroutine(battleManager.ResolveKOs());

        battleManager.ResetCombatantPositions();
        yield return wait025;

        battleManager.ResetCombatantAnimations();

        //check actor status and move to next phase
        StartCoroutine(EndActionPhase());
    }

    public IEnumerator EndActionPhase()
    {
        Debug.Log("action complete!");

        yield return null;
        //change to turn end state
        stateMachine.ChangeState((int)BattleStateType.TurnEnd);
    }
    #endregion
    /////////////////////////////////////////////////////////

    #region Functions
    private void TriggerGuardEffects()
    {
        //reduce guard
        if (actionEventToExecute.Action is Attack)
        {
            Attack attack = (Attack)actionEventToExecute.Action;
            foreach (ActionSubevent actionSubevent in actionEventToExecute.ActionSubevents)
            {
                if (actionSubevent.Target is EnemyCombatant 
                    && actionSubevent.ActionSummary.Values[ActionSummaryValue.DidHit]
                    && actionSubevent.ActionSummary.CumHealthEffect > 0)
                {
                    EnemyCombatant enemyCombatant = (EnemyCombatant)actionSubevent.Target;
                    int breakAmount = 1 + attack.BreakBonus;
                    if (actionSubevent.ActionSummary.Values[ActionSummaryValue.DidHitWeakness])
                    {
                        //breakAmount = Mathf.CeilToInt((float)breakAmount * BattleConsts.VulnerableBreakMultiplierConst);
                        breakAmount += 1;
                    }
                    breakAmount = Mathf.CeilToInt(ApplyActionModifiers(breakAmount, ActionModifierType.Break, actionSubevent));
                    enemyCombatant.ApplyBreak(breakAmount);
                }
            }
        }
    }

    private IEnumerator TriggerSecondaryEffects()
    {
        //apply turn modifier effects tied to action
        //if (actionEventToExecute.Action.ActorTurnModifier != 0)
        //{
        //    battleTimeline.ApplyTurnModifier(actionEventToExecute.Actor, actionEventToExecute.Action.ActorTurnModifier, false, false, 0, false);
        //}

        //if (actionEventToExecute.Action.TargetTurnModifier != 0)
        //{
        //    foreach (ActionSubevent actionSubevent in actionEventToExecute.ActionSubevents)
        //    {
        //        if (actionSubevent.ActionSummary.Values[ActionSummaryValue.DidHit])
        //        {
        //            battleTimeline.ApplyTurnModifier(actionSubevent.Target, actionSubevent.ActionSummary.Action.TargetTurnModifier, false, false, 0, false);
        //        }
        //    }
        //}

        //action effects
        QueueActorBattleEvents(actionEventToExecute.Action.BattleEventTriggers);

        //queued universal effects
        QueueActorBattleEvents(universalTriggers);

        //queue actor and target trigger effects
        QueueActorBattleEvents(actionEventToExecute.Actor.BattleEventTriggers[BattleEventType.Acting]);
        foreach (ActionSubevent actionSubevent in actionEventToExecute.ActionSubevents)
        {
            QueueTargetBattleEvents(actionSubevent, actionSubevent.Target.BattleEventTriggers[BattleEventType.Targeted]);
        }

        //trigger in order
        yield return StartCoroutine(battleEventQueue.ExhaustQueueCo());

        //update timeline with any changes
        battleTimeline.DisplayTurnOrder();
    }

    private void QueueActorBattleEvents(List<BattleEventTrigger> battleEventTriggers)
    {
        if (actionEventToExecute == null)
        {
            return;
        }

        foreach (BattleEventTrigger thisEventTrigger in battleEventTriggers)
        {
            //get targets
            List<BattleEventTarget> targets = new List<BattleEventTarget>();

            foreach (ActionSubevent actionSubevent in actionEventToExecute.ActionSubevents)
            {
                //check each subevent for matching conditions
                if (thisEventTrigger.TriggerCheck() && thisEventTrigger.ConditionsCheck(actionEventToExecute.Actor, actionSubevent.Target, actionSubevent.ActionSummary))
                {
                    //only roll once for actions that will apply to different targets than the triggering action
                    if (thisEventTrigger.UseTargetOverride)
                    {
                        if (thisEventTrigger.TriggerCheck())
                        {
                            int cumHealthEffect = actionEventToExecute.GetCumHealthEffect();
                            List<Combatant> targetsTemp = battleManager.GetAltTargets(actionEventToExecute.Actor, thisEventTrigger.TargetOverride, thisEventTrigger.PickRandomTarget);
                            
                            foreach (Combatant target in targetsTemp)
                            {
                                targets.Add(new BattleEventTarget(target, cumHealthEffect));
                            }
                        }
                        break;
                    }
                    else if (thisEventTrigger.TriggerCheck())
                    {
                        targets.Add(new BattleEventTarget(actionSubevent.Target, actionSubevent.ActionSummary.CumHealthEffect));
                    }
                }
            }
            //if there are any valid targets, create battle event and add it to queue
            if (targets.Count > 0)
            {
                BattleEvent battleEvent = new BattleEvent(thisEventTrigger.BattleEventType, actionEventToExecute.Actor, targets, thisEventTrigger.TriggerableEffectContainers, thisEventTrigger.EventName, thisEventTrigger.ActionAnimations, thisEventTrigger.TriggerableEffectType);
                battleEventQueue.AddEvent(battleEvent);
            }
        }
    }

    private void QueueTargetBattleEvents(ActionSubevent actionSubevent, List<BattleEventTrigger> battleEventTriggers)
    {
        //get total health effect
        int cumHealthEffect = actionSubevent.ActionSummary.CumHealthEffect;

        foreach (BattleEventTrigger thisTrigger in battleEventTriggers)
        {
            Debug.Log("Checking " + thisTrigger.name + " effect...");

            List<Combatant> targets = new List<Combatant>();
            //check for matching conditions 
            if (thisTrigger.ConditionsCheck(actionSubevent.Target, actionSubevent.Actor, actionSubevent.ActionSummary))
            {
                //only roll once for actions that will apply to different targets (won't actually use this target)
                if (thisTrigger.UseTargetOverride)
                {
                    if (thisTrigger.TriggerCheck())
                    {
                        targets = battleManager.GetAltTargets(actionSubevent.Target, thisTrigger.TargetOverride, thisTrigger.PickRandomTarget);
                    }
                }
                else if (thisTrigger.TriggerCheck())
                {
                    targets.Add(actionSubevent.Actor);
                }
            }
            //if there are any valid targets, create battle event and add it to queue
            if (targets.Count > 0)
            {
                List<BattleEventTarget> battleEventTargets = new List<BattleEventTarget>();
                foreach (Combatant target in targets)
                {
                    battleEventTargets.Add(new BattleEventTarget(target, cumHealthEffect));
                }

                BattleEvent battleEvent = new BattleEvent(thisTrigger.BattleEventType, actionSubevent.Target, battleEventTargets, thisTrigger.TriggerableEffectContainers, thisTrigger.EventName, thisTrigger.ActionAnimations, thisTrigger.TriggerableEffectType);
                battleEventQueue.AddEvent(battleEvent);
            }
        }
    }

    private Vector2 GetVFXPosition(ActionAnimationData actionAnimationData, Combatant actor, Combatant target)
    {
        Vector2 spawnPosition = target.CombatantSpawnPositions[actionAnimationData.CombatantSpawnPosition].transform.position;

        switch (actionAnimationData.BattlefieldSpawnPosition)
        {
            case BattlefieldSpawnPosition.Actor:
                spawnPosition = actor.CombatantSpawnPositions[actionAnimationData.CombatantSpawnPosition].transform.position;
                break;
            case BattlefieldSpawnPosition.TargetedGridCenter:
                spawnPosition = gridManager.GetTileArray(target.CombatantType)[1, 1].transform.position;
                break;
            default:
                break;
        }
        spawnPosition.x += actionAnimationData.SpawnPositionOffset.x;
        spawnPosition.y += actionAnimationData.SpawnPositionOffset.y;
        return spawnPosition;
    }

    private int ApplyMPCostModifiers(Action action, Combatant actor, bool isIntervention)
    {
        float baseValue = (float)action.MPCost;
        ActionSummary actionSummary = new ActionSummary(action, isIntervention);
        //action
        foreach (ActionModifier actionModifier in action.ActionModifierDict[ActionModifierType.MPCost])
        {
            baseValue = actionModifier.ApplyModifier(baseValue, actor, null, actionSummary);
        }
        //action (custom)
        foreach (CustomActionModifier actionModifier in action.CustomActionModifierDict[ActionModifierType.MPCost])
        {
            baseValue = actionModifier.ApplyModifier(baseValue, actor, null, actionSummary);
        }
        //actor
        foreach (ActionModifier actionModifier in actor.ActionModifiers[BattleEventType.Acting][ActionModifierType.MPCost])
        {
            baseValue = actionModifier.ApplyModifier(baseValue, actor, null, actionSummary);
        }
        return Mathf.Clamp(Mathf.FloorToInt(baseValue), 1, 99);
    }

    private float ApplyActionModifiers(float baseValue, ActionModifierType actionModifierType, ActionSubevent actionSubevent)
    {
        //action
        foreach (ActionModifier actionModifier in actionSubevent.ActionSummary.Action.ActionModifierDict[actionModifierType])
        {
            baseValue = actionModifier.ApplyModifier(baseValue, actionSubevent.Actor, actionSubevent.Target, actionSubevent.ActionSummary);
        }
        //action (custom)
        foreach (CustomActionModifier actionModifier in actionSubevent.ActionSummary.Action.CustomActionModifierDict[actionModifierType])
        {
            baseValue = actionModifier.ApplyModifier(baseValue, actionSubevent.Actor, actionSubevent.Target, actionSubevent.ActionSummary);
        }
        //actor
        foreach (ActionModifier actionModifier in actionSubevent.Actor.ActionModifiers[BattleEventType.Acting][actionModifierType])
        {
            baseValue = actionModifier.ApplyModifier(baseValue, actionSubevent.Actor, actionSubevent.Target, actionSubevent.ActionSummary);
        }
        //target
        foreach (ActionModifier actionModifier in actionSubevent.Target.ActionModifiers[BattleEventType.Targeted][actionModifierType])
        {
            baseValue = actionModifier.ApplyModifier(baseValue, actionSubevent.Target, actionSubevent.Actor, actionSubevent.ActionSummary);
        }
        return baseValue;
    }
    #endregion
}