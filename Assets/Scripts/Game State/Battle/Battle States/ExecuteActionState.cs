using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class ExecuteActionState : BattleState
{   
    [Header("Events (Signals)")]
    // [SerializeField] private SignalSenderGO onCameraZoomIn;
    private ActionEvent actionEventToExecute;
    private List<ActionEvent> targetQueue = new List<ActionEvent>();
    // private List<ActionEvent> followupQueue = new List<ActionEvent>();
    private List<Combatant> counterQueue = new List<Combatant>();

    private void Start()
    {
        battleStateType = BattleStateType.Execute;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //clear counter queue if it didn't fully clear last turn
        if(counterQueue.Count > 0)
            counterQueue.Clear();

        //copy current action event
        actionEventToExecute = turnData.actionEvent.Clone();

        //apply costs of action
        battleManager.ApplyAction();
        
        // onCameraZoomIn.Raise(turnData.targetedTile.gameObject);
        StartCoroutine(ExecuteAction());
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

    public IEnumerator ExecuteAction()
    {
        Debug.Log(actionEventToExecute.actor.characterName + " used " + actionEventToExecute.action.actionName);

        //move into melee range
        if(actionEventToExecute.action.isMelee)
        {
            actionEventToExecute.actor.Move(gridManager.centerPositions[actionEventToExecute.primaryTarget.tile.y][actionEventToExecute.actor.combatantType], "Idle", 2f);
            yield return new WaitUntil(() => !actionEventToExecute.actor.moving);
        }

        //apply all of the actor's modifiers to action event
        Combatant actor = actionEventToExecute.actor;
        foreach(ActionEventModifier eventModifier in actor.actionEventModifiers[EventTriggerType.OnAct])
        {
            actionEventToExecute = eventModifier.ApplyModifiers(actionEventToExecute, actor);
            // if(eventModifier.removeOnApply)
            // {
            //     actor.actionEventModifiers[EventTriggerType.OnAct].Remove(eventModifier);
            // }
        }

        //if targeting is random, pick a random target from list of potential targets
        if(actionEventToExecute.action.hitRandomTarget)
        {
            List<Combatant> potentialTargets = battleManager.GetCombatants(actionEventToExecute.GetCombatantType());
            int roll = Mathf.FloorToInt(Random.Range(0, potentialTargets.Count + 1));
            actionEventToExecute.primaryTarget = potentialTargets[roll];
            actionEventToExecute.targets = new List<Combatant>(){potentialTargets[roll]};
        }

        //apply event modifiers for all allies (ex: block incoming attack)
        List<Combatant> allyCombatants = battleManager.GetCombatants(actionEventToExecute.combatantType);
        //shuffle
        for(int i = 0; i < allyCombatants.Count; i++) 
        {
            Combatant temp = allyCombatants[i];
            int randomIndex = Random.Range(i, allyCombatants.Count);
            allyCombatants[i] = allyCombatants[randomIndex];
            allyCombatants[randomIndex] = temp;
        }
        foreach(Combatant ally in allyCombatants)
        {
            foreach(ActionEventModifier eventModifier in ally.actionEventModifiers[EventTriggerType.OnPartyTargeted])
            {
                actionEventToExecute = eventModifier.ApplyModifiers(actionEventToExecute, ally);
                // if(eventModifier.removeOnApply)
                // {
                //     ally.actionEventModifiers[EventTriggerType.OnAllyTargeted].Remove(eventModifier);
                // }
            }
        }

        foreach(Combatant target in actionEventToExecute.targets)
        {
            //split action into multiple subactions, each w/ 1 target & apply target modifiers
            ActionEvent tempActionEvent = actionEventToExecute.Clone();
            tempActionEvent.targets = new List<Combatant>(){target};
            //apply event modifiers for target
            foreach(ActionEventModifier eventModifier in target.actionEventModifiers[EventTriggerType.OnTargeted])
            {
                tempActionEvent = eventModifier.ApplyModifiers(tempActionEvent, target);
                // if(eventModifier.removeOnApply)
                // {
                //     target.actionEventModifiers[EventTriggerType.OnTargeted].Remove(eventModifier);
                // }
            }
            targetQueue.Add(tempActionEvent);
        }
        //start action
        StartCoroutine(CastAnimationPhase());
    }


    //trigger action animation + visual effect on user
    public IEnumerator CastAnimationPhase()
    {
        if(actionEventToExecute.action.hasCastAnimation)
        {
            Debug.Log("Cast Phase");
            //cast animation
            if(actionEventToExecute.action.castAnimatorTrigger != "")
            {
                actionEventToExecute.actor.animator.SetTrigger(actionEventToExecute.action.castAnimatorTrigger);
            }
            //spawn casting effect
            if(actionEventToExecute.action.castGraphicPrefab != null)
            {
                yield return new WaitForSeconds(actionEventToExecute.action.castGraphicDelay); 
                Vector3 spawnPosition = actionEventToExecute.actor.transform.position;
                GameObject graphicObject = Instantiate(actionEventToExecute.action.castGraphicPrefab, spawnPosition, Quaternion.identity);
            }
            yield return new WaitForSeconds(actionEventToExecute.action.castAnimationDuration);
            StartCoroutine(ActionAnimationPhase()); 
        }
        else
        {
            yield return null;
            StartCoroutine(ActionAnimationPhase()); 
        }
    }

    private IEnumerator ActionAnimationPhase()
    {
        Debug.Log("starting animation");
        //user animation
        if(actionEventToExecute.action.executeAnimatorTrigger != "")
        {
            actionEventToExecute.actor.animator.SetTrigger(actionEventToExecute.action.executeAnimatorTrigger);
        }
        yield return new WaitForSeconds(actionEventToExecute.action.effectGraphicDelay);
        StartCoroutine(ProjectileAnimationPhase());         
    }

    private IEnumerator ProjectileAnimationPhase()
    {
        Debug.Log("Projectile Phase");
        if(actionEventToExecute.action.hasProjectileAnimation && actionEventToExecute.action.projectileGraphicPrefab != null)
        {
            Vector3 startPosition = new Vector3(actionEventToExecute.actor.tile.transform.position.x + (actionEventToExecute.actor.GetDirection().x * 0.1f), actionEventToExecute.actor.transform.position.y + (actionEventToExecute.actor.GetDirection().y * 0.1f));
            GameObject projectileObject = Instantiate(actionEventToExecute.action.projectileGraphicPrefab, startPosition, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Move(actionEventToExecute.primaryTarget.tile.transform.position);
            
            yield return new WaitUntil(() => projectile.reachedTarget);
            Destroy(projectileObject);
        }
        yield return null;
        StartCoroutine(EffectAnimationPhase());
    }

    private IEnumerator EffectAnimationPhase()
    {
        Debug.Log("Effect Phase");
        //spawn visual effect
        if(actionEventToExecute.action.effectGraphicPrefab)
        {
            Vector2 spawnPosition = turnData.actionEvent.primaryTarget.transform.position; 
            if(actionEventToExecute.action.aoeType == AOEType.All)
            {
                spawnPosition = gridManager.GetTileArray(actionEventToExecute.combatantType)[1, 1].transform.position;
            }
            GameObject graphicObject = Instantiate(actionEventToExecute.action.effectGraphicPrefab, spawnPosition, actionEventToExecute.action.effectGraphicPrefab.transform.rotation);
            //destroy visual effect
            yield return new WaitForSeconds(actionEventToExecute.action.effectAnimationDuration);
            Destroy(graphicObject);
        }
        else 
        {
            yield return new WaitForSeconds(actionEventToExecute.action.effectAnimationDuration);
        }
        StartCoroutine(ApplyActionEffects());
    }

    private IEnumerator ApplyActionEffects()
    {
        foreach(ActionEvent actionEvent in targetQueue)
        {
            actionEvent.TriggerEvent();
            // foreach(ActionEffectTrigger actionEffectTrigger in actionEvent.action.actionEffectTriggers)
            // {
            //     if(actionEffectTrigger.actionEffect is ActionEffectHealth)
            // {
            // if(actionEvent.action.actionEffectTriggers == ActionType.Attack && !counterQueue.Contains(actionEvent.targets[0]))
            // {
            //     counterQueue.Add(actionEvent.targets[0]);
            // }
        }
        actionEventToExecute.hitCounter++;

        yield return new WaitForSeconds(0.5f);

        if(actionEventToExecute.hitCounter < actionEventToExecute.action.hitCount)
        {
            Debug.Log("more hits remain");
            StartCoroutine(ProjectileAnimationPhase());
        }
        else
        {
            Debug.Log("all hits complete");
            StartCoroutine(ResolveActionPhase());
        }
    }

    private IEnumerator ResolveActionPhase()
    {
        if(actionEventToExecute.action.isMelee)
        {
            actionEventToExecute.actor.Move(actionEventToExecute.actor.tile.transform, "Idle");
        }

        //complete animations
        List<Combatant> combatants = battleManager.GetCombatants(CombatantType.Player);
        combatants.AddRange(battleManager.GetCombatants(CombatantType.Enemy));
        foreach(Combatant combatant in combatants)
        {
            combatant.ReturnToDefaultAnimation(); 
            combatant.ResolveHealthChange();
        }
        yield return new WaitForSeconds(0.5f);

        //check targets for ko
        bool actorKO = false;
        for(int i = combatants.Count - 1; i > 0; i--)
        {
            Combatant thisCombatant = combatants[i];
            if(thisCombatant.hp.GetCurrentValue() <= 0)
            {
                battleManager.KOCombatant(thisCombatant);
                combatants.RemoveAt(i);
                //remove combatant from counter queue
                for(int j = counterQueue.Count - 1; i > 0; i--)
                {
                    if(counterQueue[j] == thisCombatant)
                    {
                        counterQueue.RemoveAt(j);
                    }
                }
                if(thisCombatant == turnData.combatant)
                {
                    actorKO = true;
                }
            }
        }
        yield return new WaitForSeconds(0.5f);

        //hide hp bars
        // foreach(Combatant combatant in combatants)
        // {
        //     combatant.ToggleHPBar(false);
        // }

        //move to next phase
        if(actorKO)
        {
            StartCoroutine(EndActionPhase());
        }
        else
        {
            CounterPhase();
        }
    }

    private void CounterPhase()
    {
        Debug.Log("Counter phase");
        for(int x = counterQueue.Count - 1; x >= 0; x--)
        {
            CounterAction counterAction = counterQueue[0].counterAction;
            if(counterAction != null && Roll(counterAction.chance))
            {
                //create counter action
                actionEventToExecute = new ActionEvent(counterAction.action, counterQueue[x]);
                actionEventToExecute.primaryTarget = turnData.combatant;
                actionEventToExecute.targets = new List<Combatant>() {turnData.combatant};
                actionEventToExecute.canCounter = false;
                //remove from queue
                counterQueue.RemoveAt(x);
                //execute
                StartCoroutine(ExecuteAction());
                return;
            }
        }
        StartCoroutine(EndActionPhase());
    }

    public IEnumerator EndActionPhase()
    {
        Debug.Log("action complete!");  

        yield return new WaitForSeconds(0.6f);
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
}