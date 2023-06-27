using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DynamicListSlot : MonoBehaviour, ISelectHandler
{
    [HideInInspector] public DynamicList dynamicList;
    [SerializeField] protected OutlinedText nameText;
    public Image icon;
    public Button button;

    public void OnEnable()
    {
        button = GetComponent<Button>();
    }

    public virtual void AssignData(ScriptableObject scriptableObject)
    {
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        if(dynamicList != null)
        {
            dynamicList.OnSelectSlot(this);
        }
    }
}
