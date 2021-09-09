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
    [SerializeField] private Color previewColor;
    [Header("Info")]
    [SerializeField] private GameObject targetedIcon;
    [SerializeField] private TextMeshProUGUI accuracyText;
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
        nameText.text = turnSlot.combatant.characterName;
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

    public void DisplayAccuracyPreview(int accuracy)
    {
        animator.SetBool("Targeted", true);
        targetedIcon.SetActive(true);
        accuracyText.text = accuracy.ToString() + "%";
    }

    public void ClearAccuracyPreview()
    {
        animator.SetBool("Targeted", false);
        targetedIcon.SetActive(false);;
    }

    public void ToggleNextTurnIndicator(bool isNextTurn)
    {
        if(isNextTurn)
        {
            panel.color = previewColor;
        }
        else
        {
            panel.color = defaultColor;
        }
    }
}
