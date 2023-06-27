using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitInstance
{
    public Trait Trait { private set; get; }
    public bool IsDisabled { private set; get; } = false;

    public TraitInstance(Trait trait)
    {
        Trait = trait;
    }

    public void ToggleDisabled(bool isDisabled)
    {
        IsDisabled = isDisabled;
    }
}
