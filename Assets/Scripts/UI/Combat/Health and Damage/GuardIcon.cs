using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardIcon : MonoBehaviour
{
    //[SerializeField] private Animator animator;
    [SerializeField] private GameObject inactiveOverlay;

    public void ToggleActive(bool isActive)
    {
        if(isActive)
        {
            inactiveOverlay.SetActive(false);
        }
        else
        {
            inactiveOverlay.SetActive(true);
        }
    }
}
