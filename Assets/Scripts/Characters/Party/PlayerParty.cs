using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using StateMachineNamespace;

public class PlayerParty : MonoBehaviour
{
    public List<GameObject> activeParty;
    public List<GameObject> koParty;
    public List<GameObject> reserveParty;

    public List<GameObject> targets;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            activeParty.Add(child.gameObject);
        }
    }

    public void StartBattle()
    {
        foreach (GameObject ally in activeParty)
        {   
            ally.GetComponentInChildren<StateMachine>().ChangeState("BattleState");   
        }
        //combat start signal
    }

    public void EndBattle()
    {
        foreach (GameObject ally in activeParty)
        {   
            ally.GetComponentInChildren<StateMachine>().ChangeState("IdleState");   
        }
        //combat end signal
    }

    public void AddTargetToAll(GameObject target)
    {
        if(targets.Count <= 0)
        {
            StartBattle();
        }
        targets.Add(target);
        foreach (GameObject ally in activeParty)
        {   
            ally.GetComponent<Targeter>().AddTarget(target);   
        }
    }

    public void RemoveTargetFromAll(GameObject target)
    {
        targets.Remove(target);
        foreach (GameObject ally in activeParty)
        {   
            ally.GetComponent<Targeter>().RemoveTarget(target);   
        }
        if(targets.Count <= 0)
        {
            EndBattle();
        }
    }

    public void AllyKO(GameObject ally)
    {
        activeParty.Remove(ally);
        koParty.Add(ally);

        // OnTargetAdd.Raise(ally);
    }

    public void AllyRevive(GameObject ally)
    {
        koParty.Remove(ally);
        activeParty.Add(ally);

        // OnTargetRemove.Raise(ally);
    }

    public void AllySwap(GameObject ally1, GameObject ally2)
    {
        activeParty.Remove(ally1);
        reserveParty.Add(ally1);

        reserveParty.Remove(ally2);
        activeParty.Add(ally2);

        //change party member signal
    }


}
