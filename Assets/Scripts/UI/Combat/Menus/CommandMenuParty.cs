using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMenuParty : CommandMenu
{
    [SerializeField] protected CommandMenuMain commandMenuMain;
    protected void Update()
    {
        if (display.activeInHierarchy)
        {
            if (Input.GetButton("Cancel"))
            {
                display.SetActive(false);
                commandMenuMain.Display();
            }
        }
    }
}
