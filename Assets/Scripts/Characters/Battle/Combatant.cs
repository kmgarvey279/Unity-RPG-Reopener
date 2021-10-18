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

public class Combatant : MonoBehaviour
{
    [Header("Character Stats")]
    public CharacterInfo characterInfo;
    public string characterName;
    public int level;
    public DynamicStat hp;
    public DynamicStat mp;
    public Dictionary<BattleStatType, Stat> battleStatDict = new Dictionary<BattleStatType, Stat>();
    public Dictionary<ElementalProperty, Stat> elementalResistDict;
    public List<Action> skills;
    [Header("Status")]
    public bool ko = false;
    public List<StatusEffect> statusEffects;
    [Header("Game Object Components")]
    public Animator animator;
    public GameObject spriteFill;
    [Header("Parent Scripts")]
    public Battlefield battlefield;
    [Header("Child Scripts")]
    public HealthDisplay healthDisplay;
    public StatusEffectDisplay statusEffectDisplay;
    public MaskController maskController;
    [Header("Events")]
    public SignalSenderGO onTargetSelect;
    public SignalSenderGO onTargetDeselect;
    [Header("Position/Direction")]
    public Tile tile;
    public Vector2 lookDirection;
    public GridMovement gridMovement;
    [HideInInspector] public float recoveryTime = 1f;
    [HideInInspector] public float koTime = 1f;

    public virtual void Awake()
    {
        characterName = characterInfo.characterName;
        //stats
        level = characterInfo.level;
        hp = characterInfo.hp;
        mp = characterInfo.mp;
        SetBattleStats(characterInfo.statDict);
        elementalResistDict = characterInfo.elementalResistDict;
        //skills
        skills = characterInfo.skills;
        //components
        healthDisplay = GetComponentInChildren<HealthDisplay>();
        statusEffectDisplay = GetComponentInChildren<StatusEffectDisplay>();
        maskController = GetComponentInChildren<MaskController>();
        gridMovement = GetComponentInChildren<GridMovement>();
    }

    public virtual void SetBattleStats(Dictionary<StatType, Stat> statDict)
    {
    }

    public virtual void Start()
    {
        battlefield = GetComponentInParent<Battlefield>();
        SnapToTileCenter();
    }

    public void SnapToTileCenter()
    {
        Tilemap tilemap = battlefield.gridManager.tilemap;
        
        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
        Vector3 newPosition = tilemap.GetCellCenterWorld(cellPosition);
        transform.position = newPosition;
    }

    public void OnTurnStart()
    {
        if(statusEffects.Count > 0)
        {
            for(int i = statusEffects.Count - 1; i >= 0; i--)
            {
                statusEffects[i].OnTurnStart(this);            
            }
        }
    }

    public Vector2 GetDirection()
    {
        return new Vector2(animator.GetFloat("Look X"), animator.GetFloat("Look Y"));
    }

    public void SetTile(Tile tile)
    {
        this.tile = tile;
    }

    public void FaceTarget(Transform target)
    {
        Vector2 direction = (target.position - transform.position).normalized;
        SetLookDirection(direction);
    }

    public void SetLookDirection(Vector2 newDirection)
    {
        lookDirection = newDirection;
        animator.SetFloat("Look X", newDirection.x);
        animator.SetFloat("Look Y", newDirection.y);
    }

    public void EvadeAttack()
    {
        healthDisplay.HandleHealthChange(DamagePopupType.Miss, 0);
    }

    public void TakeDamage(int damageAmount, Vector2 attackDirection)
    {
        if(statusEffects.Count > 0)
        {
            for(int i = statusEffects.Count - 1; i >= 0; i--)
            {
                statusEffects[i].OnHit(this);            
            }
        }
        if(attackDirection.x != 0 || attackDirection.y !=0)
            SetLookDirection(new Vector2(-attackDirection.x, -attackDirection.y));
        //switch to damage animation
        animator.SetTrigger("Stun");
        //update health
        ChangeHealth(-damageAmount);
        //ko target if hp <= 0
        if(hp.GetCurrentValue() <= 0)
        {
            StartCoroutine(KO());
        }
        else 
        {
            StartCoroutine(EndStun());
        }
    }

    public void ChangeHealth(int amount)
    {
        hp.ChangeCurrentValue(amount);
        if(amount > 0)
        {
            healthDisplay.HandleHealthChange(DamagePopupType.Heal, amount);
        }
        else
        {
            healthDisplay.HandleHealthChange(DamagePopupType.Damage, amount);
        }
    }

    public void AddStatusEffect(StatusEffect statusEffect)
    {
        statusEffects.Add(statusEffect);
        statusEffectDisplay.AddStatusIcon(statusEffect);
        if(statusEffect.animatorTrigger != null)
            animator.SetTrigger(statusEffect.animatorTrigger);
    }

    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        statusEffects.Remove(statusEffect);
        statusEffectDisplay.RemoveStatusIcon(statusEffect);
        if(statusEffect.animatorTrigger != null)
            animator.SetTrigger("Idle");
    }

    private IEnumerator EndStun()
    {
        yield return new WaitForSeconds(recoveryTime);
        animator.SetTrigger("Idle");
    }

    private IEnumerator KO()
    {
        yield return new WaitForSeconds(koTime);
        Debug.Log("Dead");
    }

    public void Select()
    {
        maskController.TriggerSelected();
        onTargetSelect.Raise(this.gameObject);
        // healthDisplay.ToggleBarVisibility(true);
    }

    public void Deselect()
    {
        maskController.EndAnimation();
        onTargetDeselect.Raise(this.gameObject);
        // healthDisplay.ToggleBarVisibility(false);
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
