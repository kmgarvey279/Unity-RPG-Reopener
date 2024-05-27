using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public StatusEffectInstance StatusEffectInstance { get; private set; }
    [SerializeField] private Image iconImage;
    [SerializeField] private OutlinedText duration;
    [SerializeField] private OutlinedText stacks;
    [SerializeField] private GameObject glowingSquare;
    private Button button;
    [SerializeField] private SignalSenderGO onSelectStatusIcon;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void AssignEffect(StatusEffectInstance _statusEffectInstance)
    {
        StatusEffectInstance = _statusEffectInstance;
        iconImage.sprite = _statusEffectInstance.StatusEffect.Icon;
        if (StatusEffectInstance.StatusEffect.HasDuration)
        {
            duration.SetText(StatusEffectInstance.Duration.CurrentValue.ToString());
        }
        else
        {
            duration.SetText("-");
        }
        if (StatusEffectInstance.StatusEffect.HasStacks)
        {
            stacks.SetText("x" + StatusEffectInstance.Stacks.CurrentValue.ToString());
        }
        else
        {
            stacks.SetText("-");
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        glowingSquare.SetActive(true);
        onSelectStatusIcon.Raise(this.gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        glowingSquare.SetActive(false);
    }

    public void ToggleButton(bool isActive)
    {
        button.interactable = isActive;
    }
}
