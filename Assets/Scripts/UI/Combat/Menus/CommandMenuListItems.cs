using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMenuListItems : CommandMenuList
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private ActionUseItem useActionTemplate;
    [Header("List")]
    [SerializeField] private DynamicListItems dynamicListItems;
    protected void Update()
    {
        if (display.activeInHierarchy)
        {
            if (Input.GetButton("Cancel"))
            {
                display.SetActive(false);
                dynamicListItems.ClearList();
                commandMenuMain.Display();
            }
        }
    }
    public override void DisplayList()
    {
        dynamicListItems.CreateList(inventory.ItemDict[ItemType.Usable]);
        display.SetActive(true);
    }

    public override void OnSelectSlot(GameObject slotObject)
    {
        Debug.Log("select item slot");
        base.OnSelectSlot(slotObject);

        DynamicListSlotItem slotItem = slotObject.GetComponent<DynamicListSlotItem>();
        if (slotItem != null)
        {
            UsableItem usableItem = (UsableItem)slotItem.InventoryItem.Item;
            if (usableItem != null)
            {
                ActionUseItem actionUseItem = usableItem.CreateAction(useActionTemplate);

                display.SetActive(false);
                dynamicListItems.ClearList();

                battleTimeline.UpdateTurnAction(battleTimeline.CurrentTurn, actionUseItem);
                onChangeBattleState.Raise((int)BattleStateType.TargetSelect);
            }
        }
    }
}
