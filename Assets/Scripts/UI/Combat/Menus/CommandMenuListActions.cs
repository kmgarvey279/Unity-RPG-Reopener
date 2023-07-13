using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMenuListActions : CommandMenuList
{
    [Header("List")]
    [SerializeField] private DynamicListActions dynamicListActions;
    protected void Update()
    {
        if (display.activeInHierarchy)
        {
            if (Input.GetButton("Cancel"))
            {
                display.SetActive(false);
                dynamicListActions.ClearList();
                commandMenuMain.Display();
            }
        }
    }
    public override void DisplayList()
    {
        PlayableCombatant actor = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;
        if (actor != null)
        {
            dynamicListActions.CreateList(actor.Skills);
        }
        display.SetActive(true);
    }

    public override void OnSelectSlot(GameObject slotObject)
    {
        base.OnSelectSlot(slotObject);

        DynamicListSlotAction slotAction = slotObject.GetComponent<DynamicListSlotAction>();
        if (slotAction != null)
        {
            Combatant actor = battleTimeline.CurrentTurn.Actor;

            if (slotAction.Action.ActionCostType == ActionCostType.HP && slotAction.Action.Cost > actor.HP.Value
                || slotAction.Action.ActionCostType == ActionCostType.MP && slotAction.Action.Cost > actor.MP.Value)
            {
                //GetComponent<Button>().interactable = false;
                Debug.Log("Can't use that skill!");
                return;
            }
            display.SetActive(false);
            dynamicListActions.ClearList();

            battleTimeline.UpdateTurnAction(battleTimeline.CurrentTurn, slotAction.Action);
            onChangeBattleState.Raise((int)BattleStateType.TargetSelect);
        }
    }
}
