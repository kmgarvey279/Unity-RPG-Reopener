using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class GeneralConsts
{
    public int LevelCap { get; private set; } = 99;
    public int[] EXPRequirements { get; private set; }
    public int StatCap { get; private set; } = 99;

    public const int baseEXPRequirement = 20;
    public const float perLevelEXPMultiplier = 1.5f;


    public GeneralConsts()
    {
        EXPRequirements = new int[LevelCap + 1];
        int lastValue = baseEXPRequirement;

        EXPRequirements[0] = 0;
        EXPRequirements[EXPRequirements.Length - 1] = 0;
        for (int i = 1; i < EXPRequirements.Length - 1; i++)
        {
            lastValue = Mathf.RoundToInt(lastValue * 1.1f);
            EXPRequirements[i] = lastValue;
        }
    }
}

public class DatabaseDirectory : MonoBehaviour
{
    public static DatabaseDirectory Instance;

    [field: SerializeField] public ItemDatabase ItemDatabase { get; private set; }
    [field: SerializeField] public PlayableCombatantDatabase PlayableCombatantDatabase { get; private set; }
    [field: SerializeField] public EnemyDatabase EnemyDatabase { get; private set; }
    [field: SerializeField] public GeneralConsts GeneralConsts { get; private set; }

    public void Awake()
    {
        GeneralConsts = new GeneralConsts();
        if (Instance == null)
        {
            Instance = this;
        }
    }
}

