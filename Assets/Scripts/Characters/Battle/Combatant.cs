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
    public Dictionary<BattleStatType, Stat> battleStatDict;
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
    public MaskController maskController;
    [Header("Events")]
    public SignalSender onMoveComplete;
    public SignalSenderGO onTargetSelect;
    public SignalSenderGO onTargetDeselect;
    [Header("Grid")]
    public Tile tile;
    [HideInInspector] public List<Tile> path = new List<Tile>();
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
        maskController = GetComponentInChildren<MaskController>();
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

    public Vector2 GetDirection()
    {
        return new Vector2(animator.GetFloat("Look X"), animator.GetFloat("Look Y"));
    }

    public void SetTile(Tile tile)
    {
        this.tile = tile;
    }

    public void Move(Tile destination)
    {
        path = battlefield.gridManager.GetPath(tile, destination);
        animator.SetBool("Moving", true);
    }

    public virtual void EndMove()
    {
        onMoveComplete.Raise();
    }

    public void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        animator.SetFloat("Look X", direction.x);
        animator.SetFloat("Look Y", direction.y);
    }

    private void Update()
    {
        float speed = 4f;
        if(path.Count > 0)
        {
            if(Vector3.Distance(transform.position, path[0].transform.position) < 0.0001f)
            {
                path.RemoveAt(0);
                if(path.Count == 0)
                {
                    animator.SetBool("Moving", false);
                    EndMove();
                }
            }
            else
            {
                Vector3 moveDirection = (path[0].transform.position - transform.position).normalized;
                if(!Mathf.Approximately(moveDirection.x, 0.0f) || !Mathf.Approximately(moveDirection.y, 0.0f))
                {
                    animator.SetFloat("Look X", Mathf.Round(moveDirection.x));
                    animator.SetFloat("Look Y", Mathf.Round(moveDirection.y));
                }
                float step = speed * Time.deltaTime; 
                transform.position = Vector3.MoveTowards(transform.position, path[0].transform.position, step);   
            }
        }
    }

    public void EvadeAttack()
    {
        healthDisplay.HandleHealthChange(DamagePopupType.Miss, 0);
    }

    public void TakeDamage(int damageAmount)
    {
        //switch to damage animation
        animator.SetBool("Stun", true);
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
    }

    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        statusEffects.Remove(statusEffect);
    }

    private IEnumerator EndStun()
    {
        yield return new WaitForSeconds(recoveryTime);
        animator.SetBool("Stun", false);
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
