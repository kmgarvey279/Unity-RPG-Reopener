using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    private enum TurnState
    {
        acting, 
        waiting
    } 
    private TurnState turnState;
    [Header("Lists of Potential Actions")]
    [SerializeField] private List<Action> actions = new List<Action>(); 
    [SerializeField] private List<Action> specialActions = new List<Action>(); 
    [Header("Queued Actions")]
    private Action previousAction;
    public Action currentAction;
    [Header("Action Frequency Timers")]
    [SerializeField] private float turnGaugeMax;
    private float turnGaugeCurrent;
    [SerializeField] private float specialGaugeMax;
    private float specialGaugeCurrent;

    public string targetTag;

    // Start is called before the first frame update
    public virtual void Awake()
    {
        turnState = TurnState.waiting; 
        previousAction = null;
        turnGaugeCurrent = 0;
        specialGaugeCurrent = 0;
    }

    public virtual void Update()
    {
        //increase special gauge, regardless of state
        if(specialGaugeCurrent < specialGaugeMax)
        {
            specialGaugeCurrent += Time.deltaTime;
        }
        if(turnState == TurnState.waiting)
        {
            //increase turn gauge only when in waiting state
            if(turnGaugeCurrent < turnGaugeMax)
            {
                turnGaugeCurrent += Time.deltaTime;
            }
            //if it is now the npc's "turn"
            if(turnGaugeCurrent >= turnGaugeMax)
            {
                turnState = TurnState.acting;
                turnGaugeCurrent = 0;
                //use special if ready
                if(specialGaugeCurrent >= specialGaugeMax)
                {
                    SetSpecialAction();
                }
                //otherwise use normal Action
                else
                {
                    SetNormalAction();
                }
            } 
        }
    }

    public virtual void SetNormalAction()
    {
        if(actions.Count > 0)
        {
            //check if target is in range of any action on list (list # based on priority)
            bool repeatAction = false; 
            foreach (Action action in actions)
            {
                if(action.CheckConditions())
                {
                    if(previousAction != null && action == previousAction)
                    {
                        repeatAction = true;
                    } 
                    else
                    {
                        currentAction = action;
                        return; 
                    }
                }
            }
            if(repeatAction)
            {
                currentAction = previousAction;
                return;
            }
        }
        Debug.Log("Nothing to do!");
    }

    public virtual void SetSpecialAction()
    {
        if(specialActions.Count > 0)
        {
            //check if target is in range of any special on special list (list # based on priority)
            foreach(Action special in specialActions)
            {
                //if there is a special Action to use
                if(special.CheckConditions())
                {
                    specialGaugeCurrent = 0;
                    currentAction = special;
                    return;
                }    
            }
        }
        SetNormalAction();
    }

    public void FinishAction()
    {
        turnState = TurnState.waiting;
        previousAction = currentAction;
        currentAction = null;
    }
}
