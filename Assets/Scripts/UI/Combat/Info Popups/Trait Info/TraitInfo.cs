using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TraitInfo : MonoBehaviour
{
    [Header("Header")]
    [SerializeField] private TextMeshProUGUI traitNameValue;
    [SerializeField] private TextMeshProUGUI traitTypeValue;
    [Header("Info")]
    [SerializeField] private TextBox textBox;

    public void DisplayTrait(Trait trait)
    {
        traitNameValue.SetText(trait.TraitName);
        traitTypeValue.SetText(trait.TraitType);

        textBox.SetText(trait.Description, trait.SecondaryDescription);
    }

    public void Clear()
    {
        traitNameValue.text = "------";
        traitTypeValue.text = "";
        textBox.Clear();
    }
}

