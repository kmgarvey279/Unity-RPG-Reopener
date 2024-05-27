using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFilterTab : Tab
{
    [field: SerializeField] public ItemFilterType ItemFilterType { get; private set; }
}
