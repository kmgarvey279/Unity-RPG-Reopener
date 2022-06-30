using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//stores information related to the current turn
[System.Serializable]
public class TurnData
{
    public Combatant combatant;
    [Header("Selected Action")]
    public ActionEvent actionEvent;
    public bool hasMoved = false;

    public TurnData(Combatant combatant)
    {
        this.combatant = combatant;
    }
}