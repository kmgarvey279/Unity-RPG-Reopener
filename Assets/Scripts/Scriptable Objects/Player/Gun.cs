using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class Gun : ScriptableObject
{
    public string gunName;
    public float gunRecoil;
    public float gunDelay;
}
