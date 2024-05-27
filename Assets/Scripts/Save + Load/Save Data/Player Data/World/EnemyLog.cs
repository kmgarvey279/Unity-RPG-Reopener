using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyLogEntry
{
    [field: SerializeField] public EnemyInfo EnemyInfo { get; private set; }
    [field: SerializeField] public List<ElementalProperty> RevealedElements { get; private set; } = new List<ElementalProperty>();
    [field: SerializeField] public int KillCount { get; private set; } = 0;

    public EnemyLogEntry(EnemyInfo enemyInfo)
    {
        EnemyInfo = enemyInfo;
    }

    public void AddRevealedElement(ElementalProperty elementalProperty)
    {
        if (!RevealedElements.Contains(elementalProperty))
        {
            RevealedElements.Add(elementalProperty);
        }
    }
}


[System.Serializable]
public class EnemyLog
{
    public Dictionary<EnemyInfo, EnemyLogEntry> EnemyEntries = new Dictionary<EnemyInfo, EnemyLogEntry>();

    public void AddEnemy(EnemyInfo newEnemyInfo)
    {
        if (EnemyEntries.ContainsKey(newEnemyInfo))
        {
            return;
        }

        EnemyEntries.Add(newEnemyInfo, new EnemyLogEntry(newEnemyInfo));
    }

    public void AddRevealedElement(EnemyInfo enemyInfo, ElementalProperty elementalProperty)
    {
        if (!EnemyEntries.ContainsKey(enemyInfo))
        {
            return;
        }

        EnemyEntries[enemyInfo].AddRevealedElement(elementalProperty);
    }
    //public void SetData(EnemyLog enemyLog)
    //{
    //    EnemyEntries = enemyLog.EnemyDict;
    //}

    //public SerializableEnemyData()
    //{
    //    enemyEntries = new Dictionary<EnemyInfo, EnemyLogEntry>();
    //}
}
