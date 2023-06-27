using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Usable Item", menuName = "Inventory/Items/Usable Item")]
public class UsableItem : Item
{
    public bool UseInOverworld { get; private set; } = false;
    //add overworld/menu effects here
    public List<TriggerableBattleEffect> TriggerableBattleEffects { get; private set; } = new List<TriggerableBattleEffect>();
    public TargetingType TargetingType { get; private set; }
    public AOEType AOEType { get; private set; }
    public List<ActionAnimationData> ActionAnimations { get; private set; } = new List<ActionAnimationData>();

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

    public ActionUseItem CreateAction(ActionUseItem useActionTemplate)
    {
        ActionUseItem actionUseItem = ScriptableObject.CreateInstance("ActionUseItem") as ActionUseItem;
        actionUseItem.Init(this);
        return actionUseItem;
    }
}
