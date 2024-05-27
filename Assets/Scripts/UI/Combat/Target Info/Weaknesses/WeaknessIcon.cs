using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaknessIcon : MonoBehaviour
{
    [SerializeField] private GameObject unknownOverlay;
    [SerializeField] private GameObject bonusBorder;
    [SerializeField] private Glow bonusGlow;

    public void ToggleUnknown(bool isUnknown)
    {
        unknownOverlay.SetActive(isUnknown);
    }

    public void ToggleBonus(bool bonusActive)
    {
        bonusBorder.SetActive(bonusActive);
    }

    public void RefreshGlow()
    {
        bonusGlow.Refresh();
    }
}
