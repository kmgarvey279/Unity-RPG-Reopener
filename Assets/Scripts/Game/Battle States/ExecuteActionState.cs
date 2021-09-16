using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using BattleCalculationsNamespace;

[System.Serializable]
public class ExecuteActionState : BattleState
{
    private TurnData turnData;
    [SerializeField] private GridManager gridManager;
    private BattleCalculations battleCalculations;
    
    [Header("Events (Signals)")]
    [SerializeField] private SignalSenderGO onCameraZoomIn;

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Eneter execute state");
        battleCalculations = new BattleCalculations();
        
        turnData = battleManager.turnData;

        // onCameraZoomIn.Raise(turnData.targetedTile.gameObject);
        ExecuteAction();
    }

    public override void StateUpdate()
    {
        
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {

    }

    public void ExecuteAction()
    {
        Debug.Log(turnData.combatant.characterName + " used " + turnData.action.actionName);
        if(turnData.targetedTile != null && turnData.targetedTile != turnData.combatant.tile)
        {
            turnData.combatant.FaceTarget(turnData.targetedTile.transform);
        }
        TriggerAnimation();
    }

    public void TriggerAnimation()
    {
        if(turnData.action.animatorTrigger != null)
        {
            turnData.combatant.animator.SetTrigger(turnData.action.animatorTrigger);
        }
        StartCoroutine(SpawnGraphicCo());
    }

    public IEnumerator SpawnGraphicCo()
    {
        yield return new WaitForSeconds(0.25f);
        
        GameObject graphicObject = null;
        if(turnData.action.effectGraphicPrefab != null)
        {
            graphicObject = Instantiate(turnData.action.effectGraphicPrefab, turnData.targetedTile.transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(TriggerActionEffectsCo());
        // if(graphicObject.GetComponent<ActionGraphic>() is ProjectileGraphic)
        // {
        //     ProjectileGraphic projectileGraphic = graphicObject.GetComponent<ProjectileGraphic>();
        //     projectileGraphic.destination = destination;
        // }
    }

    public IEnumerator TriggerActionEffectsCo()
    {
        if(turnData.targets.Count > 0)
        {
            foreach(Combatant target in turnData.targets)
            {
                bool didHit = battleCalculations.HitCheck(turnData.action, turnData.combatant, target, gridManager.GetMoveCost(turnData.combatant.tile, target.tile));
                if(didHit)
                {
                    Debug.Log(target.characterName + " was hit!");
                    foreach (ActionEffect effect in turnData.action.effects)
                    {
                        effect.ApplyEffect(turnData.action, turnData.combatant, target);
                    }  
                } 
                else
                {
                    target.EvadeAttack();
                    Debug.Log(target.characterName + " dodged the attack!");
                }
            }
            yield return new WaitForSeconds(1f);
        } 
        else
        {
            yield return new WaitForSeconds(0.2f);
        }
        EndAction();
    }

    public void EndAction()
    {
        Debug.Log("action complete!");
        battleManager.EndTurn();
    }
}

