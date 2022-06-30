using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnPanel : MonoBehaviour
{
    [Header("Data")]
    public Combatant combatant;
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI nameText;
    [Header("Panel")]
    [SerializeField] private GameObject display;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color previewColor;
    [Header("Animations/Movement")]
    public Animator animatorMovement;
    [SerializeField] private Animator animatorColor;
    RectTransform rt;
    public Vector3 destination;
    [Header("Cursor")]
    [SerializeField] private Image selectCursor;

    private void Start()
    {
        SetToDefaultColor();
    }

    public void Move(Vector3 newPosition)
    {
        RectTransform rt = GetComponent<RectTransform>();
        destination = newPosition;
        Vector3 start = rt.position; 
        float moveTime = 0.5f;
        float t = 0;
        Vector3 step = start;
        while(t < moveTime)
        {
            step = Vector3.Lerp(start, destination, t / moveTime);
            t += Time.deltaTime;
            rt.position = step;
        }
        if(rt.position != destination)
        {
            rt.position = destination;
        }
    }

    public void Remove(Combatant combatant)
    {

    }

    public void Insert(Combatant combatant, int newIndex)
    {

    }

    public void AssignCombatant(Combatant combatant)
    {
        this.combatant = combatant;
        nameText.text = combatant.characterName;
    }
    
    public void ToggleSelectCursor(bool isSelected)
    {
        selectCursor.enabled = isSelected;
    }

    public void ToggleTargetingPreview(bool isTargeted)
    {
        // animator.SetBool("Targeted", isTargeted);
        RectTransform rt = display.GetComponent<RectTransform>();
        if(isTargeted)
        {
            animatorColor.SetTrigger("Targeted");
            rt.anchoredPosition = new Vector2(-10, 0);
        }
        else 
        {
            animatorColor.SetTrigger("Default");
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
