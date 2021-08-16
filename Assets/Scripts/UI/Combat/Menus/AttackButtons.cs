using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButtons : MonoBehaviour
{
    [SerializeField] private Image attackIcon1;
    [SerializeField] private Image attackIcon2;

    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;

    public void UpdateButtons(bool button1State, bool button2State)
    {
        if(button1State)
        {
            attackIcon1.color = activeColor;
        } 
        else
        {
            attackIcon1.color = inactiveColor;
        }

        if(button2State)
        {
            attackIcon2.color = activeColor;
        } 
        else
        {
            attackIcon2.color = inactiveColor;
        }
    }
}
