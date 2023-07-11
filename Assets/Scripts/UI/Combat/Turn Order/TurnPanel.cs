using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class TurnPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI index;
    [SerializeField] private TextMeshProUGUI counter;
    [SerializeField] private SortingGroup sortingGroup;
    [Header("Actor Panel")]
    public GameObject actorPanel;
    [SerializeField] private Image actorPortrait;
    [SerializeField] private OutlinedText actorLetter;
    [SerializeField] private GameObject actorHighlight;
    [SerializeField] private GameObject interventionBorder;
    [SerializeField] private GameObject playerBorderActor;
    [SerializeField] private GameObject enemyBorderActor;
    [Header("Action/Target Panel")]
    private bool actionPreviewActive = false;
    [SerializeField] private Image actionIcon;
    [SerializeField] private GameObject targetPanel;
    [SerializeField] private Image targetPortrait;
    [SerializeField] private OutlinedText targetLetter;
    [SerializeField] private GameObject targetHighlight;
    [SerializeField] private GameObject playerBorderTarget;
    [SerializeField] private GameObject enemyBorderTarget;
    [Header("Animations")]
    [SerializeField] private Animator animatorActorMovement;
    [SerializeField] private Animator animatorTargetMovement;
    [Header("Misc. Images")]
    [SerializeField] private GameObject turnPosUp; 
    [SerializeField] private GameObject turnPosDown;
    [SerializeField] private Sprite interventionIcon;
    [SerializeField] private Sprite playerIcon;
    [SerializeField] private Sprite playerPartyIcon;
    [SerializeField] private Sprite enemyIcon;
    [SerializeField] private Sprite attackIcon;
    [SerializeField] private Sprite healIcon;
    [SerializeField] private Sprite buffIcon;
    [SerializeField] private Sprite debuffIcon;
    [SerializeField] private Sprite otherIcon;
    private Dictionary<ActionType, Sprite> actionIcons = new Dictionary<ActionType, Sprite>();
    private WaitForSeconds wait02= new WaitForSeconds(0.2f);

    //public Turn Turn { get; private set; }
    public bool IsNew { get; private set; } = true;

    private void OnEnable()
    {
        actionIcons = new Dictionary<ActionType, Sprite>()
        {
            { ActionType.Attack, attackIcon},
            { ActionType.Heal, healIcon},
            { ActionType.ApplyBuff, buffIcon},
            { ActionType.RemoveDebuff, buffIcon},
            { ActionType.ApplyDebuff, debuffIcon},
            { ActionType.RemoveBuff, debuffIcon},
            { ActionType.Other, otherIcon}
        };
    }

    public void RemoveNewStatus()
    {
        IsNew = false;
    }

    public void UpdateCounter(int listIndex, int storedIndex, int counterValue)
    {
        index.text = listIndex.ToString() + "/" + storedIndex.ToString();
        counter.text = counterValue.ToString();
    }

    public void TriggerActorAnimation(string animatorTrigger)
    {
        animatorActorMovement.SetTrigger(animatorTrigger);
        if(actionPreviewActive) 
        {
            if (animatorTrigger == "Exit" || animatorTrigger == "Enter")
            {
                TriggerTargetAnimation(animatorTrigger);
            }
            else if (animatorTrigger == "Kill")
            {
                TriggerTargetAnimation("Exit");
            }
        }
    }

    public void TriggerTargetAnimation(string animatorTrigger)
    {
        animatorTargetMovement.SetTrigger(animatorTrigger);
    }

    public void SetActor(Combatant actor, bool isIntervention)
    {
        actorLetter.SetText(actor.CharacterLetter);
        actorPortrait.sprite = actor.TurnIcon;

        if (isIntervention)
        {
            interventionBorder.SetActive(true);
        }
        else if (actor.CombatantType == CombatantType.Player)
        {
            playerBorderActor.SetActive(true);
        }
        else if (actor.CombatantType == CombatantType.Enemy)
        {
            enemyBorderActor.SetActive(true);
        }
    }

    public IEnumerator DisplayActionPreviewCo(Action action, List<Combatant> targets, bool targetUnknown)
    {
        if (!targetPanel.activeInHierarchy)
        {
            targetPanel.SetActive(true);
        }
        //targetHighlight.SetActive(true);

        if (actionPreviewActive)
        {
            animatorTargetMovement.SetTrigger("Swap");
            yield return wait02;
        }
        else
        {
            animatorTargetMovement.SetTrigger("Enter");
            actionPreviewActive = true;
        }

        //set action icon
        actionIcon.sprite = actionIcons[action.ActionType];

        //set targets icon
        if(action.AOEType == AOEType.All)
        {
            if (targets[0].CombatantType == CombatantType.Player)
            {
                targetPortrait.sprite = playerPartyIcon;
            }
            else
            {
                targetPortrait.sprite = enemyIcon;
            }
            targetLetter.SetText("all");
        }
        else if (targetUnknown)
        {
            if (targets[0].CombatantType == CombatantType.Player)
            {
                targetPortrait.sprite = playerIcon;
            }
            else
            {
                targetPortrait.sprite = enemyIcon;
            }
            targetLetter.SetText("?");
        }
        else
        {
            targetPortrait.sprite = targets[0].TurnIcon;
            targetLetter.SetText(targets[0].CharacterLetter);
        }
        if (targets[0].CombatantType == CombatantType.Player)
        {
            playerBorderTarget.SetActive(true);
        }
        else if (targets[0].CombatantType == CombatantType.Enemy)
        {
            enemyBorderTarget.SetActive(true);
        }

        yield return null;
    }

    public void HideActionPreview()
    {
        if(actionPreviewActive)
        {
            //targetHighlight.SetActive(false);
            actionPreviewActive = false;
            animatorTargetMovement.SetTrigger("Exit");
        }
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


    public void Highlight(bool isTargeted)
    {
        actorHighlight.SetActive(isTargeted);
    }
}
