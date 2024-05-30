using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaknessDisplay : MonoBehaviour
{
    [Header("Weak")]
    [SerializeField] private WeaknessIcon slashIconWeak;
    [SerializeField] private WeaknessIcon pierceIconWeak;
    [SerializeField] private WeaknessIcon strikeIconWeak;
    [SerializeField] private WeaknessIcon fireIconWeak;
    [SerializeField] private WeaknessIcon iceIconWeak;
    [SerializeField] private WeaknessIcon electricIconWeak;
    [SerializeField] private WeaknessIcon darkIconWeak;
    //[Header("Resist")]
    //[SerializeField] private GameObject slashIconResist;
    //[SerializeField] private GameObject pierceIconResist;
    //[SerializeField] private GameObject strikeIconResist;
    //[SerializeField] private GameObject fireIconResist;
    //[SerializeField] private GameObject iceIconResist;
    //[SerializeField] private GameObject electricIconResist;
    //[SerializeField] private GameObject darkIconResist;

    private Dictionary<ElementalProperty, WeaknessIcon> weaknesses;
    //private Dictionary<ElementalProperty, GameObject> resistances;

    public void Awake()
    {
        weaknesses = new Dictionary<ElementalProperty, WeaknessIcon>
        {
            { ElementalProperty.Slash, slashIconWeak },
            { ElementalProperty.Strike, strikeIconWeak },
            { ElementalProperty.Pierce, pierceIconWeak },
            { ElementalProperty.Fire, fireIconWeak },
            { ElementalProperty.Ice, iceIconWeak },
            { ElementalProperty.Electric, electricIconWeak },
            { ElementalProperty.Dark, darkIconWeak }
        };

        //resistances = new Dictionary<ElementalProperty, GameObject>
        //{
        //    { ElementalProperty.Slash, slashIconResist },
        //    { ElementalProperty.Strike, strikeIconResist },
        //    { ElementalProperty.Pierce, pierceIconResist },
        //    { ElementalProperty.Fire, fireIconResist },
        //    { ElementalProperty.Ice, iceIconResist },
        //    { ElementalProperty.Electric, electricIconResist },
        //    { ElementalProperty.Dark, darkIconResist }
        //};
    }

    public void DisplayWeaknesses(EnemyInfo enemyInfo)
    {
        if (!SaveManager.Instance.LoadedData.PlayerData.EnemyLog.EnemyEntries.ContainsKey(enemyInfo.EnemyID))
        {
            return;
        }
        EnemyLogEntry enemyLogEntry = SaveManager.Instance.LoadedData.PlayerData.EnemyLog.EnemyEntries[enemyInfo.EnemyID];

        //display weaknesses
        foreach (KeyValuePair<ElementalProperty, WeaknessIcon> iconEntry in weaknesses)
        {
            //display icons
            if (enemyInfo.Vulnerabilities.Contains(iconEntry.Key))
            {
                iconEntry.Value.gameObject.SetActive(true);
                //set revealed/unknown overlay
                if (enemyLogEntry.RevealedElements.Contains(iconEntry.Key))
                {
                    iconEntry.Value.ToggleUnknown(false);
                }
                else
                {
                    iconEntry.Value.ToggleUnknown(true);
                }
            }
            else
            {
                iconEntry.Value.gameObject.SetActive(false);
            }
        }
        //display resistances
        //foreach (KeyValuePair<ElementalProperty, GameObject> iconEntry in resistances)
        //{
        //    if (enemyInfo.Vulnerabilities.Contains(iconEntry.Key)
        //        && enemyLogEntry.RevealedElements.Contains(iconEntry.Key))
        //    {
        //        iconEntry.Value.SetActive(true);
        //    }
        //    else
        //    {
        //        iconEntry.Value.SetActive(false);
        //    }
        //}
    }
}
