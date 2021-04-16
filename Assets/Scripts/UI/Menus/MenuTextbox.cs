using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuTextbox : MonoBehaviour
{
    public TextMeshProUGUI textField;

    public void UpdateText(string newText)
    {
        textField.text = newText;
    }

    private void OnDisable()
    {
        textField.text = "";    
    }
}
