using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpeakerData
{
    [field: SerializeField] public string SpeakerName { get; private set; } 
    [field: SerializeField, TextArea(2, 4)] public List<string> Lines { get; private set; }
}

[CreateAssetMenu(fileName = "New Dialogue Data", menuName = "Story/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [field: SerializeField] public List<SpeakerData> Speakers { get; private set; }
}
