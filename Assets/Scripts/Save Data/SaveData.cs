using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{

    public static SaveData current;
    public Combatant claire;
    public Combatant blaine;
    public Combatant mutiny;

    public SaveData()
    {
        claire = new Combatant();
        blaine = new Combatant();
        mutiny = new Combatant();
    }
}
