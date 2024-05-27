using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Database/Inventory")]
public class ItemDatabase : ScriptableObject
{
    public Dictionary<string, Item> LookupDictionary { get; private set; }
    public List<Item> items;

    public void OnEnable()
    {
        PopulateList();
    }

    void PopulateList()
    {
        items = new List<Item>();
        LookupDictionary = new Dictionary<string, Item>();

        string[] guids = AssetDatabase.FindAssets("t:Item", new string[]{ "Assets/Scriptable Objects/Inventory/Items" });
        foreach (string guid in guids) 
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(guid);
            Item item = AssetDatabase.LoadAssetAtPath<Item>(SOpath);
            items.Add(item);

            if (!LookupDictionary.ContainsKey(item.ItemID))
            {
                LookupDictionary.Add(item.ItemID, item);
                //Debug.Log("Adding item entry " + item.Guid + " : " + item.ItemName);
            }
        }
    }
}

