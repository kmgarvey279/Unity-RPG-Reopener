using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ScrollableListSlotSave : ScrollableListSlot
{
    [SerializeField] public int FileNum { get; private set; } 
    
    [SerializeField] private TextMeshProUGUI fileNumValue;
    [SerializeField] private TextMeshProUGUI locationValue;
    [SerializeField] private TextMeshProUGUI playtimeValue;
    [SerializeField] private TextMeshProUGUI dateValue;

    [SerializeField] private Image background;
    [SerializeField] private Color emptyColor;
    [SerializeField] private Color savedColor;
    [SerializeField] private Color autoColor;
    [SerializeField] private Color autoTextColor;
    [SerializeField] private GameObject glow;

    //[SerializeField] private SignalSenderGO onSelectSave;
    [SerializeField] private SignalSenderGO onClickSaveFile;

    [SerializeField] private PartyTabDisplay partyTabDisplay;
    //[SerializeField] private List<SaveSlotPartyIcon> partyIcons =  new List<SaveSlotPartyIcon>();
    //private Dictionary<PlayableCharacterID, Sprite> playerSprites;
    //[SerializeField] private Sprite claireSprite;
    //[SerializeField] private Sprite mutinySprite;
    //[SerializeField] private Sprite blaineSprite;
    //[SerializeField] private Sprite shadSprite;
    //[SerializeField] private Sprite oshiSprite;
    //[SerializeField] private Sprite lucySprite;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
    }

    public override void OnClick()
    {
        Debug.Log("On click save slot");
        onClickSaveFile.Raise(this.gameObject);
    }

    public void AssignSaveFileData(SaveFile saveFile)
    {
        if (saveFile.PlayerData != null)
        {
            if (saveFile.FileNum == 0)
            {
                background.color = autoColor;
            }
            else
            {
                background.color = savedColor;
            }
        }
        else
        {
            background.color = emptyColor;
        }

        FileNum = saveFile.FileNum;
        if (saveFile.FileNum == 0)
        {
            fileNumValue.text = "Autosave";
            fileNumValue.color = autoTextColor;
        }
        else
        {
            fileNumValue.text = "File " + saveFile.FileNum.ToString();
        }
        locationValue.text = saveFile.SaveLocation;
        playtimeValue.text = TimeSpan.FromSeconds(saveFile.Playtime).Hours.ToString("D2")
            + ":" + TimeSpan.FromSeconds(saveFile.Playtime).Minutes.ToString("D2")
            + ":" + TimeSpan.FromSeconds(saveFile.Playtime).Seconds.ToString("D2");
        dateValue.text = saveFile.SaveDate;

        if (saveFile.PlayerData != null)
        {
            PartyData partyData = saveFile.PlayerData.PartyData;
            partyTabDisplay.DisplayParty(partyData.ActivePartyMembers, partyData.ReservePartyMembers);
        }
    }

    public void ToggleGlow(bool isGlowing)
    {
        glow.SetActive(isGlowing);
    }
}
