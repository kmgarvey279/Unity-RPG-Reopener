using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CombatantState
{
    Default, 
    Casting, 
    Defending,
    KO, 
    Frozen
}

public enum CombatantTargetState
{
    Default,
    Targetable,
    Untargetable
}

//always default to false
public enum CombatantBool
{
    CanLinkWithSelfInChain,
    CannotContributeToChain,
    CannotBenefitFromChain,
    CannotHit,
    CannotTakeDamage,
    CannotBeHealed,
    CannotKill,
    CannotDamage,
    CannotUseMelee,
    CannotUseMagic
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
    protected BattleTimeline battleTimeline;

    [Header("Child Scripts")]
    [SerializeField] protected HealthDisplay healthDisplay;
    [SerializeField] protected TargetSelect targetSelect;

    [Header("Signals")]
    [SerializeField] protected SignalSenderBattleEvent onAddBattleEvent;


    protected WaitForSeconds wait05 = new WaitForSeconds(0.5f);
    protected WaitForSeconds wait1 = new WaitForSeconds(1f);

    public CombatantState CombatantState { get; private set; } = CombatantState.Default;
    public bool IsLoaded { get; protected set; } = false;
    public CombatantType CombatantType { get; protected set; }
    public string CharacterName { get; protected set; }
    public string CharacterLetter { get; protected set; } = "";
    public Sprite TurnIcon { get; private set; }
    public ClampInt HP { get; protected set; }
    public ClampInt Barrier { get; protected set; }
    public ClampInt MP { get; protected set; }
    public Dictionary<StatType, Stat> Stats { get; protected set; } = new Dictionary<StatType, Stat>();
    public Dictionary<SecondaryStatType, SecondaryStat> SecondaryStats { get; protected set; } = new Dictionary<SecondaryStatType, SecondaryStat>();
    public Dictionary<ElementalProperty, ElementalResistance> Resistances { get; protected set; } = new Dictionary<ElementalProperty, ElementalResistance>();
    public Dictionary<ElementalProperty, float> ResistanceModifiers { get; protected set; } = new Dictionary<ElementalProperty, float>();
    public Dictionary<CombatantBool, int> CombatantBools;
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

    public virtual IEnumerator SetCharacterData(CharacterInfo characterInfo, PlayableCharacterID playableCharacterID = PlayableCharacterID.None)
    {
        SetName(characterInfo.CharacterName);
        TurnIcon = characterInfo.TurnIcon;
        
        //hp/mp
        HP = new ClampInt(characterInfo.HP.CurrentValue, 0, characterInfo.HP.CurrentValue);
        Barrier = new ClampInt(0, 0, characterInfo.HP.CurrentValue);
        MP = new ClampInt(characterInfo.MP.CurrentValue, 0, characterInfo.MP.CurrentValue);

        //stats
        foreach (KeyValuePair<StatType, Stat> statEntry in characterInfo.Stats)
        {
            Stats.Add(statEntry.Key, new Stat(statEntry.Value.CurrentValue));
        }
        foreach (KeyValuePair<SecondaryStatType, SecondaryStat> statEntry in characterInfo.SecondaryStats)
        {
            SecondaryStats.Add(statEntry.Key, new SecondaryStat(statEntry.Value.CurrentValue));
        }

        //resistances
        Resistances = characterInfo.Resistances;
        foreach (KeyValuePair<ElementalProperty, List<float>> modifier in characterInfo.ResistanceModifiers)
        {
            ResistanceModifiers.Add(modifier.Key, (modifier.Value).Sum());
        }

        //bools, 0 == false, > 0 == true 
        CombatantBools = new Dictionary<CombatantBool, int>();
        foreach (CombatantBool combatantBool in System.Enum.GetValues(typeof(CombatantBool)))
        {
            CombatantBools.Add(combatantBool, 0);
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
        yield return null;
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
        int newHealthValue = HP.Value - amount;
        HP.UpdateValue(newHealthValue);

        if (HP.Value <= 0)
        {
            CombatantState = CombatantState.KO;
        }

        StartCoroutine(healthDisplay.DisplayHealthChange());
        healthDisplay.DisplayPopup(PopupType.Damage, CombatantType, amount.ToString(), isCrit, Resistances[elementalProperty]);
    }

    public virtual void OnHealed(int amount, bool isCrit = false)
    {
        int newHealthValue = HP.Value + amount;
        HP.UpdateValue(newHealthValue);

        StartCoroutine(healthDisplay.DisplayHealthChange());
        healthDisplay.DisplayPopup(PopupType.Heal, CombatantType, amount.ToString(), isCrit);
    }

    public void OnEvade()
    { 
        TriggerAnimation("Move", false);

        healthDisplay.DisplayPopup(PopupType.Miss, CombatantType, "MISS");
    }

    public virtual void OnCastStart()
    {
        CombatantState = CombatantState.Casting;
    }

    public virtual void OnCastEnd()
    {
        CombatantState = CombatantState.Default;
    }

    public virtual void OnKO()
    {
        animator.SetTrigger("Die");
    }

    public virtual void OnRevive(float percentOfHPToRestore)
    {
        CombatantState = CombatantState.Default;
        //get new health value
        int newHP = Mathf.FloorToInt(HP.MaxValue * percentOfHPToRestore);
        OnHealed(newHP);
        //trigger animation
        animator.SetTrigger("Revive");
        //do battle manager stuff
        battleManager.ReviveCombatant(this);
    }
    #endregion
    
    #region Combatant State / Bools

    //public virtual void SetCombatantState(CombatantState newState)
    //{
    //    if (CombatantState != newState)
    //    {
    //        CombatantState = newState;
    //    }
    //}

    public bool CheckBool(CombatantBool combatantBool)
    {
        if (CombatantBools[combatantBool] == 0)
        {
            return false;
        }
        return true;
    }

    public void ModifyCombatantBool(CombatantBool combatantBool, bool isTrue)
    {
        if (isTrue)
        {
            CombatantBools[combatantBool] += 1;
        }
        else
        {
            CombatantBools[combatantBool] -= 1;
            if (CombatantBools[combatantBool] < 0)
            {
                CombatantBools[combatantBool] = 0;
            }
        }
    }
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
        battleTimeline.ApplyTurnModifier(this, newModifier, false, false, 0);
    }
    #endregion

    #region Chain Multiplier
    //add value
    public float GetNewChainMultiplier(float originalMultiplier, List<Combatant> currentChain)
    {
        float newChainMultiplier = originalMultiplier;
        //if new chain
        if (currentChain.Count == 0)
        {
            newChainMultiplier += SecondaryStats[SecondaryStatType.ChainStartBonus].CurrentValue;
        }
        else
        {
            newChainMultiplier += (0.15f + SecondaryStats[SecondaryStatType.ChainContributionBonus].CurrentValue);
        }
        return newChainMultiplier;
    }

    public bool ChainBreakCheck(List<Combatant>currentChain)
    {
        if (this.CombatantType == CombatantType.Player)
        {
            if (CheckBool(CombatantBool.CanLinkWithSelfInChain))
            {
                if (currentChain.Count > 1 && currentChain[currentChain.Count - 1] == this && currentChain[currentChain.Count - 2] == this)
                {
                    return true;
                }
            }
            else if (currentChain.Count >= 1 && currentChain[currentChain.Count - 1] == this)
            {
                return true;
            }
        }
        return false;
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
