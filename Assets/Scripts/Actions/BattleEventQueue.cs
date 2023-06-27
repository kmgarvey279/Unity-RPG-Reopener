using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class BattleEvent
{
    public Combatant Actor { get; private set; }
    public Dictionary<Combatant, float> Targets { get; private set; }
    public TriggerableBattleEffect TriggerableBattleEffect { get; private set; }
    public BattleEvent(Combatant _actor, Dictionary<Combatant, float> _targets, TriggerableBattleEffect _triggerableBattleEffect)
    {
        Actor = _actor;
        Targets = _targets;
        TriggerableBattleEffect = _triggerableBattleEffect;
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
        int eventCap = 200;
        int eventsTriggered = 0;
        while (Events.Count > 0)
        {
            BattleEvent currentEvent = Events[0];
            if (currentEvent.TriggerableBattleEffect.DisplayName)
            {
                effectPopup.Display(currentEvent.TriggerableBattleEffect.EffectName);
            }

            Dictionary<Combatant, float> targets = currentEvent.Targets;
            if (currentEvent.TriggerableBattleEffect.TargetSelf)
            {
                targets = new Dictionary<Combatant, float>() { { currentEvent.Actor, 0 } };
            }

            //trigger animations
            foreach (ActionAnimationData animationData in currentEvent.TriggerableBattleEffect.ActionAnimations)
            {
                
                //trigger actor sprite animation
                if (animationData.BattleAnimatorTrigger != BattleAnimatorTrigger.None)
                {
                    if(animationData.BattleAnimatorTrigger == BattleAnimatorTrigger.Custom)
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
                        foreach (KeyValuePair<Combatant, float> target in targets)
                        {
                            Vector2 vfxSpawnPoint = GetVFXPosition(animationData, currentEvent.Actor, target.Key);

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
                                    Vector2 destination = target.Key.CombatantBindPositions[CombatantBindPosition.Center].transform.position;
                                    StartCoroutine(actionVFX.MoveCo(destination));
                                }
                            }
                        }
                    }
                    else
                    {
                        Vector2 vfxSpawnPoint = GetVFXPosition(animationData, currentEvent.Actor, currentEvent.Actor);

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
                        }
                    }
                }
                //wait for animations to finish
                yield return animationData.Duration;
            }

            //trigger effects
            foreach (KeyValuePair<Combatant, float> target in targets)
            {
                currentEvent.TriggerableBattleEffect.ApplyEffect(currentEvent.Actor, target.Key, target.Value);
                foreach(TriggerableBattleEffect additionalEffect in currentEvent.TriggerableBattleEffect.AdditionalEffects)
                {
                    additionalEffect.ApplyEffect(currentEvent.Actor, target.Key, target.Value);
                }
            }

            //remove event
            Events.RemoveAt(0);
            eventsTriggered++;

            effectPopup.Hide();

            //safeguard
            if (eventsTriggered > eventCap)
            {
                RemoveAllEvents();
                break;
            }
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
