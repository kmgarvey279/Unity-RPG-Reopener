using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class BattleEvent
{
    //data
    public BattleEventType BattleEventType { get; protected set; }
    public TriggerableEffectType TriggerableEffectType { get; protected set; }
    public Combatant Actor { get; protected set; }
    public List<BattleEventTarget> Targets { get; protected set; } = new List<BattleEventTarget>();
    public List<TriggerableEffectContainer> TriggerableEffectContainers { get; private set; }
    //display
    public string EffectName { get; private set; }
    //animation
    public List<ActionAnimationData> ActionAnimations { get; private set; } = new List<ActionAnimationData>();

    public BattleEvent(BattleEventType _battleEventType, Combatant _actor, List<BattleEventTarget> _targets, List<TriggerableEffectContainer> _triggerableEffectContainers, string _effectName, List<ActionAnimationData> _actionAnimations, TriggerableEffectType _triggerableEffectType)
    {
        BattleEventType = _battleEventType;
        Actor = _actor;
        Targets = _targets;
        TriggerableEffectContainers = _triggerableEffectContainers;
        EffectName = _effectName;
        ActionAnimations = _actionAnimations;
        TriggerableEffectType = _triggerableEffectType;
    }
}

public class BattleEventQueue : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private BattleTimeline battleTimeline;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private ActionPopup effectPopup;
    //[SerializeField] private ActionPopup effectPopupEnemy;
    private WaitForSeconds wait025 = new WaitForSeconds(0.25f);
    private WaitForSeconds wait05 = new WaitForSeconds(0.5f);
    private WaitForSeconds wait125 = new WaitForSeconds(1.25f);
    [field: SerializeField] public List<BattleEvent> Events { get; private set; } = new List<BattleEvent>();
    [SerializeField] private Sprite playerTriggerIcon;
    [SerializeField] private Sprite enemyTriggerIcon;
    [SerializeField] private Sprite turnIcon;
    [SerializeField] private Sprite buffIcon;
    [SerializeField] private Sprite debuffIcon;

    public void AddEvent(BattleEvent battleEvent)
    {
        Events.Add(battleEvent);
    }

    public void RemoveAllEvents()
    {
        Events.Clear();
    }

    //public List<Combatant> GetAltTargets(Combatant actor, TargetingType targetingType, bool pickRandomTarget)
    //{
    //    List<Combatant> targets = new List<Combatant>();
    //    switch (targetingType)
    //    {
    //        case TargetingType.TargetSelf:
    //            targets.Add(actor);
    //            break;
    //        case TargetingType.TargetAllies:
    //            targets.AddRange(battleManager.GetCombatants(actor.CombatantType));
    //            targets.Remove(actor);
    //            break;
    //        case TargetingType.TargetKOAllies:
    //            targets.AddRange(battleManager.GetCombatants(actor.CombatantType, true));
    //            break;
    //        case TargetingType.TargetFriendly:
    //            targets.AddRange(battleManager.GetCombatants(actor.CombatantType));
    //            break;
    //        case TargetingType.TargetHostile:
    //            if (actor.CombatantType == CombatantType.Player)
    //            {
    //                targets.AddRange(battleManager.GetCombatants(CombatantType.Enemy));
    //            }
    //            else
    //            {
    //                targets.AddRange(battleManager.GetCombatants(CombatantType.Player));
    //            }
    //            break;
    //        default:
    //            break;
    //    }
    //    if (targets.Count > 1 && pickRandomTarget)
    //    {
    //        int roll = Random.Range(0, targets.Count);
    //        targets = new List<Combatant>() { targets[roll] };
    //    }
    //    return targets;
    //}

    public IEnumerator ExhaustQueueCo()
    {
        //sort by priority
        Events = Events.OrderBy(e => e.TriggerableEffectType).ToList();
        int loopCap = 200;
        int loops = 0;
        while (Events.Count > 0)
        {
            BattleEvent currentEvent = Events[0];

            if (currentEvent.Targets.Count > 0)
            {
                Debug.Log("Triggering Event: " + currentEvent.EffectName);
                //display name when triggered?
                if (currentEvent.EffectName != "")
                {
                    //if (currentEvent.Actor is PlayableCombatant)
                    //{
                    if (currentEvent.TriggerableEffectType == TriggerableEffectType.Turn)
                    {
                        effectPopup.Display(currentEvent.EffectName, turnIcon);
                    }
                    else if (currentEvent.TriggerableEffectType == TriggerableEffectType.Buff)
                    {
                        effectPopup.Display(currentEvent.EffectName, buffIcon);
                    }
                    else if (currentEvent.TriggerableEffectType == TriggerableEffectType.Debuff)
                    {
                        effectPopup.Display(currentEvent.EffectName, debuffIcon);
                    }
                    else
                    {
                        if (currentEvent.Actor is PlayableCombatant)
                        {
                            effectPopup.Display(currentEvent.EffectName, playerTriggerIcon);
                        }
                        else
                        {
                            effectPopup.Display(currentEvent.EffectName, enemyTriggerIcon);
                        }
                    }
                    //}
                    //else
                    //{
                    //    effectPopupEnemy.Display(currentEvent.EffectName);
                    //}
                    yield return wait05;
                }

                //trigger animations
                foreach (ActionAnimationData animationData in currentEvent.ActionAnimations)
                {
                    //wait to trigger? (pause doesn't count towards animation duration)
                    yield return new WaitForSeconds(animationData.Delay);

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
                        if (animationData.ChangeActorOpacity)
                        {
                            StartCoroutine(currentEvent.Actor.ChangeOpacityCo(animationData.NewActorOpacity, false));
                        }
                    }

                    //spawn animation visual effects
                    if (animationData.VFXPrefab != null)
                    {
                        //if spawn multiple animations at each target
                        if (animationData.SpawnForEachTarget)
                        {
                            foreach (BattleEventTarget battleEventTarget in currentEvent.Targets)
                            {
                                Combatant target = battleEventTarget.Combatant;

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

                                    //bind vfx to spawn point?
                                    if (animationData.BindVFXToCombatant)
                                    {
                                        if (animationData.BattlefieldSpawnPosition == BattlefieldSpawnPosition.Actor)
                                        {
                                            effectObject.transform.parent = currentEvent.Actor.CombatantSpawnPositions[CombatantSpawnPosition.Center];
                                        }
                                        else if (animationData.BattlefieldSpawnPosition == BattlefieldSpawnPosition.Target)
                                        {
                                            effectObject.transform.parent = target.CombatantSpawnPositions[CombatantSpawnPosition.Center];
                                        }
                                    }

                                    //start animation
                                    actionVFX.TriggerAnimation(shouldFlip);

                                    //start movemenet if projectile
                                    if (animationData.IsProjectile)
                                    {
                                        Vector2 destination = target.CombatantSpawnPositions[CombatantSpawnPosition.Center].transform.position;
                                        if (animationData.ReverseProjectile)
                                        {
                                            destination = currentEvent.Actor.CombatantSpawnPositions[CombatantSpawnPosition.Center].transform.position;
                                        }
                                        StartCoroutine(actionVFX.MoveCo(destination));
                                    }
                                }
                            }
                        }
                        else if (currentEvent.Targets.Count > 0)
                        {
                            Combatant firstTarget = currentEvent.Targets[0].Combatant;

                            Vector2 vfxSpawnPoint = GetVFXPosition(animationData, currentEvent.Actor, firstTarget);

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

                                //bind vfx to spawn point?
                                if (animationData.BindVFXToCombatant)
                                {
                                    if (animationData.BattlefieldSpawnPosition == BattlefieldSpawnPosition.Actor)
                                    {
                                        effectObject.transform.parent = currentEvent.Actor.CombatantSpawnPositions[CombatantSpawnPosition.Center];
                                    }
                                    else if (animationData.BattlefieldSpawnPosition == BattlefieldSpawnPosition.Target)
                                    {
                                        effectObject.transform.parent = currentEvent.Targets[0].Combatant.CombatantSpawnPositions[CombatantSpawnPosition.Center];
                                    }
                                }

                                //start animation
                                actionVFX.TriggerAnimation(shouldFlip);

                                //start movemenet if projectile
                                if (animationData.IsProjectile)
                                {
                                    Vector2 destination = firstTarget.CombatantSpawnPositions[CombatantSpawnPosition.Center].transform.position;
                                    if (animationData.ReverseProjectile)
                                    {
                                        destination = currentEvent.Actor.CombatantSpawnPositions[CombatantSpawnPosition.Center].transform.position;
                                    }

                                    StartCoroutine(actionVFX.MoveCo(destination));
                                }
                            }
                        }
                    }
                    //wait for animations to finish
                    yield return new WaitForSeconds(animationData.Duration);
                }
                //yield return wait125;

                //trigger effects
                foreach (TriggerableEffectContainer triggerableEffectContainer in currentEvent.TriggerableEffectContainers)
                {
                    foreach (BattleEventTarget target in currentEvent.Targets)
                    {
                        triggerableEffectContainer.TriggerEffect(currentEvent.Actor, target.Combatant, target.ValueOverride);
                        //yield return wait025;
                    }
                    //yield return wait05;
                    yield return wait025;
                }
                effectPopup.Hide();
                //yield return wait025;
                //effectPopupEnemy.Hide();

                //reset sprites
                currentEvent.Actor.SetOpacityToDefault();
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
}
