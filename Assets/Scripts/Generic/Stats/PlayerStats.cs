using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private EquipmentManager equipmentManager;

    private void Start()
    {
       equipmentManager = GetComponent<EquipmentManager>();
       equipmentManager.onEquipmentChanged += OnEquipmentChanged; 
    }

    private void OnEquipmentChanged(EquipmentObject newEquipment, EquipmentObject oldEquipment)
    {
        if(newEquipment != null)
        {
            attack.AddModifier(newEquipment.attackModifier);
            defense.AddModifier(newEquipment.defenseModifier);
            special.AddModifier(newEquipment.specialModifier);
        }

        if(oldEquipment != null)
        {
            attack.RemoveModifier(newEquipment.attackModifier);
            defense.RemoveModifier(newEquipment.defenseModifier);
            special.RemoveModifier(newEquipment.specialModifier);
        }
    }
}
