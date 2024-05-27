using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFilterTabs : MonoBehaviour
{
    [SerializeField] private int selectedTabIndex = 0;
    [SerializeField] private List<ItemFilterTab> tabs = new List<ItemFilterTab>();
    [SerializeField] private SignalSender onChangeItemTab;

    public ItemFilterType CurrentItemFilterType
    {
        get
        {
            return tabs[selectedTabIndex].ItemFilterType;
        }
    }

    private void OnEnable()
    {
        foreach (Tab tab in tabs)
        {
            tab.ChangeState(TabStateType.Default);
        }

        selectedTabIndex = 0;
        tabs[selectedTabIndex].ChangeState(TabStateType.Selected);

        InputManager.Instance.OnPressLeft.AddListener(PressLeft);
        InputManager.Instance.OnPressRight.AddListener(PressRight);
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPressLeft.RemoveListener(PressLeft);
        InputManager.Instance.OnPressRight.RemoveListener(PressRight);
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
        int newIndex = selectedTabIndex + direction;
        if (newIndex >= tabs.Count)
        {
            newIndex = 0;
        }
        if (newIndex < 0)
        {
            newIndex = tabs.Count - 1;
        }
        ChangeCurrentTab(newIndex);
    }

    private void ChangeCurrentTab(int newIndex)
    {
        foreach (Tab tab in tabs)
        {
            tab.ChangeState(TabStateType.Default);
        }

        selectedTabIndex = newIndex;
        tabs[selectedTabIndex].ChangeState(TabStateType.Selected);

        onChangeItemTab.Raise();
    }
}
