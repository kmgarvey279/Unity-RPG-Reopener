using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class ExecuteActionState : BattleState
{   
    [Header("Events (Signals)")]
    [SerializeField] private SignalSenderGO onCameraZoomIn;
    private int hits;
    private Tile targetedTile;
    private List<Combatant> targets;
    private List<Combatant> knockedbackTargets = new List<Combatant>();
    private List<Combatant> collisions = new List<Combatant>();

    private void Start()
    {
        battleStateType = BattleStateType.Execute;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        hits = 0;
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
        // if(turnData.targetedTile != null && turnData.targetedTile != turnData.combatant.tile)
        // {
        //     turnData.combatant.FaceTarget(turnData.targetedTile.transform);
        // }
        if(turnData.action.actionType == ActionType.Move)
        {
            MovePhase();
        }
        else
        {
            StartCoroutine(CastAnimationPhase());
        }
    }

    //trigger action animation + visual effect on user
    public IEnumerator CastAnimationPhase()
    {
        if(turnData.action.hasCastAnimation)
        {
            Debug.Log("Cast Phase");
            //cast animation
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
            StartActionAnimation();
        }
        else
        {
            yield return null;
            StartActionAnimation(); 
        }
    }

    private void MovePhase()
    {
        Debug.Log("Move Phase");
        List<Tile> path = gridManager.GetPath(turnData.combatant.tile, turnData.targetedTile, TargetType.TargetPlayer);
        if(turnData.targetedTile.occupier)
        {
                List<Tile> reversePath = new List<Tile>();
                reversePath.AddRange(path);
                reversePath.Reverse();
                turnData.targetedTile.occupier.Move(reversePath, MovementType.Move);
        }
        turnData.combatant.Move(path, MovementType.Move);
    }

    public void OnMoveEnd(GameObject combatantObject)
    {
        if(combatantObject.GetComponent<Combatant>() == turnData.combatant)
        {
            StartCoroutine(EndActionPhase());
        }
    }

    public void StartActionAnimation()
    {
        targetedTile = turnData.targetedTile;
        targets = turnData.targets;
        collisions.Clear();
        if(turnData.action.hitRandomTarget)
        {
            int roll = Mathf.FloorToInt(Random.Range(0, turnData.targets.Count + 1));
            targetedTile = turnData.targets[roll].tile;
            targets = new List<Combatant>(){turnData.targets[roll]};
        }
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
                Vector3 startPosition = new Vector3(turnData.combatant.tile.transform.position.x + (turnData.combatant.GetDirection().x * 0.1f), turnData.combatant.transform.position.y + (turnData.combatant.GetDirection().y * 0.1f));
                GameObject projectileObject = Instantiate(turnData.action.projectileGraphicPrefab, startPosition, Quaternion.identity);
                projectileObject.GetComponent<Projectile>().Move(targetedTile.transform.position);
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
            List<GameObject> effects = new List<GameObject>();
            Tile[,] tileArray = gridManager.GetTileArray(turnData.targetType);
            foreach(AOE aoe in turnData.action.aoes)
            {
                //default spawn position (center of battlefield)
                Vector2 spawnPosition = tileArray[1, 1].transform.position;
                if(turnData.action.isFixedAOE)
                {
                    spawnPosition = tileArray[aoe.fixedStartPosition.x, aoe.fixedStartPosition.y].transform.position;
                }
                else
                {
                    if(targetedTile && aoe.aoeType != AOEType.All)
                    {
                        if(aoe.aoeType == AOEType.Row)
                        {
                            spawnPosition = tileArray[1, targetedTile.y].transform.position;
                        }  
                        else if(aoe.aoeType == AOEType.Column)
                        {
                            spawnPosition = tileArray[targetedTile.x, 1].transform.position;
                        }   
                        else
                        { 
                            spawnPosition = targetedTile.transform.position;
                        }
                    }
                }
                GameObject graphicObject = Instantiate(turnData.action.effectGraphicPrefab, spawnPosition, turnData.action.effectGraphicPrefab.transform.rotation);
                effects.Add(graphicObject);
            }
            //destory visual effect
            yield return new WaitForSeconds(turnData.action.effectAnimationDuration);
            foreach(GameObject effect in effects)
            {
                Destroy(effect);
            }
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
        if(targets.Count > 0)
        {
            for(int i = targets.Count - 1; i >= 0; i--)
            {
                Debug.Log("checking target");
                if(turnData.action.actionType == ActionType.Attack)
                {
                    Debug.Log("is attack");
                    Combatant target = targets[i];
                    int hitChance = Mathf.Clamp(turnData.action.accuracy + turnData.combatant.battleStatDict[BattleStatType.Accuracy].GetValue() - target.battleStatDict[BattleStatType.Evasion].GetValue(), 1, 99);
                    Debug.Log(hitChance);
                    bool didHit = Roll(hitChance);
                    if(didHit || turnData.action.guaranteedHit)
                    {
                        target.TakeHit(turnData.combatant);
                        Debug.Log(target.characterName + " was hit!");
                    }
                    else
                    {
                        target.EvadeAttack(turnData.combatant);
                        targets.Remove(target);
                        Debug.Log(target.characterName + " dodged the attack!");
                    }
                }
            }
        }
        StartCoroutine(KnockbackPhase());
    }

    private IEnumerator KnockbackPhase()
    {
        Debug.Log("Knockback Phase");
        if(turnData.action.knockback.doKnockback)
        { 
            //order targets based on knockback direction
            if(turnData.action.knockback.moveX == -1)
            {
                targets = targets.OrderByDescending(target=>target.tile.x).ToList();
            }
            else if(turnData.action.knockback.moveX == 1)
            {
                targets = targets.OrderByDescending(target=>target.tile.x).ToList();
            }
            else if(turnData.action.knockback.moveY == -1)
            {
                targets = targets.OrderByDescending(target=>target.tile.y).ToList();
            }
            else if(turnData.action.knockback.moveY == 1)
            {
                targets = targets.OrderByDescending(target=>target.tile.y).ToList();
            }

            foreach(Combatant target in targets)
            {
                Tile destinationTile = target.tile;
                Vector2Int newCoordinates = turnData.action.knockback.GetKnockbackDestination(target.tile);
                destinationTile = gridManager.GetTileArray(turnData.targetType)[newCoordinates.x, newCoordinates.y];
                if(destinationTile != target.tile)
                {
                    if(destinationTile.occupier == null)
                    {
                        target.Move(new List<Tile>(){target.tile, destinationTile}, MovementType.Knockback); 
                    }
                }
            }
            yield return new WaitForSeconds(0.4f);
            StartCoroutine(HealthEffectPhase());
        }
        else 
        {
            yield return null;
            StartCoroutine(HealthEffectPhase());
        }
    }

    //trigger effects of action (ex: damage, heal, status effect)
    public IEnumerator HealthEffectPhase()
    {
        Debug.Log("Damage/heal phase");
        if(targets.Count > 0)
        {
            foreach(Combatant target in targets)
            {
                bool crit = false;
                if(turnData.action.actionType == ActionType.Heal)
                {
                    float offensiveStat = (float)turnData.combatant.battleStatDict[turnData.action.offensiveStat].GetValue();
                    float healAmount = Mathf.Clamp((float)turnData.action.power * offensiveStat / 10f, 1, 9999);
                    target.Heal(Mathf.FloorToInt(healAmount));
                }
                else if(turnData.action.actionType == ActionType.Attack)
                {
                    float offensiveStat = (float)turnData.combatant.battleStatDict[turnData.action.offensiveStat].GetValue();
                    float damageAmount = Mathf.Clamp((float)(turnData.action.power * offensiveStat / 10f) * Random.Range(0.85f, 1f), 1, 9999); 
                    if(Roll(turnData.combatant.battleStatDict[BattleStatType.CritRate].GetValue()))
                    {
                        damageAmount = damageAmount * 1.25f;
                        crit = true;
                    }
                    // damageAmount = damageAmount - target.battleStatDict[turnData.action.defensiveStat].GetValue();
                    target.Damage(Mathf.FloorToInt(damageAmount), crit);
                }
            }
            yield return new WaitForSeconds(0.4f);
            KOCheckPhase();
        } 
        else
        {
            yield return null;
            KOCheckPhase();
        }
    }

    private void KOCheckPhase()
    {
        if(targets.Count > 0)
        {
            for(int i = targets.Count - 1; i >= 0; i--)
            {
                if(targets[i].hp.GetCurrentValue() <= 0)
                {
                    battleManager.KOCombatant(targets[i]);
                    targets.RemoveAt(i);
                }
            }
        }
        StartCoroutine(StatusEffectPhase());
    }

    private IEnumerator StatusEffectPhase()
    {
        if(turnData.action.statusEffectSO != null && targets.Count > 0)
        {
            foreach(Combatant target in targets)
            {
                int offensiveStat = turnData.combatant.battleStatDict[turnData.action.offensiveStat].GetValue();
                if(turnData.action.statusEffectSO.isBuff)
                {
                    target.AddStatusEffect(turnData.action.statusEffectSO,  offensiveStat);
                }
                else if(Roll(turnData.action.statusEffectChance))
                {
                    target.AddStatusEffect(turnData.action.statusEffectSO, offensiveStat);
                }
            }
            yield return new WaitForSeconds(0.4f);
        }
        if(targets.Count < 1)
        {
            StartCoroutine(EndActionPhase());
        }
        else
        {
            hits++;
            if(hits < turnData.action.hitCount)
            {
                StartActionAnimation();
            }
            else
            {
                StartCoroutine(EndActionPhase());
            }
        }
    }

    public IEnumerator EndActionPhase()
    {
        Debug.Log("action complete!");
        turnData.combatant.animator.SetTrigger("Idle");    
        yield return new WaitForSeconds(0.6f);
        if(turnData.targets.Count > 0)
        {
            foreach(Combatant target in turnData.targets)
            {
                target.animator.SetTrigger("Idle");
                target.ToggleHPBar(false);
            }
        }    
        battleManager.UpdateActionPoints(-turnData.action.apCost);
        if(turnData.actionPoints > 0)
        {
            if(turnData.combatant is PlayableCombatant)
            {
                stateMachine.ChangeState((int)BattleStateType.Menu);
            }
            else
            {
                stateMachine.ChangeState((int)BattleStateType.EnemyTurn);
            }
        }
        else
        {
            stateMachine.ChangeState((int)BattleStateType.TurnEnd);
        }
    }

    public bool Roll(int chance)
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