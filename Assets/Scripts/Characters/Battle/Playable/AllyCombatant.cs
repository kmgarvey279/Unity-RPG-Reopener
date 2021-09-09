using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyCombatant : Combatant
{
    private BattlePartyPanel battlePartyPanel;

    public void AssignBattlePartyPanel(BattlePartyPanel battlePartyPanel)
    {
        this.battlePartyPanel = battlePartyPanel;
    }
}
