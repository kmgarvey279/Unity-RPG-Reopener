using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuTab : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private PauseMenu pauseMenu;
    [SerializeField] private PauseSubmenuType pauseSubmenuType;

    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private GameObject cursor;
    [SerializeField] private Color selectedColor;

    private void Start()
    {
        pauseMenu = GetComponentInParent<PauseMenu>();
    }

    public void ToggleActive(bool isActiveMenu)
    {

    }

    public void OnSelect(BaseEventData eventData)
    {
        cursor.SetActive(true);
        label.color = selectedColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        cursor.SetActive(false);
        label.color = Color.white;
    }

    public void OnClick()
    {
        if (pauseMenu != null)
        {
            pauseMenu.OnClickTab(pauseSubmenuType);
        }
        OnDeselect(null);
    }
}
