using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class BattleStartState : BattleState
{
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Entering Battle Start State");

        StartCoroutine(OnStartCo());
    }

    public override void StateUpdate()
    {

    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private IEnumerator OnStartCo()
    {
        List<Combatant> actors = battleManager.GetCombatants(CombatantType.All);
        foreach (Combatant actor in actors)
        {
            StartCoroutine(actor.OnBattleStart());
        }
        bool allActorsReady = false;
        int loops = 0;
        while (!allActorsReady)
        {
            bool oneNotReady = false;
            foreach (Combatant actor in actors)
            {
                if (!actor.OnBattleStartIsComplete)
                {
                    oneNotReady = true;
                    continue;
                }
            }
            if (!oneNotReady)
            {
                allActorsReady = true;
            }
            yield return null;

            loops++;
            if (loops > 1000)
            {
                Debug.Log("error: on battle start failed");
                break;
            }
        }

        stateMachine.ChangeState((int)BattleStateType.ChangeTurn);
    }
}

