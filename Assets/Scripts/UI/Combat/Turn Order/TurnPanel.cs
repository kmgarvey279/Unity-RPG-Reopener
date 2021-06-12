using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnPanel : MonoBehaviour
{
    private TurnSlot turnSlot;
    [SerializeField] private TextMeshProUGUI nameText;

    public void AssignTurnSlot(TurnSlot newSlot)
    {
        turnSlot = newSlot;
        nameText.text = turnSlot.combatant.characterInfo.name + " " + turnSlot.turnCounter.ToString();
    }
}
