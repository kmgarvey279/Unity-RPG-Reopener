using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartyTabDisplay : MonoBehaviour
{
    [SerializeField] private bool isInteractable;
    [SerializeField] private int selectedTabIndex = 0;
    [SerializeField] private List<PartyTab> partyTabs = new List<PartyTab>();
    [field: SerializeField] public List<PartyTab> DisplayedTabs { get; private set; } = new List<PartyTab>();

    private Dictionary<PlayableCharacterID, Sprite> playerSprites;
    [SerializeField] private Sprite claireSprite;
    [SerializeField] private Sprite mutinySprite;
    [SerializeField] private Sprite blaineSprite;
    [SerializeField] private Sprite shadSprite;
    [SerializeField] private Sprite oshiSprite;
    [SerializeField] private Sprite lucySprite;

    [SerializeField] private SignalSender onChangeCharacterTab;

    private void OnEnable()
    {
        if (isInteractable)
        {
            InputManager.Instance.OnPressLeft.AddListener(PressLeft);
            InputManager.Instance.OnPressRight.AddListener(PressRight);
        }
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPressLeft.RemoveListener(PressLeft);
        InputManager.Instance.OnPressRight.RemoveListener(PressRight);
    }

    private void Awake()
    {
        playerSprites = new Dictionary<PlayableCharacterID, Sprite>()
        {
            { PlayableCharacterID.Claire, claireSprite },
            { PlayableCharacterID.Mutiny, mutinySprite },
            { PlayableCharacterID.Blaine, blaineSprite },
            { PlayableCharacterID.Shad, shadSprite },
            { PlayableCharacterID.Oshi, oshiSprite },
            { PlayableCharacterID.Lucy, lucySprite }
        };

        partyTabs = GetComponentsInChildren<PartyTab>().ToList();
        foreach (PartyTab tab in partyTabs)
        {
            tab.gameObject.SetActive(false);
        }
    }

    public void DisplayParty(List<PlayableCharacterID> activePartyMembers, List<PlayableCharacterID> reservePartyMembers)
    {
        DisplayedTabs.Clear();

        List<PlayableCharacterID> fullParty = new List<PlayableCharacterID>();
        fullParty.AddRange(activePartyMembers);
        fullParty.AddRange(reservePartyMembers);

        for (int i = 0; i < fullParty.Count; i++)
        {
            DisplayedTabs.Add(partyTabs[i]);

            partyTabs[i].gameObject.SetActive(true);

            partyTabs[i].SetValues(fullParty[i], playerSprites[fullParty[i]]);

            if (activePartyMembers.Contains(fullParty[i]))
            {
                DisplayedTabs[i].ToggleOverlay(false);
            }
            else
            {
                DisplayedTabs[i].ToggleOverlay(true);
            }
        }

        if (isInteractable)
        {
            selectedTabIndex = 0;
            DisplayedTabs[selectedTabIndex].ToggleSelected(true);
        }
    }

    public PlayableCharacterID CurrentCharacterID
    {
        get 
        {
            return DisplayedTabs[selectedTabIndex].PlayableCharacterID;
        }
    }

    public void ToggleInteractivity(bool isCurrentlyInteractable)
    {
        if (isCurrentlyInteractable)
        {
            InputManager.Instance.OnPressLeft.AddListener(PressLeft);
            InputManager.Instance.OnPressRight.AddListener(PressRight);
        }
        else
        {
            InputManager.Instance.OnPressLeft.RemoveListener(PressLeft);
            InputManager.Instance.OnPressRight.RemoveListener(PressRight);
        }
    }

    public void PressLeft(bool isPressed)
    {
        CycleTabs(-1);
    }

    public void PressRight(bool isPressed)
    {
        CycleTabs(1);
    }

    private void CycleTabs(int direction)
    {
        if (DisplayedTabs.Count <= 1)
        {
            return;
        }

        foreach (PartyTab partyTab in partyTabs)
        {
            partyTab.ToggleSelected(false);
        }

        int newIndex = selectedTabIndex + direction;
        if (newIndex >= DisplayedTabs.Count)
        {
            newIndex = 0;
        }
        if (newIndex < 0)
        {
            newIndex = DisplayedTabs.Count - 1;
        }
        selectedTabIndex = newIndex;

        DisplayedTabs[selectedTabIndex].ToggleSelected(true);

        onChangeCharacterTab.Raise();
    }
}
