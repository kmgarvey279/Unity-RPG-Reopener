using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "New Enemy Database", menuName = "Database/Enemy")]
public class EnemyDatabase : ScriptableObject
{
    public Dictionary<string, EnemyInfo> LookupDictionary { get; private set; }
    public List<EnemyInfo> enemies;

    public void OnEnable()
    {
        PopulateList();
    }

    void PopulateList()
    {
        enemies = new List<EnemyInfo>();
        LookupDictionary = new Dictionary<string, EnemyInfo>();

        string[] guids = AssetDatabase.FindAssets("t:EnemyInfo", new string[] { "Assets/Scriptable Objects/Battle/Combatants/Enemies/Info" });
        foreach (string guid in guids)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(guid);
            EnemyInfo enemyInfo = AssetDatabase.LoadAssetAtPath<EnemyInfo>(SOpath);
            enemies.Add(enemyInfo);

            if (!LookupDictionary.ContainsKey(enemyInfo.EnemyID))
            {
                LookupDictionary.Add(enemyInfo.EnemyID, enemyInfo);
                //Debug.Log("Adding item entry " + item.Guid + " : " + item.ItemName);
            }
        }
    }
}
