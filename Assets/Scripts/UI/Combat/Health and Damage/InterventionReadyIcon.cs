using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterventionReadyIcon : MonoBehaviour
{
    [SerializeField] private GameObject inactiveOverlay;
    [SerializeField] private GameObject queuedOverlay;

    public void ToggleActive(bool isActive)
    {
        inactiveOverlay.SetActive(!isActive);
    }

    public void ToggleQueued(bool isQueued)
    {
        queuedOverlay.SetActive(isQueued);
    }
}
