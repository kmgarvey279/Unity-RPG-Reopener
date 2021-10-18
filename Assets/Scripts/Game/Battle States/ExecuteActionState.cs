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
        Debug.Log("Enter execute state");
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
        base.OnExit();
    }

    public void ExecuteAction()
    {
        Debug.Log(turnData.combatant.characterName + " used " + turnData.action.actionName);
        if(turnData.targetedTile != null && turnData.targetedTile != turnData.combatant.tile)
        {
            turnData.combatant.FaceTarget(turnData.targetedTile.transform);
        }
        StartCoroutine(CastAnimationCo());
    }

    //trigger action animation + visual effect on user
    public IEnumerator CastAnimationCo()
    {
        //cast animation
        if(turnData.action.hasCastAnimation)
        {
            if(turnData.action.castAnimatorTrigger != null)
            {
                turnData.combatant.animator.SetTrigger(turnData.action.castAnimatorTrigger);
            }
            //spawn casting effect
            if(turnData.action.castGraphicPrefab != null)
            {
                yield return new WaitForSeconds(turnData.action.castGraphicDelay); 
                Vector3 spawnPosition = turnData.combatant.transform.position;
                GameObject graphicObject = Instantiate(turnData.action.castGraphicPrefab, spawnPosition, Quaternion.identity);
            }
            yield return new WaitForSeconds(turnData.action.castAnimationDuration);
            MoveAnimation();
        }
        else
        {
            MoveAnimation();
        }
    }

    private void MoveAnimation()
    {
        if(turnData.action.hasMoveAnimation && turnData.targetedTile != turnData.combatant.tile)
        {
            turnData.combatant.gridMovement.Move(new List<Tile>(){turnData.combatant.tile, turnData.targetedTile}, MovementType.Dash); 
        }
        else
        {
            StartCoroutine(ProjectileAnimationCo());
        }
    }

    public void OnMoveEnd()
    {
        StartCoroutine(ProjectileAnimationCo());
    }

    public IEnumerator ProjectileAnimationCo()
    {
            if(turnData.action.hasProjectileAnimation && turnData.action.projectileGraphicPrefab != null)
            {
                if(turnData.action.projectileAnimatorTrigger != null)
                {
                    turnData.combatant.animator.SetTrigger(turnData.action.projectileAnimatorTrigger);
                }
                yield return new WaitForSeconds(turnData.action.projectileGraphicDelay);
                Vector3 startPosition = new Vector3(turnData.combatant.tile.transform.position.x + (turnData.combatant.lookDirection.x * 0.1f), turnData.combatant.transform.position.y + (turnData.combatant.lookDirection.y * 0.1f));
                GameObject projectileObject = Instantiate(turnData.action.projectileGraphicPrefab, startPosition, Quaternion.identity);
                projectileObject.GetComponent<Projectile>().Move(turnData.targetedTile.transform.position);
            }
            else
            {
                yield return null; 
                StartCoroutine(EffectAnimationCo());
            }
    }

    public void OnProjectileEnd()
    {
        StartCoroutine(EffectAnimationCo());
    }

    private IEnumerator EffectAnimationCo()
    {
        //user animation
        if(turnData.action.effectAnimatorTrigger != null)
        {
            turnData.combatant.animator.SetTrigger(turnData.action.effectAnimatorTrigger);
        }
        //spawn visual effect
        if(turnData.action.effectGraphicPrefab)
        {
            yield return new WaitForSeconds(turnData.action.effectGraphicDelay); 
            Vector3 spawnPosition = turnData.targetedTile.transform.position;
            GameObject graphicObject = Instantiate(turnData.action.effectGraphicPrefab, spawnPosition, Quaternion.identity);
            //destory visual effect
            yield return new WaitForSeconds(turnData.action.effectAnimationDuration);
            Destroy(graphicObject);
        }
        else 
        {
            yield return new WaitForSeconds(turnData.action.effectAnimationDuration);
        }
        KnockbackAnimation();
    }

    private void KnockbackAnimation()
    {
        if(turnData.action.knockback > 0)
        {
            foreach (Combatant target in turnData.targets)
            {
                Tile destination = target.tile;
                //find furthest tile target can be knocked back to
                List<Tile> row = gridManager.GetRow(target.tile, turnData.combatant.lookDirection, turnData.action.knockback, true);

                destination = row[row.Count - 1];
                if(destination != target.tile)
                {
                    target.gridMovement.Move(new List<Tile>(){target.tile, destination}, MovementType.Knockback); 
                }
            }
        }
    }

    public void OnKnockbackEnd()
    {
        StartCoroutine(TriggerActionEffectsCo());
    }

    //trigger effects of action (ex: damage, heal, status effect)
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
                    
                    //damage/effects
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
            yield return new WaitForSeconds(0.3f);
        }
        EndAction();
    }

    public void EndAction()
    {
        Debug.Log("action complete!");
        turnData.combatant.animator.SetTrigger("Idle");
        battleManager.EndTurn();
    }
}

