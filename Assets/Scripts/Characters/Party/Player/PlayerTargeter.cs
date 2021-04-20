using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeter : Targeter
{
    // public bool targetLock = false;

    // public override void AddTarget(GameObject obj)
    // {       
    //     base.AddTarget(obj);
    //     if(currentTarget == null)
    //     {
    //         currentTarget = obj;
    //         targetLock = true;
    //         currentTarget.GetComponent<Enemy>().ToggleLockOn(true);
    //     }
    // }

    // public void ToggleTargetLock()
    // {
    //     if(targetLock)
    //     {
    //         targetLock = false;
    //         currentTarget = null;
    //         currentTarget.GetComponent<Enemy>().ToggleLockOn(false);
    //     }
    //     else
    //     {
    //         SetNewTarget();
    //         if(currentTarget != null)
    //         {
    //             targetLock = true;
    //             currentTarget.GetComponent<Enemy>().ToggleLockOn(true);
    //         }
    //     }
    // }

    // public override void SetNewTarget()
    // {
    //     currentTarget = GetClosestTarget();
    // }
}
