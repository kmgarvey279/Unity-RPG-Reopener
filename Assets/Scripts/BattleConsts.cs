using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleConsts
{
    //damage/healing
    public const float AttackMultiplierConst = 8f;
    public const float HealingMultiplierConst = 6f;
    public const float DefenseMultiplierConst = 4f;
    public const float DefenseApplicationConst = 100f;

    public const float BaseCritMultiplierConst = 1.5f;

    public const float VulnerableMultiplierConst = 1.25f;

    public const float OpenMultiplierConst = 1.75f;

    public const float VarianceMinDamage = 0.95f;
    public const float VarianceMaxDamage = 1.05f;
    public const float VarianceMinHeal = 0.97f;
    public const float VarianceMaxHeal = 1.03f;

    //guard/break
    public const float VulnerableBreakMultiplierConst = 1.5f;

    //mp
    public const float BaseMPPercentRegen = 0.2f;
    public const float InterventionMPPercentRegen = 0.3f;
    
    //interventions
    public const int BaseInterventionPointsGeneration = 10;
    public const int DefendInterventionPointsGeneration = 20;
}
