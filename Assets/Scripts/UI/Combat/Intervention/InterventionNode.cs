using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterventionNode : MonoBehaviour
{
    [SerializeField] private int value;
    [SerializeField] private AnimatedBar interventionBar;
    [SerializeField] private GameObject ready;
    [SerializeField] private GameObject queued;

    private void Awake()
    {
        ready.SetActive(false);
        queued.SetActive(false);
    }

    //public void SetInitialValue(int value)
    //{
    //    this.value = value;
    //    interventionBar.SetInitialValue(100, value);
    //    if (value == 100)
    //    {
    //        ready.SetActive(true);
    //    }
    //    else
    //    {
    //        ready.SetActive(false);
    //    }
    //}

    //public void SetValue(int value)
    //{
    //    interventionBar.DisplayChange(value);
    //    interventionBar.ResolveChange(value);
    //    if (value == 100)
    //    {
    //        glow.SetActive(true);
    //    }
    //    else
    //    {
    //        glow.SetActive(false);
    //    }
    //}

    public void ToggleReady(bool isReady)
    {
        ready.SetActive(isReady);
    }

    public void ToggleQueued(bool isQueued)
    {
        queued.SetActive(isQueued);
    }

    //public void RefreshGlowAnimation()
    //{
    //    glow.Refresh();
    //}
}
