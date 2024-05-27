using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VersionControl.Git;
using UnityEngine;

public class EnemyCombatant : Combatant
{
    [field: SerializeField] public EnemyAI EnemyAI { get; protected set; }
    [SerializeField] private VulnerabilityPreview vulnerabilityPreview;
    [SerializeField] private StatusEffect openStatus;

    protected override void Awake()
    {
        base.Awake();
        CombatantType = CombatantType.Enemy;
    }

    #region Data Get/Set
    private EnemyCombatantBattleData enemyCombatantBattleData
    {
        get
        {
            return (EnemyCombatantBattleData)combatantBattleData;
        }
    }

    public EnemyInfo EnemyInfo 
    { 
        get
        {
            return enemyCombatantBattleData.EnemyInfo;
        }
    }
    public bool IsBoss
    {
        get
        {
            return enemyCombatantBattleData.IsBoss;
        }
    }
    public int Guard
    {
        get
        {
            return enemyCombatantBattleData.Guard.CurrentValue;
        }
        private set
        {
            enemyCombatantBattleData.Guard.UpdateValue(value);
        }
    }
    public int MaxGuard
    {
        get
        {
            return enemyCombatantBattleData.Guard.CurrentValue;
        }
    }
    public List<ElementalProperty> Vulnerabilities
    {
        get
        {
            return enemyCombatantBattleData.Vulnerabilities;
        }
    }
    #endregion

    public void SetCharacterData(EnemyInfo enemyInfo)
    {
        combatantBattleData = new EnemyCombatantBattleData(enemyInfo);
        EnemyAI = new EnemyAI(enemyInfo);

        healthDisplay.SetValues();
        healthDisplay.SetGuardValues();

        IsLoaded = true;
    }

    public override void HideHealthInfo()
    {
        base.HideHealthInfo();

        HideVulnerability();
    }

    #region Guard
    public void ApplyBreak(int amount)
    {
        ModifyGuard(-amount);
    }

    public void RefreshGuard()
    {
        if (!CheckForStatus(openStatus) && Guard <= 0)
        {
            ModifyGuard(MaxGuard);
        }
    }

    public void ModifyGuard(int amount)
    {
        Guard += amount;
        healthDisplay.DisplayChanges();
    }
    #endregion

    #region Vulnerability
    public void DisplayVulnerability(bool isRevealed, ElementalProperty elementalProperty)
    {
        bool isVulnerable = false;
        if (Vulnerabilities.Contains(elementalProperty))
        {
            isVulnerable = true;
        }
        vulnerabilityPreview.Display(isRevealed, isVulnerable);
    }

    public void HideVulnerability()
    {
        vulnerabilityPreview.Hide();
    }

    public float ApplyVulnerabiltyMultiplier(float baseDamage, bool didHitWeakness)
    {
        float vulnerabilityMultiplier = 1f;
        if (CheckForStatus(openStatus))
        {
            vulnerabilityMultiplier = BattleConsts.OpenMultiplierConst;
        }
        else if (didHitWeakness)
        { 
            vulnerabilityMultiplier = BattleConsts.VulnerableMultiplierConst;
        }
        return baseDamage * vulnerabilityMultiplier;
    }
    #endregion

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        RefreshGuard();
    }

    public override void OnKO()
    {
        base.OnKO();
        Tile.UnassignOccupier();
        StartCoroutine(DestroyEnemyCo());
    }

    public override void OnAttacked(int amount, bool isCrit, bool isVulnerable)
    {
        TriggerAnimation("Hit", false);
        OnDamaged(amount, isCrit, isVulnerable);
    }

    public override void OnRevive(float percentOfHPToRestore)
    {
        base.OnRevive(percentOfHPToRestore);
        Tile.AssignOccupier(this);
    }

    private IEnumerator DestroyEnemyCo()
    {
        yield return wait1;
        gameObject.SetActive(false);
    }
}
