using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSubmenu : MonoBehaviour
{
    protected bool isActive;
    [SerializeField] protected GameObject display;
    protected PauseMenu pauseMenu;

    protected virtual void Awake()
    {
        pauseMenu = GetComponentInParent<PauseMenu>();
        if (display.activeInHierarchy)
        {
            display.SetActive(false);
            isActive = false;
        }
    }

    public virtual void Display()
    {
        if (display != null && !display.activeInHierarchy)
        {
            display.SetActive(true);
        }
        isActive = true;
    }

    public virtual void Hide()
    {
        if (display.activeInHierarchy)
        {
            display.SetActive(false);
        }
        isActive = false;
    }
}
