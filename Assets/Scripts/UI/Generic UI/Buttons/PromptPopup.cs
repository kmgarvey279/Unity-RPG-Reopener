using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PromptPopup : MonoBehaviour
{
    [SerializeField] private GameObject firstOption;
    [SerializeField] private SignalSenderInt onClickPromptOption;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstOption);
    }

    public void OnClickOption(int selectedOption)
    {
        onClickPromptOption.Raise(selectedOption);
    }
}
