using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.ParticleSystem;

public enum CombatantTargetState
{
    Default,
    Targetable,
    Untargetable
}

public class Combatant : MonoBehaviour
{
    [Header("Sprite and Animations")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Material defaultMaterial;
    [SerializeField] protected Material greyscaleMaterial;
    [SerializeField] protected string idleAnimationOverride = "";
    protected Dictionary<StatusEffect, GameObject> statusVFXs = new Dictionary<StatusEffect, GameObject>();
    [SerializeField] private Transform belowBindPosition;
    [SerializeField] private Transform centerBindPosition;
    [SerializeField] private Transform aboveBindPosition;
    [SerializeField] private Transform frontBindPosition;
    [SerializeField] private Transform backBindPosition;
    protected float baseMoveSpeed = 4f;
    [SerializeField] protected GameObject critEffectPrefab;

    [Header("Mask")]
    [SerializeField] protected MaskController targetableMaskController;
    [SerializeField] protected MaskController targetedMaskController;
    [SerializeField] protected Color selectedColor;
    [SerializeField] protected Color unselectableColor;

    [Header("External References")]
    protected BattleManager battleManager;
    protected GridManager gridManager;

    [Header("Child Scripts")]
    [SerializeField] protected HealthDisplay healthDisplay;
    [SerializeField] protected TargetSelect targetSelect;

    [Header("Signals")]
    [SerializeField] protected SignalSenderBattleEvent onAddBattleEvent;

    protected WaitForSeconds wait05 = new WaitForSeconds(0.5f);
    protected WaitForSeconds wait1 = new WaitForSeconds(1f);

    public bool IsCasting { get; protected set; } = false;
    public bool IsKOed { get; protected set; } = false;
    public bool CanRevive { get; protected set; } = false;
    public bool IsLoaded { get; protected set; } = false;
    public CombatantType CombatantType { get; protected set; }
    public string CharacterName { get; protected set; }
    public string CharacterLetter { get; protected set; } = "";
    public Sprite TurnIcon { get; private set; }
    public DynamicStat HP { get; protected set; }
    public DynamicStat Barrier { get; protected set; }
    public DynamicStat MP { get; protected set; }
    public Dictionary<StatType, Stat> Stats { get; protected set; } = new Dictionary<StatType, Stat>();
    public Dictionary<SecondaryStatType, float> SecondaryStats { get; protected set; } = new Dictionary<SecondaryStatType, float>();
    public Dictionary<ElementalProperty, ElementalResistance> Resistances { get; protected set; } = new Dictionary<ElementalProperty, ElementalResistance>();
    public Dictionary<ElementalProperty, int> ResistanceModifiers { get; protected set; } = new Dictionary<ElementalProperty, int>();
    public List<TraitInstance> TraitInstances { get; protected set; } = new List<TraitInstance>();
    public Dictionary<BattleEventType, Dictionary<ActionModifierType, List<ActionModifier>>> ActionModifiers { get; protected set; } = new Dictionary<BattleEventType, Dictionary<ActionModifierType, List<ActionModifier>>>();
    public Dictionary<BattleEventType, List<TriggerableBattleEffect>> TriggerableBattleEffects { get; protected set; } = new Dictionary<BattleEventType, List<TriggerableBattleEffect>>();
    public List<StatusEffectInstance> StatusEffectInstances { get; protected set; } = new List<StatusEffectInstance>();
    public Tile Tile { get; protected set; }
    public Dictionary<CombatantBindPosition, Transform> CombatantBindPositions { get; protected set; }

    #region Setup
protected virtual void Awake()
    {
        //child components
        healthDisplay = GetComponentInChildren<HealthDisplay>();
        CombatantBindPositions = new Dictionary<CombatantBindPosition, Transform>();
        CombatantBindPositions.Add(CombatantBindPosition.Below, belowBindPosition);
        CombatantBindPositions.Add(CombatantBindPosition.Center, centerBindPosition);
        CombatantBindPositions.Add(CombatantBindPosition.Above, aboveBindPosition);
        CombatantBindPositions.Add(CombatantBindPosition.Front, frontBindPosition);
        CombatantBindPositions.Add(CombatantBindPosition.Back, backBindPosition);
    }

    public virtual void SetCharacterData(CharacterInfo characterInfo, PlayableCharacterID playableCharacterID = PlayableCharacterID.None)
    {
        gridManager = GetComponentInParent<GridManager>();
        //combatant info
        //CharacterInfo = characterInfo;
        SetName(characterInfo.CharacterName);
        TurnIcon = characterInfo.TurnIcon;
        
        //hp/mp
        HP = new DynamicStat(characterInfo.GetStat(StatType.HP), characterInfo.GetStat(StatType.HP));
        Barrier = new DynamicStat(characterInfo.GetStat(StatType.HP), 0);
        MP = new DynamicStat(characterInfo.GetStat(StatType.MP), characterInfo.GetStat(StatType.MP));

        //stats
        foreach (StatType statType in System.Enum.GetValues(typeof(StatType)))
        {
            Stats.Add(statType, new Stat(characterInfo.GetStat(statType)));
        }
        foreach (SecondaryStatType secondaryStatType in System.Enum.GetValues(typeof(SecondaryStatType)))
        {
            SecondaryStats.Add(secondaryStatType, characterInfo.SecondaryStats[secondaryStatType]);
        }

        //resistances
        Resistances = characterInfo.Resistances;
        foreach (KeyValuePair<ElementalProperty, List<int>> modifier in characterInfo.ResistanceModifiers)
        {
            ResistanceModifiers.Add(modifier.Key, (modifier.Value).Sum());
        }

        //triggerable effects
        //create
        TriggerableBattleEffects.Add(BattleEventType.Acting, new List<TriggerableBattleEffect>());
        TriggerableBattleEffects.Add(BattleEventType.Targeted, new List<TriggerableBattleEffect>());
        //populate
        foreach (TriggerableBattleEffect triggerableBattleEffect in characterInfo.TriggerableBattleEffects)
        {
            TriggerableBattleEffects[triggerableBattleEffect.BattleEventType].Add(triggerableBattleEffect);
        }

        //action modifiers
        //create
        ActionModifiers.Add(BattleEventType.Acting, new Dictionary<ActionModifierType, List<ActionModifier>>());
        foreach (ActionModifierType actionModifierType in System.Enum.GetValues(typeof(ActionModifierType)))
        {
            ActionModifiers[BattleEventType.Acting].Add(actionModifierType, new List<ActionModifier>());
        }
        ActionModifiers.Add(BattleEventType.Targeted, new Dictionary<ActionModifierType, List<ActionModifier>>());
        foreach (ActionModifierType actionModifierType in System.Enum.GetValues(typeof(ActionModifierType)))
        {
            ActionModifiers[BattleEventType.Targeted].Add(actionModifierType, new List<ActionModifier>());
        }
        //populate
        foreach (ActionModifier actionModifier in characterInfo.ActionModifiers)
        {
            ActionModifiers[actionModifier.BattleEventType][actionModifier.ActionModifierType].Add(actionModifier);
        }

        //traits
        foreach (Trait trait in characterInfo.Traits)
        {
            TraitInstances.Add(new TraitInstance(trait));
            foreach (TriggerableBattleEffect triggerableBattleEffect in trait.TriggerableBattleEffects)
            {
                TriggerableBattleEffects[triggerableBattleEffect.BattleEventType].Add(triggerableBattleEffect);
            }
            foreach (ActionModifier actionModifier in trait.ActionModifiers)
            {
                ActionModifiers[actionModifier.BattleEventType][actionModifier.ActionModifierType].Add(actionModifier);
            }
        }
    }

    public void SetName(string name, string letter = "")
    {
        CharacterName = name;
        CharacterLetter = letter;
    }

    public void SetBattleManager(BattleManager _battleManager)
    {
        battleManager = _battleManager;
    }

    public void SetUICamera(Camera camera)
    {
        targetSelect.SetUICamera(camera);
        healthDisplay.SetUICamera(camera);
    }
    #endregion

    #region Battle Events

    public virtual void ApplyActionCost(ActionCostType actionCostType, int cost)
    {
        Debug.Log("applying cost: " + actionCostType);
        if (actionCostType == ActionCostType.HP)
        {
            OnDamaged(cost, false, ElementalProperty.None);

            ResolveHealthChange();
        }
    }

    public virtual IEnumerator OnTurnStartCo()
    {
        for (int i = StatusEffectInstances.Count - 1; i >= 0; i--)
        {
            if (StatusEffectInstances[i].StatusEffect.TickAtTurnStart)
            {
                //trigger health effects
                if (StatusEffectInstances[i].StatusEffect.HealthEffectType != HealthEffectType.None)
                {
                    yield return StartCoroutine(TriggerStatusHealthEffectCo(StatusEffectInstances[i]));
                }
                //tick
                StatusEffectInstances[i].Tick();
                if (StatusEffectInstances[i].StatusEffect.RemoveOnTick || StatusEffectInstances[i].Counter <= 0)
                {
                    RemoveStatusEffectInstance(StatusEffectInstances[i]);
                }
            }
        }
        yield return null;
    }

    public virtual IEnumerator OnTurnEndCo()
    {
        for (int i = StatusEffectInstances.Count - 1; i >= 0; i--)
        {
            if (!StatusEffectInstances[i].StatusEffect.TickAtTurnStart)
            {
                //trigger health effects
                if (StatusEffectInstances[i].StatusEffect.HealthEffectType != HealthEffectType.None)
                {
                    yield return StartCoroutine(TriggerStatusHealthEffectCo(StatusEffectInstances[i]));
                }
                //tick
                StatusEffectInstances[i].Tick();
                if (StatusEffectInstances[i].Counter <= 0)
                {
                    RemoveStatusEffectInstance(StatusEffectInstances[i]);
                }
            }
        }
        yield return null;
    }

    //public void Block()
    //{
    //    TriggerAnimation("Guard");
    //}

    //public bool BlockCheck()
    //{
    //    TriggerAnimation("Stun");
    //    return false;
    //}

    public void OnAttacked(int amount, bool isCrit, bool wasBlocked, ElementalProperty elementalProperty)
    {
        TriggerAnimation("Stun", false);
        if (isCrit)
        {
            StartCoroutine(OnCritDamageCo());
        }
        OnDamaged(amount, isCrit, elementalProperty);
    }

    protected IEnumerator OnCritDamageCo()
    {
        GameObject effectObject = Instantiate(critEffectPrefab, CombatantBindPositions[CombatantBindPosition.Center].position, critEffectPrefab.transform.rotation);
        effectObject.transform.parent = CombatantBindPositions[CombatantBindPosition.Center];

        effectObject.GetComponent<VFXParent>().TriggerAnimation();
        //destory visual effect
        yield return wait05;
        Destroy(effectObject);
    }

    public virtual void OnDamaged(int amount, bool isCrit, ElementalProperty elementalProperty)
    {
        HP.ChangeCurrentValue(-amount);

        if (HP.CurrentValue <= 0)
        {
            IsKOed = true;
        }

        StartCoroutine(healthDisplay.DisplayHealthChange());
        healthDisplay.DisplayPopup(PopupType.Damage, CombatantType, amount.ToString(), isCrit, Resistances[elementalProperty]);
    }

    public virtual void OnHealed(int amount, bool isCrit = false)
    {
        HP.ChangeCurrentValue(amount);

        StartCoroutine(healthDisplay.DisplayHealthChange());
        healthDisplay.DisplayPopup(PopupType.Heal, CombatantType, amount.ToString(), isCrit);
    }

    public void OnEvade()
    { 
        TriggerAnimation("Move", false);

        healthDisplay.DisplayPopup(PopupType.Miss, CombatantType, "MISS");
    }

    public virtual void OnKO()
    {
        animator.SetTrigger("Die");
    }

    public virtual void OnRevive(float percentOfHPToRestore)
    {
        IsKOed = false;
        //get new health value
        int newHP = Mathf.FloorToInt(HP.GetValue() * percentOfHPToRestore);
        OnHealed(newHP);
        //trigger animation
        animator.SetTrigger("Revive");
        //do battle manager stuff
        battleManager.ReviveCombatant(this);
    }

    public virtual void OnCastStart()
    {
        IsCasting = true;
    }

    public virtual void OnCastEnd() 
    {
        IsCasting = false;
    }

    //public IEnumerator TriggerBattleEffectsCo(BattleEventType battleEventType, List<ActionSubevent> actionSubevents)
    //{
    //    List<BattleEvent> battleEvents = new List<BattleEvent>();

    //    //check triggerable effects
    //    foreach (TriggerableBattleEffect triggerableBattleEffect in TriggerableBattleEffects[battleEventType])
    //    {

    //        //go through each actor/target subevent
    //        foreach (ActionSubevent actionSubevent in actionSubevents)
    //        {
    //        }
    //        if (targets.Count > 0) 
    //        {
    //            BattleEvent battleEvent = new BattleEvent(this, targets, triggerableBattleEffect);
    //            battleEvents.Add(battleEvent);
    //        }
    //    }
    //    foreach (BattleEvent battleEvent in battleEvents)
    //    {
    //        onAddBattleEvent.Raise(battleEvent);
    //    }
    //    yield return null;
    //}
    #endregion

    #region Movement and Grid

    public IEnumerator Move(Transform destination, string moveAnimation, float moveSpeedMultiplier = 1f)
    {
        animator.SetTrigger(moveAnimation);
        while (destination.position != transform.position)
        {
            float step = (baseMoveSpeed * moveSpeedMultiplier) * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, destination.position, step);
            Vector2 moveDirection = transform.position - destination.position;
            yield return null;
        }
        transform.position = destination.position;
        yield return null;
    }

    public void SetTile(Tile newTile)
    {
        Tile = newTile;
    }
    #endregion

    #region Animations

    public void TriggerAnimation(string newAnimatorTrigger, bool overrideDefaultIdleAnimation)
    {
        animator.SetTrigger(newAnimatorTrigger);
        if(overrideDefaultIdleAnimation)
        {
            idleAnimationOverride = newAnimatorTrigger;
        }
    }

    public void ReturnToDefaultAnimation()
    {
        if(idleAnimationOverride != "")
        {
            TriggerAnimation(idleAnimationOverride, false);
        }
        else
        {
            TriggerAnimation("Idle", false);
        }
    }

    public void StartTimeStop()
    {
            spriteRenderer.material = greyscaleMaterial;
            animator.speed = 0;
    }

    public void EndTimeStop()
    {
        spriteRenderer.material = defaultMaterial;
        animator.speed = 1;
    }
    #endregion

    #region Health
    public virtual void DisplayHealthBar(bool shouldDisplay)
    {
        healthDisplay.Display(shouldDisplay);
    }

    public virtual void ResolveHealthChange()
    {
        StartCoroutine(healthDisplay.ResolveHealthChange());
    }

    #endregion

    #region Turn Counter
    //add value
    public void ApplyTurnModifier(float newModifier)
    {
        //    battleManager.ApplyTurnModifier(this, newModifier, false);
    }
    #endregion

    #region Status Effects
    protected IEnumerator TriggerStatusAnimationCo(GameObject effectPrefab)
    {
        GameObject effectObject = Instantiate(effectPrefab, CombatantBindPositions[CombatantBindPosition.Center].position, effectPrefab.transform.rotation);
        effectObject.transform.parent = CombatantBindPositions[CombatantBindPosition.Center];
        //destory visual effect
        yield return wait05;
        Destroy(effectObject);
    }

    protected IEnumerator TriggerStatusHealthEffectCo(StatusEffectInstance statusEffectInstance)
    {
        if (statusEffectInstance.StatusEffect.TriggerVFX != null)
        {
            yield return TriggerStatusAnimationCo(statusEffectInstance.StatusEffect.TriggerVFX);
        }
        
        if (statusEffectInstance.StatusEffect.HealthEffectType == HealthEffectType.Damage)
        {
            OnDamaged(statusEffectInstance.Potency, false, ElementalProperty.None);
        }
        else if (statusEffectInstance.StatusEffect.HealthEffectType == HealthEffectType.Heal)
        {
            OnHealed(statusEffectInstance.Potency, false);
        }
        yield return null;
    }

    public void AddStatusEffect(StatusEffect newStatusEffect, int potency = 0)
    {
        Debug.Log("Applying status: " + newStatusEffect.EffectName);
        //create new instance
        StatusEffectInstance newStatusInstance = new StatusEffectInstance(newStatusEffect, potency);

        //check if target already has status
        for (int i = StatusEffectInstances.Count - 1; i >= 0; i--)
        {
            StatusEffectInstance thisStatus = StatusEffectInstances[i];
            if (thisStatus.StatusEffect == newStatusEffect || newStatusEffect.EffectsToRemove.Contains(thisStatus.StatusEffect)) 
            {
                RemoveStatusEffectInstance(thisStatus);
            }
        }

        //add to status instance list
        StatusEffectInstances.Add(newStatusInstance);

        //set up
        foreach (TriggerableBattleEffect triggerableBattleEffect in newStatusEffect.TriggerableBattleEffects)
        {
            TriggerableBattleEffects[triggerableBattleEffect.BattleEventType].Add(triggerableBattleEffect);
        }
        foreach (ActionModifier actionModifier in newStatusEffect.ActionModifiers)
        {
            ActionModifiers[actionModifier.BattleEventType][actionModifier.ActionModifierType].Add(actionModifier);
        }

        //spawn trigger animation effect
        if (newStatusInstance.StatusEffect.TriggerVFX != null)
        {
            StartCoroutine(TriggerStatusAnimationCo(newStatusInstance.StatusEffect.TriggerVFX));
        }

        //trigger popup
        PopupType popupType = PopupType.Damage;
        if (newStatusInstance.StatusEffect.StatusEffectType == StatusEffectType.Buff)
        {
            popupType = PopupType.Buff;
        }
        else if (newStatusInstance.StatusEffect.StatusEffectType == StatusEffectType.Debuff)
        {
            popupType = PopupType.Debuff;
        }
        StartCoroutine(healthDisplay.DisplayHealthChange());

        //spawn persistent animation effect
        if (newStatusInstance.StatusEffect.PersistentVFX != null)
        {
            GameObject effectObject = Instantiate(newStatusInstance.StatusEffect.PersistentVFX, CombatantBindPositions[CombatantBindPosition.Center].position, newStatusInstance.StatusEffect.PersistentVFX.transform.rotation);
            effectObject.transform.parent = CombatantBindPositions[CombatantBindPosition.Center];
            statusVFXs.Add(newStatusInstance.StatusEffect, effectObject);
        }

        //start target animation
        if (newStatusEffect.AnimatorOverride != BattleAnimatorTrigger.None)
        {
            if (newStatusEffect.AnimatorOverride == BattleAnimatorTrigger.Custom)
            {
                idleAnimationOverride = newStatusEffect.CustomAnimatorOverride;
                animator.SetTrigger(idleAnimationOverride);
            }
            else
            {
                idleAnimationOverride = newStatusEffect.AnimatorOverride.ToString();
                animator.SetTrigger(idleAnimationOverride);
            }
        }

        //change target sprite speed
        animator.speed = animator.speed + newStatusEffect.TargetSpeedChange;
    }

    public void RemoveStatusEffect(StatusEffect statusEffectToRemove)
    {
        foreach (StatusEffectInstance statusEffectInstance in StatusEffectInstances)
        {
            if(statusEffectInstance.StatusEffect == statusEffectToRemove)
            {
                RemoveStatusEffectInstance(statusEffectInstance);
                break;
            }

        }
    }

    protected void RemoveStatusEffectInstance(StatusEffectInstance statusEffectInstance)
    {
        //remove from status list
        StatusEffectInstances.Remove(statusEffectInstance);

        //clear effects
        foreach (TriggerableBattleEffect triggerableBattleEffect in statusEffectInstance.StatusEffect.TriggerableBattleEffects)
        {
            TriggerableBattleEffects[triggerableBattleEffect.BattleEventType].Remove(triggerableBattleEffect);
        }
        foreach (ActionModifier actionModifier in statusEffectInstance.StatusEffect.ActionModifiers)
        {
            ActionModifiers[actionModifier.BattleEventType][actionModifier.ActionModifierType].Remove(actionModifier);
        }

        //reset target animation
        if (statusEffectInstance.StatusEffect.AnimatorOverride != BattleAnimatorTrigger.None)
        {
            string triggerToRemove = statusEffectInstance.StatusEffect.AnimatorOverride.ToString();
            if (triggerToRemove == "Custom")
            {
                triggerToRemove = statusEffectInstance.StatusEffect.CustomAnimatorOverride;
            }

            if (idleAnimationOverride == triggerToRemove)
            {
                idleAnimationOverride = "";
                ReturnToDefaultAnimation();
            }
        }

        //change target sprite speed to default
        animator.speed = animator.speed - statusEffectInstance.StatusEffect.TargetSpeedChange;

        //remove vfx
        GameObject effectToRemove = statusVFXs[statusEffectInstance.StatusEffect];
        statusVFXs.Remove(statusEffectInstance.StatusEffect);
        Destroy(effectToRemove);
    }

    public void RemoveAllStatusEffects(StatusEffectType statusEffectType)
    {
        for (int i = StatusEffectInstances.Count - 1; i >= 0; i--)
        {
            StatusEffectInstance statusEffectInstance = StatusEffectInstances[i];
            if (StatusEffectInstances[i].StatusEffect.CanRemove && StatusEffectInstances[i].StatusEffect.StatusEffectType == statusEffectType)
            {
                RemoveStatusEffectInstance(statusEffectInstance);
            }
        }
    }

    public void ModifyStatusStacks(StatusEffect statusEffectToModify, int amount)
    {
        for (int i = StatusEffectInstances.Count - 1; i >= 0; i--)
        {
            StatusEffectInstance statusEffectInstance = StatusEffectInstances[i];
            if (statusEffectInstance.StatusEffect == statusEffectToModify)
            {
                statusEffectInstance.ModifyStacks(amount);
                if (statusEffectInstance.Counter <= 0)
                {
                    RemoveStatusEffectInstance(statusEffectInstance);
                }
                break;
            }
        }
    }
    #endregion

    #region Target Select
    public void ChangeSelectState(CombatantTargetState combatantTargetState)
    {
        switch (combatantTargetState)
        {
            case CombatantTargetState.Targetable:
                targetSelect.ToggleButton(true);
                targetableMaskController.RemoveTint();
                break;
            case CombatantTargetState.Untargetable:
                targetSelect.ToggleButton(false);
                targetableMaskController.ApplyTint(unselectableColor, false);
                break;
            default:
                targetSelect.ToggleButton(false);
                targetableMaskController.RemoveTint();
                break;
        }
    }

    public void Select()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(targetSelect.gameObject);
    }

    public void ToggleTargeted(bool isTargeted)
    {
        if(isTargeted)
        {
            targetedMaskController.ApplyTint(selectedColor, true);
        } 
        else
        {
            targetedMaskController.RemoveTint();
        }
    }
    #endregion
}
