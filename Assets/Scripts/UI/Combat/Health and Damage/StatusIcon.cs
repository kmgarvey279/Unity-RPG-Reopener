using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour
{
    public StatusEffectInstance StatusEffectInstance { get; private set; }
    [SerializeField] private Image iconImage;
    [SerializeField] private OutlinedText turnCounter;
    [SerializeField] private GameObject glowingSquare;

    public void AssignEffect(StatusEffectInstance _statusEffectInstance)
    {
        StatusEffectInstance = _statusEffectInstance;
        iconImage.sprite = _statusEffectInstance.StatusEffect.Icon;
        UpdateCounter();
    }

    public void UpdateCounter()
    {
        string text = StatusEffectInstance.Counter.ToString();
        if(StatusEffectInstance.StatusEffect.StatusCounterType == StatusCounterType.Stacks)
        {
            text = "x" + text;
        }
        turnCounter.SetText(text);
    }

    public void OnSelect()
    {
        glowingSquare.SetActive(true);
    }

    public void OnDeselect()
    {
        glowingSquare.SetActive(false);
    }
}
