using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : ScriptableObject
{
    public string name;
    public int level;
    [Header("Stats")]
    public Stat maxHealth;
    public float currentHealth;
    public Stat maxMagic;
    public float currentMagic;
    public Stat attack;
    public Stat defense;
    public Stat special;

    public virtual void OnEnable()
    {
        currentHealth = maxHealth.GetValue();
        currentMagic = maxMagic.GetValue();
    }

    public void ChangeCurrentHealth(float amount)
    {
        float temp = currentHealth + amount;
        currentHealth = Mathf.Clamp(temp, 0, maxHealth.GetValue());
    }

    public void ChangeCurrentMagic(float amount)
    {
        float temp = currentMagic + amount;
        currentMagic = Mathf.Clamp(temp, 0, maxMagic.GetValue());
    } 
}
