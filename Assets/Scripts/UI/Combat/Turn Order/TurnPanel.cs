using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnPanel : MonoBehaviour
{
    [Header("Data")]
    public TurnSlot turnSlot;
    [Header("Bar")]
    [SerializeField] private SliderBar sliderBarPositive;
    [SerializeField] private SliderBar sliderBarNegative;
    private int sliderMax = 1000;
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI nameText;
    [Header("Panel")]
    [SerializeField] private Image panel;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color highlightColor;
    [Header("Info")]
    [SerializeField] private GameObject targetedIcon;
    [SerializeField] private GameObject nextIcon;
    [Header("Animations/Movement")]
    private Animator animator;
    [SerializeField] float moveTime;

    private void Start()
    {
        animator = GetComponent<Animator>();    
    }

    public void Move(Vector3 newPosition)
    {
        Vector2 start = transform.position; 
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
        nameText.text = turnSlot.combatant.battleStats.characterName;
        sliderBarPositive.SetMaxValue(sliderMax);
        sliderBarNegative.SetMaxValue(sliderMax);
        UpdateSliderValue();
    }

    public void UpdateSliderValue()
    {
        if(turnSlot.turnCounter < 0)
        {
            sliderBarPositive.SetCurrentValue(0);
            sliderBarNegative.SetCurrentValue(-turnSlot.turnCounter);
        }
        else 
        {
            sliderBarNegative.SetCurrentValue(0);
            sliderBarPositive.SetCurrentValue(turnSlot.turnCounter);  
        }
    }

    public void ToggleNextIcon(bool isNext)
    {
        nextIcon.SetActive(isNext);
    }

    public void ToggleTargetedAnimation(bool isTargeted)
    {
        animator.SetBool("Targeted", isTargeted);
        targetedIcon.SetActive(isTargeted);
    }

    public void Highlight()
    {
        panel.color = highlightColor;
    }

    public void Unhighlight()
    {
        panel.color = defaultColor;
    }
}
