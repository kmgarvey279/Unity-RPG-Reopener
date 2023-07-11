using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]

public class BattleEvent
{
    public Combatant Actor { get; protected set; }
    public TriggerableBattleEffect TriggerableBattleEffect { get; protected set; }
    public bool WasTriggered { get; protected set; }
    public BattleEvent(Combatant _actor, TriggerableBattleEffect _triggerableBattleEffect)
    {
        Actor = _actor;
        TriggerableBattleEffect = _triggerableBattleEffect;
    }

    public virtual bool TriggerCheck()
    {
        return true;
    }

    public virtual void TriggerEffects()
    {
    }

    public virtual List<Combatant> GetTargets()
    {
        return new List<Combatant>();
    }
}

//triggered when combatatant acts in battle
public class BattleEventActor : BattleEvent
{
    public ActionEvent ActionEvent { get; private set; }
    public List<ActionSubevent> triggeredSubevents = new List<ActionSubevent>();
    public BattleEventActor(Combatant _actor, TriggerableBattleEffect _triggerableBattleEffect, ActionEvent _actionEvent) : base(_actor, _triggerableBattleEffect)
    {
        ActionEvent = _actionEvent;
        List<ActionSubevent> validSubevents = new List<ActionSubevent>();
        //check if effect is valid
        foreach (ActionSubevent actionSubevent in ActionEvent.ActionSubevents)
        {
            if (TriggerableBattleEffect.ConditionCheck(Actor, actionSubevent.Target, actionSubevent.ActionSummary))
            {
                validSubevents.Add(actionSubevent);
            }
        }
        //roll to see if effect is triggered
        foreach (ActionSubevent actionSubevent in validSubevents)
        {
            if (TriggerableBattleEffect.TriggerCheck())
            {
                if (!WasTriggered)
                {
                    WasTriggered = true;
                }
                triggeredSubevents.Add(actionSubevent);
                //only roll once for actions that target the actor
                if (TriggerableBattleEffect.TargetSelf)
                {
                    break;
                }
            }
        }
    }

    public override void TriggerEffects()
    {
        base.TriggerEffects();

        if (WasTriggered)
        {
            if (TriggerableBattleEffect.TargetSelf)
            {
                TriggerableBattleEffect.ApplyEffect(Actor, Actor, ActionEvent.GetActorActionSummary());
            }
            else
            {
                foreach (ActionSubevent actionSubevent in triggeredSubevents)
                {
                    TriggerableBattleEffect.ApplyEffect(Actor, actionSubevent.Target, actionSubevent.ActionSummary);
                }
            }
        }
    }

    public override List<Combatant> GetTargets()
    {
        List<Combatant> targets = new List<Combatant>();
        if (triggeredSubevents.Count > 0)
        {
            if (TriggerableBattleEffect.TargetSelf)
            {
                targets.Add(Actor);
            }
            else
            {
                foreach (ActionSubevent actionSubevent in triggeredSubevents)
                {
                    targets.Add(actionSubevent.Target);
                }
            }
        }
        return targets;
    }
}

//triggered when combatatant is targeted in battle
public class BattleEventTarget : BattleEvent
{
    public ActionSubevent ActionSubevent { get; private set; }
    public BattleEventTarget(Combatant _actor, TriggerableBattleEffect _triggerableBattleEffect, ActionSubevent _actionSubevent) : base(_actor, _triggerableBattleEffect)
    {
        ActionSubevent = _actionSubevent;
        if (TriggerableBattleEffect.ConditionCheck(Actor, ActionSubevent.Actor, ActionSubevent.ActionSummary) && TriggerableBattleEffect.TriggerCheck())
        {
            WasTriggered = true;
        }
    }

    public override void TriggerEffects()
    {
        base.TriggerEffects();
        //trigger effects
        if (WasTriggered)
        {
            if (TriggerableBattleEffect.TargetSelf)
            {
                TriggerableBattleEffect.ApplyEffect(Actor, Actor, ActionSubevent.ActionSummary);
            }
            else
            {
                TriggerableBattleEffect.ApplyEffect(Actor, ActionSubevent.Actor, ActionSubevent.ActionSummary);
            }
        }
    }

    public override List<Combatant> GetTargets()
    {
        List<Combatant> targets = new List<Combatant>();
        if (WasTriggered)
        {
            if (TriggerableBattleEffect.TargetSelf)
            {
                targets.Add(Actor);
            }
            else
            {
                targets.Add(ActionSubevent.Actor);
            }
        }
        return targets;
    }
}

//triggered on combatant's turn
public class BattleEventTurn : BattleEvent
{
    public BattleEventTurn(Combatant _actor, TriggerableBattleEffect _triggerableBattleEffect) : base(_actor, _triggerableBattleEffect) 
    {
        if (TriggerableBattleEffect.ConditionCheck(Actor, Actor, null) && TriggerableBattleEffect.TriggerCheck())
        {
            WasTriggered = true;
        }
    }

    public override void TriggerEffects()
    {
        base.TriggerEffects();
        //trigger effects
        if (WasTriggered)
        {
            if (TriggerableBattleEffect.TargetSelf)
            {
                TriggerableBattleEffect.ApplyEffect(Actor, Actor, null);
            }
        }
    }

    public override List<Combatant> GetTargets()
    {
        List<Combatant> targets = new List<Combatant>();
        if (WasTriggered)
        {
            targets.Add(Actor);
        }
        return targets;
    }
}

public class BattleEventQueue : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private BattleTimeline battleTimeline;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private ActionPopup effectPopup;
    private WaitForSeconds waitShort = new WaitForSeconds(0.15f);
    private WaitForSeconds waitMid = new WaitForSeconds(0.5f);
    [field: SerializeField] public List<BattleEvent> Events { get; private set; } = new List<BattleEvent>();

    public void AddEvent(BattleEvent battleEvent)
    {
        Events.Add(battleEvent);
    }

    public void RemoveAllEvents()
    {
        Events.Clear();
    }

    public IEnumerator ExhaustQueueCo()
    {
        //sort by priority
        Events = Events.OrderBy(e => e.TriggerableBattleEffect.Priority).ToList();
        int loopCap = 200;
        int loops = 0;
        while (Events.Count > 0)
        {
            BattleEvent currentEvent = Events[0];
            if (currentEvent.WasTriggered)
            {
                if (currentEvent.TriggerableBattleEffect.DisplayName)
                {
                    effectPopup.Display(currentEvent.TriggerableBattleEffect.EffectName);
                }

                List<Combatant> targets = currentEvent.GetTargets();

                //trigger animations
                foreach (ActionAnimationData animationData in currentEvent.TriggerableBattleEffect.ActionAnimations)
                {

                    //trigger actor sprite animation
                    if (animationData.BattleAnimatorTrigger != BattleAnimatorTrigger.None)
                    {
                        if (animationData.BattleAnimatorTrigger == BattleAnimatorTrigger.Custom)
                        {
                            currentEvent.Actor.TriggerAnimation(animationData.CustomAnimatorTrigger, false);
                        }
                        else
                        {
                            currentEvent.Actor.TriggerAnimation(animationData.BattleAnimatorTrigger.ToString(), false);
                        }
                    }

                    //spawn animation visual effects
                    if (animationData.VFXPrefab != null)
                    {
                        //if spawn multiple animations at each target
                        if (animationData.SpawnForEachTarget)
                        {
                            foreach (Combatant target in targets)
                            {
                                Vector2 vfxSpawnPoint = GetVFXPosition(animationData, currentEvent.Actor, target);

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

                                    //start animation
                                    actionVFX.TriggerAnimation(shouldFlip);

                                    //start movemenet if projectile
                                    if (animationData.IsProjectile)
                                    {
                                        Vector2 destination = target.CombatantBindPositions[CombatantBindPosition.Center].transform.position;
                                        StartCoroutine(actionVFX.MoveCo(destination));
                                    }
                                }
                            }
                        }
                        else
                        {
                            Vector2 vfxSpawnPoint = GetVFXPosition(animationData, currentEvent.Actor, targets[0]);

                            GameObject effectObject = Instantiate(animationData.VFXPrefab, vfxSpawnPoint, animationData.VFXPrefab.transform.rotation);
                            VFXParent actionVFX = effectObject.GetComponent<VFXParent>();

                            if (actionVFX)
                            {
                                //flip animation?
                                bool shouldFlip = false;
                                if (currentEvent.Actor.CombatantType == CombatantType.Enemy)
                                {
                                    shouldFlip = true;
                                }

                                //start animation
                                actionVFX.TriggerAnimation(shouldFlip);
                            }
                        }
                    }
                    //wait for animations to finish
                    yield return new WaitForSeconds(animationData.Duration);
                }
                //trigger effects
                currentEvent.TriggerEffects();

                yield return new WaitForSeconds(0.75f);
                effectPopup.Hide();
            }
            //remove event
            Events.RemoveAt(0);
            loops++;

            //safeguard
            if (loops > loopCap)
            {
                RemoveAllEvents();
                break;
            }
            yield return null;
        }
    }

    private Vector2 GetVFXPosition(ActionAnimationData actionAnimationData, Combatant actor, Combatant target)
    {
        Vector2 spawnPosition = target.CombatantBindPositions[actionAnimationData.CombatantBindPosition].transform.position;

        switch (actionAnimationData.VFXSpawnPosition)
        {
            case ActionVFXPosition.Actor:
                spawnPosition = actor.CombatantBindPositions[actionAnimationData.CombatantBindPosition].transform.position;
                break;
            case ActionVFXPosition.TargetedGridCenter:
                spawnPosition = gridManager.GetTileArray(target.CombatantType)[1, 1].transform.position;
                break;
            default:
                break;
        }
        return spawnPosition;
    }
}
