using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnPanel : MonoBehaviour
{
    [Header("Data")]
    public TurnSlot turnSlot;
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI nameText;
    [Header("Panel")]
    [SerializeField] private GameObject display;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color previewColor;
    [Header("Animations/Movement")]
    private Animator animator;
    RectTransform rt;
    [SerializeField] float moveTime;
    private float defaultXPos;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();   
        SetToDefaultColor();
    }

    public void Move(Vector3 newPosition)
    {
        Vector3 start = transform.position; 
        float t = 0;
        while(transform.position != newPosition)
        {
            t += Time.deltaTime/moveTime;
            transform.position = Vector3.Lerp(start, newPosition, t); 
        }
    }

    public void AssignTurnSlot(TurnSlot newSlot)
    {
        turnSlot = newSlot;
        nameText.text = turnSlot.combatant.characterName;
    }

    public void ToggleTargetingPreview(bool isTargeted)
    {
        animator.SetBool("Targeted", isTargeted);
        RectTransform rt = display.GetComponent<RectTransform>();
        if(isTargeted)
        {
            rt.anchoredPosition = new Vector2(-10, 0);
        }
        else 
        {
            rt.anchoredPosition = new Vector2(0, 0);
        }
    }

    public void SetToPreviewColor()
    {
        display.GetComponent<Image>().color = previewColor;
    }

    public void SetToDefaultColor()
    {
        display.GetComponent<Image>().color = defaultColor;
    }
}
