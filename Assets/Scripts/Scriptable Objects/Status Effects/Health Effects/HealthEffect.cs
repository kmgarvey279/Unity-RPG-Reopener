using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEffect : MonoBehaviour
{
    protected bool isHeal = false;
    // Start is called before the first frame update
    public virtual int GetHealthChange(Combatant combatant, int potency, int remainingTurns)
    {
        return 0;
    }
}
