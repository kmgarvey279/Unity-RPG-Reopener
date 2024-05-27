using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class TurnPanel : MonoBehaviour
{
    [field: SerializeField] public Combatant Actor { get; private set; }

    //debug
    [SerializeField] private TextMeshProUGUI counter;
    [Header("Panel")]
    [SerializeField] private Image actorPortrait;
    [SerializeField] private OutlinedText actorLetter;
    [SerializeField] private GameObject actorHighlight;
    [Header("Border")]
    [SerializeField] private Image border;
    [SerializeField] private Color playerColor;
    [SerializeField] private Color enemyColor;
    [SerializeField] private Color interventionColor;
    [field: Header("Cursor")]
    [SerializeField] private GameObject selectedCursor;
    [Header("Animations")]
    [SerializeField] private Animator animatorActorMovement;
    [Header("Misc.")]
    [SerializeField] private GameObject turnPosUp; 
    [SerializeField] private GameObject turnPosDown;
    [SerializeField] private GameObject koFilter;
    [SerializeField] private GameObject stunFilter;

    private Dictionary<ActionType, Sprite> actionIcons = new Dictionary<ActionType, Sprite>();
    private WaitForSeconds wait02= new WaitForSeconds(0.2f);
    private WaitForSeconds wait04 = new WaitForSeconds(0.4f);
    private bool isSelected;

    //public Turn Turn { get; private set; }
    public bool IsNew { get; private set; } = true;

    public void RemoveNewStatus()
    {
        IsNew = false;
    }

    public void UpdateCounter(int listIndex, int storedIndex, int counterValue)
    {
        //index.text = listIndex.ToString() + "/" + storedIndex.ToString();
        counter.text = counterValue.ToString();
    }

    public void TriggerActorAnimation(string animatorTrigger)
    {
        animatorActorMovement.SetTrigger(animatorTrigger);
    }

    public void SetActor(Combatant actor, bool isIntervention)
    {
        Actor = actor;
        actorLetter.SetText(actor.CharacterLetter);
        actorPortrait.sprite = actor.TurnIcon;

        if (isIntervention)
        {
            border.color = interventionColor;
        }
        else if (actor.CombatantType == CombatantType.Player)
        {
            border.color = playerColor;
        }
        else if (actor.CombatantType == CombatantType.Enemy)
        {
            border.color = enemyColor;
        }
    }

    public void SetAsCurrentTurn()
    {
        //currentTurnBorder.SetActive(true);
    }

    public void DisplayTurnModifier(int change)
    {
        if(change > 0)
        {
            turnPosDown.SetActive(true);
        }
        else if (change < 0)
        {
            turnPosUp.SetActive(true);
        }
    }
    public void ClearTurnPosChange()
    {
        if (turnPosDown.activeInHierarchy)
        {
            turnPosDown.SetActive(false);
        }
        if (turnPosUp.activeInHierarchy)
        {
            turnPosUp.SetActive(false);
        }
    }


    public void Highlight()
    {
        actorHighlight.SetActive(true);
        Glow glow = actorHighlight.GetComponent<Glow>();
        if (glow)
        {
            glow.Refresh();
        }
    }

    public void Unhighlight()
    {
        actorHighlight.SetActive(false);
    }

    public void DisplayCursor()
    {
        isSelected = true;
        selectedCursor.SetActive(true);
    }

    public void HideCursor() 
    {
        isSelected = false;
        selectedCursor.SetActive(false);
    }

    public void CursorCheck(bool isOnscreen)
    {
        if (isSelected && isOnscreen)
        {
            selectedCursor.SetActive(true);
        }
        else
        {
            selectedCursor.SetActive(false);
        }
    }

    public void ToggleKOFilter(bool isActive)
    {
        koFilter.SetActive(isActive);
    }

    public void ToggleStunFilter(bool isStunned)
    {
        stunFilter.SetActive(isStunned);
    }
}
