using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GuidWrapper : MonoBehaviour
{
    [SerializeField]
    private string _guidString = System.Guid.NewGuid().ToString();

    [NonSerialized]
    private Guid _guid = Guid.Empty;

    public Guid Guid
    {
        get
        {
            if (_guid == Guid.Empty)
            {
                _guid = new Guid(_guidString);
            }

            return _guid;
        }
    }
}
