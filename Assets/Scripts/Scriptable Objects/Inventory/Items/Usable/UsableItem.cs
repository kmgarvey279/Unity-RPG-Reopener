using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Usable Item", menuName = "Inventory/Items/Usable Item")]
public class UsableItem : Item
{
    public bool UseInOverworld { get; private set; } = false;
    public Action UseAction { get; private set; }

    public virtual void OnEnable()
    {
        ItemType = ItemType.Usable;
    }

    public void UseOverworld(List<Combatant> targets)
    {
        foreach(Combatant target in targets)
        {
            //use
        }
    }
}
