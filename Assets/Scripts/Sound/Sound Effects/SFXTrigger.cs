using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class SFXTrigger : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private bool triggerOnSelect;

    public void OnSelect(BaseEventData eventData)
    {
        if (triggerOnSelect) 
            SoundFXManager.Instance.PlayClip(audioClip, 0.5f);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (!triggerOnSelect)
            SoundFXManager.Instance.PlayClip(audioClip, 0.5f);
    }
}
