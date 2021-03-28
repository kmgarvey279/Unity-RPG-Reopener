using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Master Item List", menuName = "Inventory/Master Item List")]
public class ItemMasterList : ScriptableObject
{
    public List<ItemObject> masterList;
    public Dictionary<int, ItemObject> masterDict = new Dictionary<int, ItemObject>();

    private void OnEnable()
    {
        for (int i = 0; i < masterList.Count; i++)
        {
            masterDict.Add(i, masterList[i]);
        }
    }
}
