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
        StartCoroutine(CastAnimationPhase());
    }

    //trigger action animation + visual effect on user
    public IEnumerator CastAnimationPhase()
    {
        Debug.Log("Cast Phase");
        //cast animation
        if(turnData.action.hasCastAnimation)
        {
            if(turnData.action.castAnimatorTrigger != "")
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
            MoveAnimationPhase();
        }
        else
        {
            MoveAnimationPhase();
        }
    }

    private void MoveAnimationPhase()
    {
        Debug.Log("Move Phase");
        if(turnData.action.hasMoveAnimation && turnData.targetedTile != turnData.combatant.tile)
        {
            turnData.combatant.gridMovement.Move(new List<Tile>(){turnData.combatant.tile, turnData.targetedTile}, MovementType.Dash); 
        }
        else
        {
            StartCoroutine(ProjectileAnimationPhase());
        }
    }

    public void OnMoveEnd()
    {
        StartCoroutine(ProjectileAnimationPhase());
    }

    public IEnumerator ProjectileAnimationPhase()
    {
        Debug.Log("Projectile Phase");
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
                StartCoroutine(EffectAnimationPhase());
            }
    }

    public void OnProjectileEnd()
    {
        StartCoroutine(EffectAnimationPhase());
    }

    private IEnumerator EffectAnimationPhase()
    {
        Debug.Log("Effect Phase");
        //user animation
        if(turnData.action.effectAnimatorTrigger != "")
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
        HitCheckPhase();
    }

    private void HitCheckPhase()
    {
        Debug.Log("Hit check phase");
        for(int i = turnData.targets.Count - 1; i >= 0; i--)
        {
            Debug.Log("checking target");
            if(turnData.action.actionType == ActionType.Attack)
            {
                Debug.Log("is attack");
                Combatant target = turnData.targets[i];
                Vector2 attackDirection = new Vector2(0,0);
                if(turnData.action.useDirection)
                    attackDirection = turnData.combatant.lookDirection;
                bool didHit = battleCalculations.HitCheck(battleCalculations.GetHitChance(turnData.action, turnData.combatant, target, gridManager.GetMoveCost(turnData.combatant.tile, target.tile)));
                if(didHit)
                {
                    target.TakeHit(attackDirection);
                    Debug.Log(target.characterName + " was hit!");
                }
                else
                {
                    target.EvadeAttack(attackDirection);
                    turnData.targets.Remove(target);
                    Debug.Log(target.characterName + " dodged the attack!");
                }
            }
        }
        if(turnData.targets.Count > 0)
        {
            KnockbackAnimationPhase();
        }
        else
        {
            EndAction();
        }
    }

    private void KnockbackAnimationPhase()
    {
        Debug.Log("Knockback Phase");
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
        else 
        {
            StartCoroutine(HealthEffectPhase());
        }
    }

    public void OnKnockbackEnd()
    {
        StartCoroutine(HealthEffectPhase());
    }

    //trigger effects of action (ex: damage, heal, status effect)
    public IEnumerator HealthEffectPhase()
    {
        Debug.Log("Damage/heal phase");
        if(turnData.targets.Count > 0)
        {
            foreach(Combatant target in turnData.targets)
            {
                if(turnData.action.actionType == ActionType.Attack)
                {
                    int damage = battleCalculations.GetDamageAmount(turnData.action, turnData.combatant, target);
                    target.ChangeHealth(-damage);
                }
                else if(turnData.action.actionType == ActionType.Heal)
                {
                    int heal = battleCalculations.GetHealAmount(turnData.action, turnData.combatant);
                    target.ChangeHealth(heal);
                }
            }
            yield return new WaitForSeconds(1f);
            for(int i = turnData.targets.Count - 1; i > 0; i--)
            {
                Combatant target = turnData.targets[i];
                if(target.hp.GetValue() <= 0)
                {
                    Debug.Log("Dead");
                    target.KO();
                    turnData.targets.Remove(target);
                }
            }
            if(turnData.targets.Count > 0)
            {
                StartCoroutine(StatusEffectPhase());
            }
            else
            {
                EndAction();
            }
        } 
    }

    private IEnumerator StatusEffectPhase()
    {
        if(turnData.action.statusEffect != null)
        {
            foreach(Combatant target in turnData.targets)
            {
                bool hitCheck = battleCalculations.HitCheck(turnData.action.statusEffectChance);
                if(hitCheck)
                {
                    target.AddStatusEffect(turnData.action.statusEffect);
                }
            }
        }
        yield return new WaitForSeconds(1f);
        EndAction();
    }

    public void EndAction()
    {
        Debug.Log("action complete!");
        turnData.combatant.animator.SetTrigger("Idle");
        foreach(Combatant target in turnData.targets)
        {
            bool hitCheck = battleCalculations.HitCheck(turnData.action.statusEffectChance);
            if(hitCheck)
            {
                target.animator.SetTrigger("Idle");
            }
        }
        stateMachine.ChangeState((int)BattleStateType.End); 
    }
}

