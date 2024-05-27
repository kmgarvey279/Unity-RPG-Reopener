using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuPartySlot : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private GameObject selectOutline;

    [SerializeField] private PauseMenuParty pauseMenuParty;
    [SerializeField] private PauseMenuStatDisplay pauseMenuStatDisplay;

    [SerializeField] private GameObject content;
    [SerializeField] private Image characterSprite;
    [SerializeField] private TextMeshProUGUI nameValue;
    [SerializeField] private TextMeshProUGUI levelValue;
    [SerializeField] private TextMeshProUGUI hpValue;
    [SerializeField] private TextMeshProUGUI maxHPValue;
    [SerializeField] private TextMeshProUGUI mpValue;
    [SerializeField] private TextMeshProUGUI maxMPValue;
    public PlayableCombatantRuntimeData PlayableCombatantRuntimeData { get; private set; }
    private Animator animator;

    private void Awake()
    {
        pauseMenuParty = GetComponentInParent<PauseMenuParty>();   
        animator = GetComponent<Animator>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (!selectOutline.activeInHierarchy)
        {
            selectOutline.SetActive(true);
        }

        if (PlayableCombatantRuntimeData != null)
        {
            pauseMenuStatDisplay.DisplayStats(PlayableCombatantRuntimeData);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (selectOutline.activeInHierarchy)
        {
            selectOutline.SetActive(false);
        }
    }

    public void OnClick()
    {
        //pauseMenuParty.OnClickPartySlot(this);
        PlayableCombatantRuntimeData.GainEXP(100);
    }

    public void AssignCharacter(PlayableCharacterID playableCharacterID, bool inActiveParty)
    {
        PlayableCombatantRuntimeData = SaveManager.Instance.LoadedData.GetPlayableCombatantRuntimeData(playableCharacterID);
        
        nameValue.text = playableCharacterID.ToString();
        hpValue.text = PlayableCombatantRuntimeData.CurrentHP.ToString();
        maxHPValue.text = PlayableCombatantRuntimeData.GetStat(IntStatType.MaxHP).ToString();
        mpValue.text = PlayableCombatantRuntimeData.CurrentMP.ToString();
        maxMPValue.text = PlayableCombatantRuntimeData.GetStat(IntStatType.MaxMP).ToString();

        characterSprite.sprite = PlayableCombatantRuntimeData.StaticInfo.TurnIcon;

        if (inActiveParty)
        {
            animator.SetTrigger("Active");
        }
        else
        {
            animator.SetTrigger("Reserve");
        }

        if (!content.activeInHierarchy)
        {
            content.SetActive(true);
        }
    }

    public void SwitchToActive()
    {
        animator.SetTrigger("Add");
    }

    public void SwitchToReserve()
    {
        animator.SetTrigger("Remove");
    }
}
