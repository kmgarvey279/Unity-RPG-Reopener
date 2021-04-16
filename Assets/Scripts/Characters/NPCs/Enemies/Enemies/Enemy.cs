using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    private bool playerLockOn = false;
    [SerializeField] private GameObject lockIcon;

    public void ToggleLockOn(bool newState)
    {
        playerLockOn = newState;
        lockIcon.SetActive(newState);
    }
}
