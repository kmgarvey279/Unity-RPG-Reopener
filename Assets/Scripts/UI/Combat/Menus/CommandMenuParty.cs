using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandMenuParty : CommandMenu
{
    private bool canSwap = false;
    [SerializeField] private CommandMenuMain commandMenuMain;

    protected void Update()
    {
        if (display.activeInHierarchy)
        {
            if (Input.GetButtonDown("Submit"))
            {
                battleManager.SwapPlayableCombatants((PlayableCombatant)battleTimeline.CurrentTurn.Actor);
            }
            else if (Input.GetButton("Cancel"))
            {
                display.SetActive(false);
                commandMenuMain.Display();
            }
        }
    }

    public override void Display()
    {
        base.Display();
    }

    public override void Hide()
    {
        base.Hide();
    }
}
