using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using Pathfinding;
using UnityEngine.Tilemaps;

public class Armor
{
    public DynamicStat durability;

    public Armor(float maxDurability)
    {
        durability = new DynamicStat(maxDurability, 0);
    }
}

public class CounterAction
{
    public Action action;
    public float chance;
}

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

public class Combatant : MonoBehaviour
{
    [Header("Character Data")]
    public CombatantType combatantType;
    public CharacterInfo characterInfo;
    public bool isLoaded = false;

    [Header("Character Stats")]
    public string characterName;
    public int level;
    public DynamicStat hp;
    public DynamicStat mp;
    public Dictionary<BattleStatType, Stat> battleStatDict = new Dictionary<BattleStatType, Stat>();
    public Dictionary<EventTriggerType, List<ActionEventModifier>> actionEventModifiers = new Dictionary<EventTriggerType, List<ActionEventModifier>>()
    {
        {EventTriggerType.OnAct, new List<ActionEventModifier>()},
        {EventTriggerType.OnTargeted, new List<ActionEventModifier>()},
        {EventTriggerType.OnPartyTargeted, new List<ActionEventModifier>()}
    };
    public Dictionary<ElementalProperty, ElementalResistance> resistDict;
    // public List<SubEffect> triggerableSubEffects = new List<TriggerableSubEffect>();
    
    [Header("Status")]
    public List<StatusEffectInstance> statusEffectInstances;
    public Armor armor;
    
    [Header("Animations")]
    [HideInInspector] public Animator animator;
    public string overrideAnimation = "";
    public bool lockAnimation = false;
    [SerializeField] protected GameObject spriteFill;
    
    [Header("Parent Scripts")]
    public GridManager gridManager;
    
    [Header("Child Scripts")]
    public HealthDisplay healthDisplay;
    protected MaskController maskController;
    public TargetSelect targetSelect;
    
    [Header("Movement and Targeting")]
    public Tile tile;
    public bool moving = false;
    public float moveSpeedMultiplier = 1f;
    public Transform moveDestination;
    public Vector2 defaultDirection;
    protected bool selected = false;
    
    [Header("Turns")]
    public TurnCounter turnCounter;
    
    [Header("Actions")]
    public Action attack;
    public CounterAction counterAction;

    [Header("Events")]
    public SignalSenderGO onTurnCounterChange;

    public virtual void Awake()
    {
        //child components
        gridManager = GetComponentInParent<GridManager>();
        healthDisplay = GetComponentInChildren<HealthDisplay>();
        maskController = GetComponentInChildren<MaskController>();
        // gridMovement = GetComponentInChildren<GridMovement>();
        // armor = new Armor();
    }

    public virtual void SetCharacterData(CharacterInfo characterInfo)
    {
        this.characterInfo = characterInfo;
        //basic info
        characterName = characterInfo.characterName;
        level = characterInfo.level;
        //hp/mp
        hp = new DynamicStat(characterInfo.hp.GetValue(), characterInfo.hp.GetCurrentValue());
        mp = new DynamicStat(characterInfo.mp.GetValue(), characterInfo.hp.GetCurrentValue());
        //stats
        battleStatDict.Add(BattleStatType.Accuracy, new Stat((characterInfo.statDict[StatType.Skill].GetValue() / 2) + (characterInfo.statDict[StatType.Agility].GetValue() / 3)));
        battleStatDict.Add(BattleStatType.CritRate, new Stat((characterInfo.statDict[StatType.Skill].GetValue() / 4)));
        battleStatDict.Add(BattleStatType.Speed, new Stat(characterInfo.statDict[StatType.Agility].GetValue() / 2));
        battleStatDict.Add(BattleStatType.Evasion, new Stat((characterInfo.statDict[StatType.Agility].GetValue() / 2) + (characterInfo.statDict[StatType.Skill].GetValue() / 3)));
        //resistances
        resistDict = characterInfo.resistDict;
        //actions
        this.attack = characterInfo.attack;
        this.counterAction = characterInfo.counterAction;
        //action event modifiers
        foreach(ActionEventModifier actionEventModifier in characterInfo.actionEventModifiers)
        {
            actionEventModifiers[actionEventModifier.eventTriggerType].Add(actionEventModifier);
        }
        turnCounter = new TurnCounter(this);
    }

    // protected void Start()
    // {
    //     gridManager = GetComponentInParent<GridManager>();
    //     healthDisplay = GetComponentInChildren<HealthDisplay>();
    //     maskController = GetComponentInChildren<MaskController>();
    // }

    private void Update()
    {
        float moveSpeed = 4f * moveSpeedMultiplier;
        if(moving)
        {
            if(Vector3.Distance(transform.position, moveDestination.transform.position) < 0.0001f)
            { 
                moving = false;
                moveDestination = null;
                moveSpeedMultiplier = 1f;
                ReturnToDefaultAnimation();
                SetDirection(defaultDirection);
            }
            else
            {
                Vector3 moveDirection = (moveDestination.transform.position - transform.position).normalized;
                if(!Mathf.Approximately(moveDirection.x, 0.0f) || !Mathf.Approximately(moveDirection.y, 0.0f))
                {
                    SetDirection(new Vector2(Mathf.Round(moveDirection.x), Mathf.Round(moveDirection.y)));
                }
                float step = moveSpeed * Time.deltaTime; 
                transform.position = Vector3.MoveTowards(transform.position, moveDestination.transform.position, step);   
            }
        }
    }

    public virtual void OnTurnStart()
    {
        if(statusEffectInstances.Count > 0)
        {
            for(int i = statusEffectInstances.Count - 1; i >= 0; i--)
            {
                statusEffectInstances[i].OnTurnStart(this);
                if(statusEffectInstances[i].statusEffectSO.hasDuration && statusEffectInstances[i].turnCounter <= 0)
                {
                    RemoveStatusEffect(statusEffectInstances[i]);
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
                // if(StatusEffectInstance is StatusEffectInstanceHealth)
                // {
                //     int healthChange = statusEffectInstance.GetHealthChange();
                //     if(healthChange > 0)
                //     {
                //         Heal(healthChange);
                //     }
                //     else if(healthChange < 0)
                //     {
                //         Damage(Mathf.Abs(healthChange));
                //     }
                // }
                statusEffectInstance.OnTurnEnd(this);
            }
        }
    }

    public void Knockback(Vector2Int newCoordinates)
    {
        Tile destinationTile = tile;
        destinationTile = gridManager.GetTileArray(combatantType)[newCoordinates.x, newCoordinates.y];
        if(destinationTile && destinationTile != tile && destinationTile.occupiers.Count < 3)
        {
            StartCoroutine(ChangeTile(destinationTile, "Stun")); 
        }
    }

    public IEnumerator ChangeTile(Tile newTile, string moveAnimation)
    {
        Debug.Log("change tile1");
        Move(newTile.transform, moveAnimation);
        yield return new WaitUntil(() => !moving);
        SetTile(tile, newTile);

    }

    public void Move(Transform destination, string moveAnimation, float moveSpeedMultiplier = 1f)
    {
        if(destination.position != transform.position)
        {
            animator.SetTrigger(moveAnimation);
            moveDestination = destination;
            this.moveSpeedMultiplier = moveSpeedMultiplier;
            moving = true;
        }
    }

    public void SetTile(Tile originalTile, Tile newTile)
    {
        originalTile.UnassignOccupier(this);
        newTile.AssignOccupier(this);
        this.tile = newTile;
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

    public void TriggerActionEffectAnimation(string newAnimatorTrigger)
    {
        if(!lockAnimation && newAnimatorTrigger != "")
        {
            animator.SetTrigger(newAnimatorTrigger);
        }
    }

    public void ReturnToDefaultAnimation()
    {
        if(!lockAnimation)
        {
            if(overrideAnimation == "")
            {
                animator.SetTrigger("Idle");
            }
            else
            {
                animator.SetTrigger(overrideAnimation);
            }
        }
    }

    public virtual void Damage(float amount, bool isCrit = false)
    {
        hp.ChangeCurrentValue(-amount);
        healthDisplay.DisplayHealthChange(PopupType.Damage, -amount, isCrit);
    }
    public virtual void Heal(float amount, bool isCrit = false)
    {
        hp.ChangeCurrentValue(amount);
        healthDisplay.DisplayHealthChange(PopupType.Heal, amount, isCrit);
    }
    public virtual void ResolveHealthChange()
    {
        healthDisplay.ResolveHealthChange();
    }

    public void DisplayMessage(PopupType popupType, string message)
    {
        healthDisplay.DisplayMessage(popupType, message);
    }

    public void ApplyTurnModifier(float newModifier)
    {
        Debug.Log(characterName + ": " + turnCounter.GetValue());
        turnCounter.ChangeModifier(newModifier);
        Debug.Log(characterName + ": " + turnCounter.GetValue());
        // onTurnCounterChange.Raise(this.gameObject);
    }

    public void AddStatusEffect(StatusEffectSO newStatusEffectSO)
    {
        //clear duplicates
        foreach(StatusEffectInstance thisStatusInstance in statusEffectInstances)
        {
            if(thisStatusInstance.statusEffectSO == newStatusEffectSO)
            {
                if(thisStatusInstance.statusEffectSO.cannotRefresh)
                {
                    return;
                }
                RemoveStatusEffect(thisStatusInstance);
            }
            if(thisStatusInstance.statusEffectSO == newStatusEffectSO.effectToCancel)
            {
                RemoveStatusEffect(thisStatusInstance);
                if(newStatusEffectSO.cancelSelf)
                {
                    return;
                }
            }
        }
        StatusEffectInstance newStatusInstance = newStatusEffectSO.CreateInstance();   
        statusEffectInstances.Add(newStatusInstance);  
        
        newStatusInstance.OnApply(this);

        if(newStatusInstance.statusEffectSO.effectObject != null)
            StartCoroutine(TriggerStatusAnimationCo(newStatusInstance.statusEffectSO.effectObject));     
        if(newStatusEffectSO.animatorTrigger != null)
            animator.SetTrigger(newStatusEffectSO.animatorTrigger);
        healthDisplay.AddStatusIcon(newStatusEffectSO);
    }

    protected IEnumerator TriggerStatusAnimationCo(GameObject effectPrefab)
    {
        float duration = 0.5f;
        GameObject effectObject = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        //destory visual effect
        yield return new WaitForSeconds(duration);
        Destroy(effectObject);
    }

    public void RemoveStatusEffect(StatusEffectInstance statusEffectInstance)
    {
        if(statusEffectInstances.Contains(statusEffectInstance) && statusEffectInstance.statusEffectSO.canRemove)
        {
            statusEffectInstance.OnRemove(this);

            statusEffectInstances.Remove(statusEffectInstance);
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

    public void ChangeSelectState(bool canSelect)
    {
        targetSelect.ToggleButton(canSelect); 
    }

    public void Target()
    {
        maskController.TriggerTargeted();
        // healthDisplay.Display(true);
    }

    public void Detarget()
    {
        maskController.EndAnimation();
        // healthDisplay.Display(false);
    }
}
