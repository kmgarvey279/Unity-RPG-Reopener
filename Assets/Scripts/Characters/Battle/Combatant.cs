using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CombatantState
{
    Default, 
    PreKO,
    KO
}

public enum CombatantTargetState
{
    Default,
    Targetable,
    Untargetable
}


public abstract class Combatant : MonoBehaviour
{
    [Header("Data")]
    [SerializeReference] protected CombatantBattleData combatantBattleData;
    #region Data Get/Set
    public string CharacterName
    {
        get
        {
            return combatantBattleData.CharacterName;
        }
        private set
        {
            combatantBattleData.CharacterName = value;
        }
    }
    public string CharacterLetter
    {
        get
        {
            return combatantBattleData.CharacterLetter;
        }
        private set
        {
            combatantBattleData.CharacterLetter = value;
        }
    }
    public Sprite TurnIcon
    {
        get
        {
            return combatantBattleData.TurnIcon;
        }
    }
    public int HP
    {
        get
        {
            return combatantBattleData.HP.CurrentValue;
        }
        protected set
        {
            combatantBattleData.HP.UpdateValue(value);
        }
    }
    public int MaxHP
    {
        get
        {
            return combatantBattleData.HP.MaxValue;
        }
    }
    public int MP
    {
        get
        {
            return combatantBattleData.MP.CurrentValue;
        }
        protected set
        {
            combatantBattleData.MP.UpdateValue(value);
        }
    }
    public int MaxMP
    {
        get
        {
            return combatantBattleData.MP.MaxValue;
        }
    }
    public int Barrier
    {
        get
        {
            return combatantBattleData.Barrier.CurrentValue;
        }
        protected set
        {
            combatantBattleData.Barrier.UpdateValue(value);
        }
    }
    public Dictionary<BattleEventType, Dictionary<UniversalModifierType, List<float>>> UniversalModifiers
    {
        get
        {
            return combatantBattleData.UniversalModifiers;
        }
    }
    public Dictionary<BattleEventType, Dictionary<ActionModifierType, List<ActionModifier>>> ActionModifiers
    {
        get
        {
            return combatantBattleData.ActionModifiers;
        }
    }
    public List<PreemptiveBattleEventTrigger> PreemptiveBattleEventTriggers
    {
        get
        {
            return combatantBattleData.PreemptiveBattleEventTriggers;
        }
    }
    public Dictionary<BattleEventType, List<BattleEventTrigger>> BattleEventTriggers
    {
        get
        {
            return combatantBattleData.BattleEventTriggers;
        }
    }
    public Dictionary<IntStatType, int> Stats
    {
        get
        {
            return combatantBattleData.Stats;
        }
    }
    public List<Trait> Traits
    {
        get
        {
            return combatantBattleData.Traits;
        }
    }
    public List<StatusEffectInstance> StatusEffectInstances
    {
        get
        {
            return combatantBattleData.StatusEffectInstances;
        }
    }
    #endregion
    [field: SerializeField] public CombatantType CombatantType { get; protected set; }
    [field: SerializeField] public CombatantState CombatantState { get; private set; } = CombatantState.Default;

    [Header("Sprite and Animations")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    [SerializeField] protected Material defaultMaterial;
    [SerializeField] protected Material greyscaleMaterial;

    protected string idleAnimationOverride = "";
    protected float defaultOpacity = 1f;
    //protected float defaultAnimatorSpeed = 1f;
    protected const float baseMoveSpeed = 20f;
    
    [Header("VFX")]
    protected Dictionary<StatusEffect, List<GameObject>> statusVFXs = new Dictionary<StatusEffect, List<GameObject>>();
    [SerializeField] private Transform belowBindPosition;
    [SerializeField] private Transform centerBindPosition;
    [SerializeField] private Transform aboveBindPosition;
    [SerializeField] private Transform frontBindPosition;
    [SerializeField] private Transform backBindPosition;
    [SerializeField] protected GameObject critVFXPrefab;
    [SerializeField] protected GameObject barrierVFXPrefab;

    [Header("Mask")]
    [SerializeField] protected MaskController targetableMaskController;
    [SerializeField] protected GameObject targetedGlow;
    [SerializeField] protected Color selectedColor;
    [SerializeField] protected Color unselectableColor;

    [Header("External References")]
    protected BattleManager battleManager;
    protected BattleTimeline battleTimeline;

    [Header("Health")]
    [SerializeField] protected HealthDisplay healthDisplay;

    [Header("Targeting")]
    [SerializeField] protected TargetSelect targetSelect;
    [SerializeField] protected GameObject selectedCursor;

    [Header("Signals")]
    [SerializeField] protected SignalSenderBattleEvent onAddBattleEvent;

    [Header("Misc.")]
    [SerializeField] protected StatusEffect barrierStatus;

    protected WaitForSeconds wait05 = new WaitForSeconds(0.5f);
    protected WaitForSeconds wait1 = new WaitForSeconds(1f);
    public bool IsLoaded { get; protected set; } = false;
    public bool OnBattleStartIsComplete { get; protected set; } = false;    
    public Tile Tile { get; protected set; }
    [field: SerializeField] public GameObject ProximityTile { get; protected set; }
    public Dictionary<CombatantSpawnPosition, Transform> CombatantSpawnPositions { get; protected set; }

    #region Setup
    protected virtual void Awake()
    {
        //child components
        CombatantSpawnPositions = new Dictionary<CombatantSpawnPosition, Transform>();
        CombatantSpawnPositions.Add(CombatantSpawnPosition.Below, belowBindPosition);
        CombatantSpawnPositions.Add(CombatantSpawnPosition.Center, centerBindPosition);
        CombatantSpawnPositions.Add(CombatantSpawnPosition.Above, aboveBindPosition);
        CombatantSpawnPositions.Add(CombatantSpawnPosition.Front, frontBindPosition);
        CombatantSpawnPositions.Add(CombatantSpawnPosition.Back, backBindPosition);
    }

    public void SetName(string characterName, string letter = "")
    {
        //this.name = name + " (" + letter + ")";
        CharacterName = characterName;
        CharacterLetter = letter;

        name = CharacterName;
        if (letter != "")
        {
            name += " (" + CharacterLetter + ")";
        }
    }

    public void SetExternalReferences(BattleManager _battleManager, BattleTimeline _battleTimeline)
    {
        battleManager = _battleManager;
        battleTimeline = _battleTimeline;
    }

    public virtual void SetUICamera(Camera camera)
    {
        targetSelect.SetUICamera(camera);
        healthDisplay.SetUICamera(camera);
    }
    #endregion

    #region Battle Events

    public IEnumerator OnBattleStart()
    {
        foreach (Trait trait in Traits)
        {
            foreach (StatusEffect statusEffect in trait.BattleStartStatusEffects)
            {
                if (statusEffect != null)
                {
                    //AddStatusEffect(statusEffect, 0);
                    //yield return wait1;
                    StatusEffectInstance newStatusInstance = new StatusEffectInstance(statusEffect, 0);
                    combatantBattleData.AddStatusInstance(newStatusInstance);
                }
            }
        }
        yield return null;
        OnBattleStartIsComplete = true;
    }

    public virtual void OnTurnStart()
    {
        for (int i = StatusEffectInstances.Count - 1; i >= 0; i--)
        {
            StatusEffectInstance statusEffectInstance = StatusEffectInstances[i];

            //tick
            StatusEffectInstances[i].Tick();

            if (statusEffectInstance.StatusEffect.TriggerTurnEventType == TurnEventType.OnStart)
            {
                TriggerStatusTurnEvents(statusEffectInstance);
            }
        }
    }

    public void OnTurnEnd()
    {
        for (int i = StatusEffectInstances.Count - 1; i >= 0; i--)
        {
            StatusEffectInstance statusEffectInstance = StatusEffectInstances[i];

            if (statusEffectInstance.StatusEffect.TriggerTurnEventType == TurnEventType.OnEnd)
            {
                TriggerStatusTurnEvents(statusEffectInstance);
            }
        }
        ResetBool(CombatantBool.TookDamageThisTurn);
    }


    public virtual void OnAttacked(int amount, bool isCrit, bool isVulnerable)
    {
    }

    public virtual void OnDamaged(int amount, bool isCrit, bool isVulnerable)
    {
        int newHealthValue = HP - amount;
        if (newHealthValue <= 0)
        {
            if (CheckBool(CombatantBool.CannotBeKO))
            {
                newHealthValue = 1;
                ModifyBool(CombatantBool.CannotBeKO, false);
            }
            else
            {
                ChangeCombatantState(CombatantState.PreKO);
            }
        }
        HP = newHealthValue;
        ModifyBool(CombatantBool.TookDamageThisTurn, true);

        healthDisplay.DisplayNumberPopup(PopupType.Damage, CombatantType, amount, isCrit, isVulnerable);
        DisplayBarChanges();
    } 

    public virtual void OnOneShot()
    {
        SpawnVFX(critVFXPrefab, CombatantSpawnPosition.Center);

        int newHealthValue = 0;
        if (combatantBattleData.CheckBool(CombatantBool.CannotBeKO))
        {
            newHealthValue = 1;
            ModifyBool(CombatantBool.CannotBeKO, false);
        }
        else
        {
            ChangeCombatantState(CombatantState.PreKO);
        }
        HP = newHealthValue;
        DisplayBarChanges();
    }

    public virtual void OnHealed(int amount, bool isCrit = false)
    {
        int newHealthValue = HP + amount;
        HP = newHealthValue;

        healthDisplay.DisplayNumberPopup(PopupType.Heal, CombatantType, amount, isCrit);
        DisplayBarChanges();
    }

    public virtual void OnApplyBarrier(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        //add status effect
        AddStatusEffect(barrierStatus, 0);

        //play apply animation
        SpawnVFX(barrierVFXPrefab, CombatantSpawnPosition.Center);

        //update value
        int newBarrierValue = Barrier + amount;
        combatantBattleData.Barrier.UpdateValue(newBarrierValue);

        DisplayBarChanges();
    }

    public int OnBarrierAbsorbDamage(int damageAmount)
    {
        SpawnVFX(barrierVFXPrefab, CombatantSpawnPosition.Center);

        int newDamageAmount = Mathf.Clamp(damageAmount - Barrier, 0, 9999);
        Debug.Log("damage after barrier: " + damageAmount);
        
        int newBarrierValue = Mathf.Clamp(Barrier - damageAmount, 0, 9999);
        Debug.Log("new barrier value: " + newBarrierValue);
        
        Barrier = newBarrierValue;

        return newDamageAmount;
    }

    public void OnEvade()
    { 
        TriggerAnimation("Move", false);

        healthDisplay.DisplayTextPopup(PopupType.Miss, "MISS");
    }

    public virtual void OnKO()
    {
        ChangeCombatantState(CombatantState.KO);
        animator.SetTrigger("Die");
    }

    public virtual void OnRevive(float percentOfHPToRestore)
    {
        if (CombatantState != CombatantState.KO)
        {
            return;
        }

        CombatantState = CombatantState.Default;
        //get new health value
        int newHP = Mathf.Clamp(Mathf.CeilToInt(MaxHP * percentOfHPToRestore), 1, MaxHP);
        OnHealed(newHP);
        //trigger animation
        animator.SetTrigger("Revive");
        //do battle manager stuff
        battleManager.ReviveCombatant(this);
    }
    #endregion

    #region Combatant State / Bools

    public virtual void ChangeCombatantState(CombatantState newState)
    {
        if (CombatantState != newState)
        {
            CombatantState = newState;
        }
    }

    public virtual void ModifyBool(CombatantBool combatantBool, bool isTrue)
    {
        if (isTrue)
        {
            combatantBattleData.CombatantBools[combatantBool] += 1;
        }
        else
        {
            combatantBattleData.CombatantBools[combatantBool] -= 1;
            if (combatantBattleData.CombatantBools[combatantBool] < 0)
            {
                combatantBattleData.CombatantBools[combatantBool] = 0;
            }
        }
    }

    public void ResetBool(CombatantBool combatantBool)
    {
        combatantBattleData.CombatantBools[combatantBool] = 0;
    }

    public bool CheckBool(CombatantBool combatantBool)
    {
        if (combatantBattleData.CombatantBools[combatantBool] == 0)
        {
            return false;
        }
        return true;
    }
    #endregion

    #region Movement and Grid

    public IEnumerator Move(Transform destination, string moveAnimation)
    {
        if (destination.position != transform.position)
        {
            animator.SetTrigger(moveAnimation);
        }
        while (destination.position != transform.position)
        {
            float step = baseMoveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, destination.position, step);
            yield return null;
        }
        transform.position = destination.position;
        yield return null;
    }

    public IEnumerator ReturnToDefaultPosition()
    {
        yield return Move(Tile.transform, "Idle");
    }

    public void SetTile(Tile newTile)
    {
        Tile = newTile;
    }
    #endregion

    #region Sprite and Animations

    public void TriggerAnimation(string newAnimatorTrigger, bool overrideDefaultIdleAnimation)
    {
        animator.SetTrigger(newAnimatorTrigger);
        if (overrideDefaultIdleAnimation)
        {
            idleAnimationOverride = newAnimatorTrigger;
        }
    }

    public void ReturnToDefaultAnimation()
    {
        if (idleAnimationOverride != "")
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

    public IEnumerator ChangeOpacityCo(float newOpacity, bool setAsDefault)
    {
        if (setAsDefault)
            defaultOpacity = newOpacity;

        float start = spriteRenderer.color.a;
        float duration = 0.5f;
        float counter = 0;
        while (spriteRenderer.color.a != newOpacity)
        {
            float newAValue = Mathf.Lerp(start, newOpacity, (counter / duration));
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAValue);
            counter += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newOpacity);
    }

    public void SetOpacityToDefault()
    {
        if(spriteRenderer.color.a != defaultOpacity)
            StartCoroutine(ChangeOpacityCo(defaultOpacity, false));
    }

    protected void SpawnVFX(GameObject effectPrefab, CombatantSpawnPosition spawnPosition)
    {
        GameObject effectObject = Instantiate(effectPrefab, CombatantSpawnPositions[spawnPosition].position, effectPrefab.transform.rotation);
        effectObject.transform.parent = CombatantSpawnPositions[spawnPosition];

        VFXParent vfxParent = effectObject.GetComponent<VFXParent>();
        if (vfxParent == null)
        {
            Debug.Log("no vfx script found");
            Destroy(effectObject);
        }
        else
        {
            Debug.Log("starting vfx animation");
            vfxParent.TriggerAnimation();
        }
    }
    #endregion

    #region Health/Mana
    public virtual void DisplayHealthInfo()
    {
        healthDisplay.Display();
    }

    public virtual void HideHealthInfo()
    {
        healthDisplay.Hide();
    }

    //called internally
    protected virtual void DisplayBarChanges()
    {
        healthDisplay.DisplayChanges();
    }

    //called externally
    public virtual void ResolveBarChanges()
    {
        healthDisplay.ResolveChanges();
    }

    public virtual void ApplyActionHPCost(float percentageCost)
    {
        int intCost = Mathf.FloorToInt(percentageCost / HP);

        int newHealthValue = Mathf.Clamp(HP - intCost, 1, MaxHP);
        combatantBattleData.HP.UpdateValue(newHealthValue);

        DisplayBarChanges();
    }

    #endregion

    #region Turn Counter
    //add value
    public void ApplyTurnModifier(float newModifier, bool applyToNextTurnOnly)
    {
        if (newModifier == 0)
        {
            return;
        }
        battleTimeline.ApplyTurnModifier(this, newModifier, false, applyToNextTurnOnly);
    }

    public void RemoveOneTurnModifiers()
    {
        battleTimeline.RemoveOneTurnModifiers(this);
    }

    public int GetNextTurnIndex(bool includeInterventions)
    {
        return battleTimeline.GetNextTurnIndex(this, includeInterventions);
    }
    #endregion

    #region Status Effects
    public void AddStatusEffect(StatusEffect newStatusEffect, int potency)
    {
        bool hasStatus = false;
        for (int i = StatusEffectInstances.Count - 1; i >= 0; i--)
        {
            StatusEffectInstance thisStatusInstance = StatusEffectInstances[i];

            //if the target already has an instance of the same status effect
            if (thisStatusInstance.StatusEffect == newStatusEffect) 
            {
                hasStatus = true;
                HandleDuplicateStatus(thisStatusInstance, potency);
            }

            if (newStatusEffect.EffectsToRemove.Contains(thisStatusInstance.StatusEffect))
            {
                RemoveStatusEffectInstance(thisStatusInstance, false);
            }
        }

        //apply effects (if not just being refreshed)
        if (!hasStatus)
        {
            HandleNewStatus(newStatusEffect, potency);
        }
    }

    public void HandleDuplicateStatus(StatusEffectInstance thisStatusInstance, int newPotency)
    {
        //overwrite or increase potency
        Debug.Log("Status found, changing potency. Original potency: " + thisStatusInstance.Potency);
        thisStatusInstance.ModifyPotency(newPotency);
        Debug.Log("Status found. New potency: " + thisStatusInstance.Potency);

        //increase duration
        if (thisStatusInstance.StatusEffect.HasDuration
        && thisStatusInstance.StatusEffect.CanIncreaseDuration
        && thisStatusInstance.Duration.CurrentValue < thisStatusInstance.StatusEffect.DurationMax)
        {
            thisStatusInstance.ModifyDuration(thisStatusInstance.StatusEffect.DurationToApply);
        }

        //increase stacks
        if (thisStatusInstance.StatusEffect.HasStacks
            && thisStatusInstance.StatusEffect.CanIncreaseStacks
            && thisStatusInstance.Stacks.CurrentValue < thisStatusInstance.StatusEffect.StacksMax)
        {
            int originalStackValue = thisStatusInstance.Stacks.CurrentValue;
            thisStatusInstance.ModifyStacks(thisStatusInstance.StatusEffect.StacksToApply);

            int stackChange = thisStatusInstance.Stacks.CurrentValue - originalStackValue;

            //display popup icon
            if (thisStatusInstance.StatusEffect.DisplayIcon)
            {
                healthDisplay.QueueAddStatusPopup(thisStatusInstance.StatusEffect, stackChange);
            }
        }
    }

    public virtual void HandleNewStatus(StatusEffect newStatusEffect, int potency)
    {
        StatusEffectInstance newStatusInstance = new StatusEffectInstance(newStatusEffect, potency);

        //add to status instance list
        combatantBattleData.AddStatusInstance(newStatusInstance);

        //turn modifier
        ApplyTurnModifier(newStatusEffect.TurnModifier, true);
        newStatusInstance.OnApplyTurnModifier(newStatusEffect.TurnModifier);

        //display popup icon
        if (newStatusInstance.StatusEffect.DisplayIcon)
        {
            healthDisplay.QueueAddStatusPopup(newStatusInstance.StatusEffect, 0);
        }

        //spawn persistent animation effect on combatant
        if (newStatusInstance.StatusEffect.PersistentVFX != null)
        {
            GameObject effectObject = Instantiate(newStatusInstance.StatusEffect.PersistentVFX, CombatantSpawnPositions[newStatusInstance.StatusEffect.PersistentVFXPosition].position, newStatusInstance.StatusEffect.PersistentVFX.transform.rotation);
            effectObject.transform.parent = CombatantSpawnPositions[newStatusInstance.StatusEffect.PersistentVFXPosition];
            if (!statusVFXs.ContainsKey(newStatusInstance.StatusEffect))
            {
                statusVFXs.Add(newStatusEffect, new List<GameObject>());
            }
            statusVFXs[newStatusInstance.StatusEffect].Add(effectObject);

            VFXParent vfxParent = effectObject.GetComponent<VFXParent>();
            if (vfxParent != null)
            {
                vfxParent.TriggerAnimation(false);
            }
        }

        //spawn persistent animation effect on tile
        if (newStatusInstance.StatusEffect.PersistentTileVFX != null)
        {
            GameObject effectObject = Instantiate(newStatusInstance.StatusEffect.PersistentTileVFX, Tile.transform.position, Quaternion.identity);
            effectObject.transform.parent = Tile.transform;
            if (!statusVFXs.ContainsKey(newStatusInstance.StatusEffect))
            {
                statusVFXs.Add(newStatusEffect, new List<GameObject>());
            }
            statusVFXs[newStatusInstance.StatusEffect].Add(effectObject);

            VFXParent vfxParent = effectObject.GetComponent<VFXParent>();
            if (vfxParent != null)
            {
                vfxParent.TriggerAnimation(false);
            }
        }

        //change target sprite animation + speed
        if (newStatusEffect.AnimatorOverride != BattleAnimatorTrigger.None)
        {
            if (newStatusEffect.AnimatorOverride == BattleAnimatorTrigger.Custom)
            {
                TriggerAnimation(newStatusEffect.CustomAnimatorOverride, true);
            }
            else
            {
                TriggerAnimation(newStatusEffect.AnimatorOverride.ToString(), true);
            }
        }

        //change target opacity
        if (newStatusEffect.SetTranparent)
        {
            StartCoroutine(ChangeOpacityCo(0.5f, true));
        }

        //if the target is stunned
        if (newStatusEffect.BoolsToModify.Contains(CombatantBool.CannotActOnTurn))
        {
            battleTimeline.ToggleStunState(this, combatantBattleData.CheckBool(CombatantBool.CannotActOnTurn));
        }
    }


    public IEnumerator ClearExpiredStatusEffectsCo(TurnEventType turnEventType)
    {
        for (int i = StatusEffectInstances.Count - 1; i >= 0; i--)
        {
            StatusEffectInstance statusEffectInstance = StatusEffectInstances[i];

            //remove?
            if (statusEffectInstance.StatusEffect.RemovalCheckTurnEventType == turnEventType && statusEffectInstance.StatusEffect.HasDuration && statusEffectInstance.Duration.CurrentValue <= 0)
            {
                RemoveStatusEffectInstance(statusEffectInstance, false);
                yield return wait05;
            }
        }
    }

    public void RemoveStatusEffect(StatusEffect statusEffectToRemove)
    {
        foreach (StatusEffectInstance statusEffectInstance in StatusEffectInstances)
        {
            if (statusEffectInstance.StatusEffect == statusEffectToRemove)
            {
                RemoveStatusEffectInstance(statusEffectInstance, true);
                break;
            }
        }
    }

    public void RemoveAllStatusEffects(StatusEffectType statusEffectType)
    {
        for (int i = StatusEffectInstances.Count - 1; i >= 0; i--)
        {
            StatusEffectInstance statusEffectInstance = StatusEffectInstances[i];
            if (StatusEffectInstances[i].StatusEffect.CanPurge && StatusEffectInstances[i].StatusEffect.StatusEffectType == statusEffectType)
            {
                RemoveStatusEffectInstance(statusEffectInstance, true);
            }
        }
    }

    protected virtual void RemoveStatusEffectInstance(StatusEffectInstance statusEffectInstance, bool wasRemovedExternally)
    {
        Debug.Log("triggering remove status effect instance");

        //remove from status list
        combatantBattleData.RemoveStatusInstance(statusEffectInstance);

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

        //reset opacity
        if (statusEffectInstance.StatusEffect.SetTranparent)
        {
            StartCoroutine(ChangeOpacityCo(1f, true));
        }

        //if the target was stunned
        if (statusEffectInstance.StatusEffect.BoolsToModify.Contains(CombatantBool.CannotActOnTurn))
        {
            battleTimeline.ToggleStunState(this, combatantBattleData.CheckBool(CombatantBool.CannotActOnTurn));
        }

        //remove vfx
        if (statusVFXs.ContainsKey(statusEffectInstance.StatusEffect))
        {
            for (int i = statusVFXs[statusEffectInstance.StatusEffect].Count - 1; i >= 0; i--)
            {
                GameObject effectToRemove = statusVFXs[statusEffectInstance.StatusEffect][i];
                Destroy(effectToRemove);
            }

            statusVFXs.Remove(statusEffectInstance.StatusEffect);
        }

        if (statusEffectInstance.StatusEffect.DisplayIcon)
        {
            healthDisplay.QueueRemoveStatusPopup(statusEffectInstance.StatusEffect, 0);
        }
    }

    //public void ModifyStatusDuration(StatusEffect statusEffectToModify, int amount)
    //{
    //    for (int i = StatusEffectInstances.Count - 1; i >= 0; i--)
    //    {
    //        StatusEffectInstance statusEffectInstance = StatusEffectInstances[i];
    //        if (statusEffectInstance.StatusEffect == statusEffectToModify)
    //        {
    //            if (amount > 0 && statusEffectInstance.StatusEffect.CanIncreaseDuration
    //                || amount < 0 && statusEffectInstance.StatusEffect.CanDecreaseDuration)
    //            {
    //                statusEffectInstance.ModifyDuration(amount);
    //            }
    //            if (statusEffectInstance.Duration.CurrentValue <= 0)
    //            {
    //                RemoveStatusEffectInstance(statusEffectInstance, true);
    //            }
    //            break;
    //        }
    //    }
    //}

    public void ModifyAllStatusDurations(StatusEffectType statusEffectType, int amount)
    {
        for (int i = StatusEffectInstances.Count - 1; i >= 0; i--)
        {
            StatusEffectInstance statusEffectInstance = StatusEffectInstances[i];
            if (StatusEffectInstances[i].StatusEffect.StatusEffectType == statusEffectType
                || amount > 0 && statusEffectInstance.StatusEffect.CanIncreaseDuration
                || amount < 0 && statusEffectInstance.StatusEffect.CanDecreaseDuration)
            {
                statusEffectInstance.ModifyDuration(amount);
            }
            if (statusEffectInstance.Duration.CurrentValue <= 0)
            {
                RemoveStatusEffectInstance(statusEffectInstance, true);
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
                if (statusEffectInstance.Stacks.CurrentValue <= 0)
                {
                    RemoveStatusEffectInstance(statusEffectInstance, true);
                }
                else
                {
                    //display popup icon
                    if (statusEffectInstance.StatusEffect.DisplayIcon)
                    {
                        healthDisplay.QueueRemoveStatusPopup(statusEffectInstance.StatusEffect, amount);
                    }
                }
                break;
            }
        }
    }

    public void IncreaseStatusPotency(StatusEffect statusEffectToModify, int change)
    {
        Debug.Log("Looking for status to update");

        for (int i = StatusEffectInstances.Count - 1; i >= 0; i--)
        {
            Debug.Log("status instance index: " + i);
            StatusEffectInstance statusEffectInstance = StatusEffectInstances[i];
            if (statusEffectInstance.StatusEffect == statusEffectToModify)
            {
                Debug.Log("Status To Update found");
                statusEffectInstance.ModifyPotency(change);
                break;
            }
        }
    }

    public bool CheckForStatus(StatusEffect statusEffect)
    {
        foreach (StatusEffectInstance statusEffectInstance in StatusEffectInstances)
        {
            if (statusEffectInstance.StatusEffect == statusEffect)
            {
                return true;
            }
        }
        return false;
    }

    public StatusEffectInstance GetStatusEffectInstance(StatusEffect statusEffect)
    {
        foreach (StatusEffectInstance statusEffectInstance in StatusEffectInstances)
        {
            if (statusEffectInstance.StatusEffect == statusEffect)
            {
                return statusEffectInstance;
            }
        }
        return null;
    }

    public int GetStatusCount(StatusEffectType statusEffectType, bool onlyGetRemoveable)
    {
        int statusCount = 0;
        foreach (StatusEffectInstance statusEffectInstance in StatusEffectInstances)
        {
            if (statusEffectInstance.StatusEffect.StatusEffectType == statusEffectType)
            {
                if (!onlyGetRemoveable || onlyGetRemoveable && statusEffectInstance.StatusEffect.CanPurge) 
                {
                    statusCount++;
                }
            }
        }
        return statusCount;
    }

    protected void TriggerStatusTurnEvents(StatusEffectInstance statusEffectInstance)
    {
        //add turn effects to queue
        foreach (BattleEventTrigger thisEventTrigger in statusEffectInstance.StatusEffect.TurnEventTriggers)
        {
            //skip event if roll fails
            if (thisEventTrigger == null || !thisEventTrigger.TriggerCheck())
            {
                continue;
            }

            //apply to self, or different targets?
            List<Combatant> targets = new List<Combatant>();
            if (thisEventTrigger.UseTargetOverride)
            {
                targets = battleManager.GetAltTargets(this, thisEventTrigger.TargetOverride, thisEventTrigger.PickRandomTarget);
            }
            else
            {
                targets.Add(this);
            }

            //if there are any valid targets, create battle event and add it to queue
            if (targets.Count > 0)
            {
                List<BattleEventTarget> battleEventTargets = new List<BattleEventTarget>();
                foreach (Combatant target in targets)
                {
                    battleEventTargets.Add(new BattleEventTarget(target, statusEffectInstance.Potency));
                }

                BattleEvent battleEvent = new BattleEvent(thisEventTrigger.BattleEventType, this, battleEventTargets, thisEventTrigger.TriggerableEffectContainers, thisEventTrigger.EventName, thisEventTrigger.ActionAnimations, thisEventTrigger.TriggerableEffectType);
                onAddBattleEvent.Raise(battleEvent);
            }
        }
    }


    protected virtual void SetOverrideAttack(Attack attack)
    {
    }

    protected virtual void ClearOverrideAttack()
    {
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

    public virtual void ToggleHighlight(bool isHighlighted)
    {
        targetedGlow.SetActive(isHighlighted);
    }
    #endregion
}
