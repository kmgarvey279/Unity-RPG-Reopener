using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;
using UnityEngine.Tilemaps;

public enum BattleStatType
{
    MeleeAttack,
    RangedAttack,
    MagicAttack,
    PhysicalDefense,
    MagicDefense,
    Accuracy,
    Evasion,
    CritRate,
    Speed,
    MoveRange,
    None
}

public class StatusEffectInstance
{
    public StatusEffectSO statusEffectSO;
    public int turnCounter;
    public int userPower;

    public StatusEffectInstance(StatusEffectSO statusEffectSO, int userPower)
    {
        this.statusEffectSO = statusEffectSO;
        this.turnCounter = statusEffectSO.turnDuration;
        this.userPower = userPower;
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
    public bool ko = false;
    public List<StatusEffectInstance> statusEffects;
    [Header("Game Object Components")]
    [HideInInspector] public Animator animator;
    [SerializeField] protected GameObject spriteFill;
    [Header("Parent Scripts")]
    public GridManager gridManager;
    [Header("Child Scripts")]
    protected HealthDisplay healthDisplay;
    protected StatusEffectDisplay statusEffectDisplay;
    protected MaskController maskController;
    [Header("Events")]
    [SerializeField] protected SignalSenderGO onTargetSelect;
    [SerializeField] protected SignalSenderGO onTargetDeselect;
    [Header("Grid and Targeting")]
    public Tile tile;
    [HideInInspector] public GridMovement gridMovement;
    private bool selected = false;

    public virtual void Awake()
    {
        characterName = characterInfo.characterName;
        //stats
        level = characterInfo.level;
        hp = characterInfo.hp;
        mp = characterInfo.mp;
        resistDict = characterInfo.resistDict;
        SetBattleStats();
        SetTraitEffects();
        //child components
        healthDisplay = GetComponentInChildren<HealthDisplay>();
        statusEffectDisplay = GetComponentInChildren<StatusEffectDisplay>();
        maskController = GetComponentInChildren<MaskController>();
        gridMovement = GetComponentInChildren<GridMovement>();
    }

    public virtual void SetBattleStats()
    {
    }

    public virtual void SetTraitEffects()
    {
    }

    public virtual void Start()
    {
        gridManager = GetComponentInParent<GridManager>();
        // SnapToTileCenter();
    }

    // public void SnapToTileCenter()
    // {
    //     // Tilemap tilemap = gridManager.tilemap;
        
    //     // Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
    //     // Vector3 newPosition = tilemap.GetCellCenterWorld(cellPosition);
    //     // transform.position = newPosition;
    // }

    public virtual void OnTurnStart()
    {
        if(statusEffects.Count > 0)
        {
            for(int i = statusEffects.Count - 1; i >= 0; i--)
            {
                if(statusEffects[i].statusEffectSO.hasDuration)
                {
                    statusEffects[i].turnCounter--;
                    if(statusEffects[i].turnCounter <= 0)
                    {
                        RemoveStatusEffect(statusEffects[i]);
                    }
                }
            }
        }
    }

    public virtual void OnTurnEnd()
    {
        if(statusEffects.Count > 0)
        {
            int cumHealthEffect = 0;
            foreach(StatusEffectInstance statusEffect in statusEffects)
            {
                float potency = (float)statusEffect.statusEffectSO.potency;
                float userPower = (float)statusEffect.userPower / (100f + (float)statusEffect.userPower);
                if(statusEffect.statusEffectSO.healOverTime)
                {
                    float healAmount = Mathf.Clamp(potency * userPower * Random.Range(0.85f, 1f), 1f, 9999f);
                    cumHealthEffect += Mathf.FloorToInt(healAmount);
                }    
                if(statusEffect.statusEffectSO.damageOverTime)
                {
                    float damageAmount = Mathf.Clamp(potency * userPower * Random.Range(0.85f, 1f), 1f, 9999f);
                    cumHealthEffect -= Mathf.FloorToInt(damageAmount);
                }   
            }
            if(cumHealthEffect > 0)
            {
                Heal(cumHealthEffect);
            }
            else if(cumHealthEffect < 0)
            {
                Damage(Mathf.Abs(cumHealthEffect));
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
        Vector2 attackDirection = attacker.GetDirection();
        if(attackDirection.x != 0 || attackDirection.y !=0)
            SetDirection(new Vector2(-attackDirection.x, -attackDirection.y));
        animator.SetTrigger("Stun");
        healthDisplay.HandleHealthChange(DamagePopupType.Miss, 0);
    }

    public void TakeHit(Combatant attacker)
    {
        Vector2 attackDirection = attacker.GetDirection();
        if(attackDirection.x != 0 || attackDirection.y !=0)
            SetDirection(new Vector2(-attackDirection.x, -attackDirection.y));
        //switch to damage animation
        animator.SetTrigger("Stun");
    }

    public virtual void Damage(int damage, Combatant attacker = null, bool isCrit = false)
    {
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

    public void AddStatusEffect(StatusEffectSO statusEffectSO, int userPower)
    {
        //clear duplicates
        foreach(StatusEffectInstance thisStatusInstance in statusEffects)
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
        StatusEffectInstance newStatusInstance = new StatusEffectInstance(statusEffectSO, userPower);
        //add status effect to list
        statusEffects.Add(newStatusInstance);
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
        statusEffectDisplay.AddStatusIcon(statusEffectSO);
        //change character sprite (if applicable);
        if(statusEffectSO.animatorTrigger != null)
            animator.SetTrigger(statusEffectSO.animatorTrigger);
    }

    public void RemoveStatusEffect(StatusEffectInstance statusEffectInstance)
    {
        if(statusEffects.Contains(statusEffectInstance))
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
            statusEffectDisplay.RemoveStatusIcon(statusEffectInstance.statusEffectSO);
            if(statusEffectInstance.statusEffectSO.animatorTrigger != null)
                animator.SetTrigger("Idle");
        }
    }


    public bool KOCheck()
    {
        if(hp.GetValue() <= 0)
        {
            KO();
            return true;
        }
        return false;
    }

    public void KO()
    {
        animator.SetTrigger("KO");
        // battleManager.OnCombatantKO(this);
    }

    public void Select()
    {
        if(!selected)
        {
            selected = true;
            maskController.TriggerSelected();
            onTargetSelect.Raise(this.gameObject);
            // healthDisplay.ToggleBarVisibility(true);
        }
    }

    public void Deselect()
    {
        if(selected)
        {
            selected = false;
            maskController.EndAnimation();
            onTargetDeselect.Raise(this.gameObject);
            // healthDisplay.ToggleBarVisibility(false);
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
