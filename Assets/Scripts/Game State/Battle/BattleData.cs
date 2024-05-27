using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleDataBool
{
    CannotEscape,
    CannotTriggerIntervention
}

public class BattleData
{
    public Dictionary<BattleDataBool, int> battleDataBools;
    //public CommandMenuStateType LastCommandMenuStateType { get; private set; }

    public BattleData()
    {
        //bools, 0 == false, > 0 == true 
        battleDataBools = new Dictionary<BattleDataBool, int>();
        foreach (BattleDataBool battleDataBool in System.Enum.GetValues(typeof(BattleDataBool)))
        {
            battleDataBools.Add(battleDataBool, 0);
        }
    }

    //public void SetLastCommandMenuStateType(CommandMenuStateType lastCommandMenuStateType)
    //{
    //    LastCommandMenuStateType = lastCommandMenuStateType;
    //}

    public bool CheckBool(BattleDataBool battleDataBool)
    {
        if (battleDataBools[battleDataBool] == 0)
        {
            return false;
        }
        return true;
    }

    public virtual void ModifyBool(BattleDataBool battleDataBool, bool isTrue)
    {
        if (isTrue)
        {
            battleDataBools[battleDataBool] += 1;
        }
        else
        {
            battleDataBools[battleDataBool] -= 1;
            if (battleDataBools[battleDataBool] < 0)
            {
                battleDataBools[battleDataBool] = 0;
            }
        }
    }

    public void ResetBool(BattleDataBool battleDataBool)
    {
        battleDataBools[battleDataBool] = 0;
    }
}
