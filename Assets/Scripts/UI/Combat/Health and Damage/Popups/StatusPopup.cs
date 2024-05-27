using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusPopup : MonoBehaviour
{
    [Header("Icon")]
    [SerializeField] private Image icon;
    [Header("Text")]
    [SerializeField] private GameObject textContainer;
    [SerializeField] private OutlinedText stackValue;
    [Header("Animation")]
    [SerializeField] private Animator motionAnimator;

    public void TriggerAdd(StatusEffect statusEffect, int stackChange, float duration)
    {
        //set icon
        if (statusEffect && statusEffect.Icon)
        {
            icon.sprite = statusEffect.Icon;
        }

        if (stackChange != 0)
        {
            textContainer.SetActive(true);
            stackValue.SetText("+" + stackChange);
        }
        else
        {
            textContainer.SetActive(false);
        }
        motionAnimator.SetTrigger("Up");
        StartCoroutine(ClearPopupCo(duration));
    }

    public void TriggerRemove(StatusEffect statusEffect, int stackChange, float duration)
    {
        //set icon
        if (statusEffect && statusEffect.Icon)
        {
            icon.sprite = statusEffect.Icon;
        }

        if (stackChange != 0)
        {
            textContainer.SetActive(true);
            stackValue.SetText("-" + stackChange);
        }
        else
        {
            textContainer.SetActive(false);
        }
        motionAnimator.SetTrigger("Down");
        StartCoroutine(ClearPopupCo(duration));
    }

    private IEnumerator ClearPopupCo(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }
}
