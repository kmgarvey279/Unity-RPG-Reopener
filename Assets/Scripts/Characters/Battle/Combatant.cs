using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;
using UnityEngine.Tilemaps;

public enum BattleStatType
{
    Attack,
    Defense,
    MagicAttack,
    MagicDefense,
    CritRate,
    Accuracy,
    Speed,
    Evasion,
    None
}

[System.Serializable]
public class StatusEffectInstance
{
    public StatusEffectSO statusEffectSO;
    public int turnCounter;
    public int potency;

    public StatusEffectInstance(StatusEffectSO statusEffectSO, int potency)
    {
        this.statusEffectSO = statusEffectSO;
        this.turnCounter = statusEffectSO.turnDuration;
        this.potency = potency;
    }
}

public class Combatant : MonoBehaviour
{
    [Header("Character Stats")]
    public CharacterInfo characterInfo;
    public string characterName;
    public int level;
    public DynamicStat hp;
    public DynamicStat mp;
    public Dictionary<BattleStatType, Stat> battleStatDict = new Dictionary<BattleStatType, Stat>();
    public Dictionary<ElementalProperty, Stat> resistDict;
    // [Header("Triggerable Effects")]
    // public List<SubEffect> triggerableSubEffects = new List<TriggerableSubEffect>();
    [Header("Status")]
    public List<StatusEffectInstance> statusEffectInstances;
    [Header("Game Object Components")]
    [HideInInspector] public Animator animator;
    [SerializeField] protected GameObject spriteFill;
    [Header("Parent Scripts")]
    public GridManager gridManager;
    [Header("Child Scripts")]
    protected HealthDisplay healthDisplay;
    protected MaskController maskController;
    [Header("Events")]
    [SerializeField] protected SignalSenderGO onTargetSelect;
    [SerializeField] protected SignalSenderGO onTargetDeselect;
    [SerializeField] protected SignalSenderGO OnCombatantKO;
    [Header("Movement and Targeting")]
    public Tile tile;
    [HideInInspector] public GridMovement gridMovement;
    public Vector2 defaultDirection;
    private bool selected = false;

    public virtual void Awake()
    {
        characterName = characterInfo.characterName;
        //stats
        level = characterInfo.level;
        hp = new DynamicStat(characterInfo.hp.GetValue(), characterInfo.hp.GetCurrentValue());
        mp = new DynamicStat(characterInfo.mp.GetValue(), characterInfo.hp.GetCurrentValue());
        resistDict = characterInfo.resistDict;
        SetBattleStats();
        SetTraitEffects();
        //child components
        healthDisplay = GetComponentInChildren<HealthDisplay>();
        maskController = GetComponentInChildren<MaskController>();
        gridMovement = GetComponentInChildren<GridMovement>();
    }

    public virtual void SetBattleStats()
    {
    }

    public virtual void SetTraitEffects()
    {
    }

    public virtual void OnTurnStart()
    {
        if(statusEffectInstances.Count > 0)
        {
            for(int i = statusEffectInstances.Count - 1; i >= 0; i--)
            {
                if(statusEffectInstances[i].statusEffectSO.hasDuration)
                {
                    statusEffectInstances[i].turnCounter--;
                    if(statusEffectInstances[i].turnCounter <= 0)
                    {
                        RemoveStatusEffect(statusEffectInstances[i]);
                    }
                }
            }
        }
    }

    public virtual void OnTurnEnd()
    {
        if(statusEffectInstances.Count > 0)
        {
            foreach(StatusEffectInstance statusEffectInstance in statusEffectInstances)
            {
                int healthChange = statusEffectInstance.statusEffectSO.GetHealthChange(this, statusEffectInstance.potency, statusEffectInstance.turnCounter);
                if(healthChange > 0)
                {
                    Heal(healthChange);
                }
                else if(healthChange < 0)
                {
                    Damage(Mathf.Abs(healthChange));
                }
            }
        }
    }

    public void Move(List<Tile> path, MovementType movementType)
    {
        SetTile(tile, path[path.Count - 1]);
        gridMovement.Move(path, movementType);
    }

    public void SetTile(Tile originalTile, Tile newTile)
    {
        originalTile.UnassignOccupier();
        newTile.AssignOccupier(this);
        this.tile = newTile;
    }

    public void FaceTarget(Transform target)
    {
        Vector2 direction = (target.position - transform.position).normalized;
        SetDirection(direction);
    }

    public Vector2 GetDirection()
    {
        return new Vector2(animator.GetFloat("Look X"), animator.GetFloat("Look Y"));
    }

    public void SetDirection(Vector2 newDirection)
    {
        animator.SetFloat("Look X", newDirection.x);
        animator.SetFloat("Look Y", newDirection.y);
    }

    public virtual void EvadeAttack(Combatant attacker)
    {
        animator.SetTrigger("Move");
        healthDisplay.HandleHealthChange(DamagePopupType.Miss, 0);
        StartCoroutine(ReturnToIdleCo());
    }

    private IEnumerator ReturnToIdleCo()
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetTrigger("Idle");
    }

    public void TakeHit(Combatant attacker)
    {
        Vector2 attackDirection = attacker.GetDirection();
        if(attackDirection.x != 0 || attackDirection.y !=0)
            SetDirection(new Vector2(-attackDirection.x, -attackDirection.y));
        //switch to damage animation
        animator.SetTrigger("Stun");
    }

    public virtual void Damage(int damage, bool isCrit = false)
    {
        Debug.Log(characterName + " took " + damage + " damage");
        hp.ChangeCurrentValue(-damage);
        if(isCrit)
        {
            healthDisplay.HandleHealthChange(DamagePopupType.Crit, damage);
        }
        else
        {
            healthDisplay.HandleHealthChange(DamagePopupType.Damage, damage);
        }
    }

    public virtual void Heal(int amount)
    {
        hp.ChangeCurrentValue(amount);
        healthDisplay.HandleHealthChange(DamagePopupType.Heal, amount);
    }

    public void AddStatusEffect(StatusEffectSO statusEffectSO, int potency)
    {
        //clear duplicates
        foreach(StatusEffectInstance thisStatusInstance in statusEffectInstances)
        {
            if(thisStatusInstance.statusEffectSO == statusEffectSO)
            {
                if(thisStatusInstance.statusEffectSO.cannotRefresh)
                {
                    return;
                }
                RemoveStatusEffect(thisStatusInstance);
            }
        }
        StatusEffectInstance newStatusInstance = new StatusEffectInstance(statusEffectSO, potency);
        //add status effect to list
        statusEffectInstances.Add(newStatusInstance);
        //apply stat modifiers        
        foreach(BattleStatModifier battleStatModifier in newStatusInstance.statusEffectSO.battleStatModifiers)
        {
            battleStatDict[battleStatModifier.statToModify].AddMultiplier(battleStatModifier.multiplier);  
        }
        foreach(ResistanceModifier resistanceModifier in newStatusInstance.statusEffectSO.resistanceModifiers)
        {
            resistDict[resistanceModifier.resistanceToModify].AddMultiplier(resistanceModifier.multiplier);  
        }
        //display icon
        healthDisplay.AddStatusIcon(statusEffectSO);
        //change character sprite (if applicable);
        if(statusEffectSO.animatorTrigger != null)
            animator.SetTrigger(statusEffectSO.animatorTrigger);
    }

    public void RemoveStatusEffect(StatusEffectInstance statusEffectInstance)
    {
        if(statusEffectInstances.Contains(statusEffectInstance))
        {
            RemoveStatusEffect(statusEffectInstance);
            foreach(BattleStatModifier battleStatModifier in statusEffectInstance.statusEffectSO.battleStatModifiers)
            {
                battleStatDict[battleStatModifier.statToModify].RemoveMultiplier(battleStatModifier.multiplier);  
            }
            foreach(ResistanceModifier resistanceModifier in statusEffectInstance.statusEffectSO.resistanceModifiers)
            {
                resistDict[resistanceModifier.resistanceToModify].RemoveMultiplier(resistanceModifier.multiplier);  
            }
            healthDisplay.RemoveStatusIcon(statusEffectInstance.statusEffectSO);
            if(statusEffectInstance.statusEffectSO.animatorTrigger != null)
                animator.SetTrigger("Idle");
        }
    }

    public virtual IEnumerator KOCo()
    {
        yield return null;
    }

    public void ToggleHPBar(bool show)
    {
        healthDisplay.Display(show);
    }

    public void Select()
    {
        if(!selected)
        {
            selected = true;
            maskController.TriggerSelected();
            onTargetSelect.Raise(this.gameObject);
        }
    }

    public void Deselect()
    {
        if(selected)
        {
            selected = false;
            maskController.EndAnimation();
            onTargetDeselect.Raise(this.gameObject);
        }
    }

    public void GrayOut()
    {
        maskController.TriggerUnselectable();
    }

    public void ClearSpriteMask()
    {
        maskController.EndAnimation();
    }
}
