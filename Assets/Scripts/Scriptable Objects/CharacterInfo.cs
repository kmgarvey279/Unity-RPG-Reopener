using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resistance
{
    public AttackProperty attackProperty;
    public float value;
}

public class CharacterInfo : ScriptableObject
{
    public string name;
    public int level;
    [Header("HP/MP")]
    public Stat maxHealth;
    public float currentHealth;
    public Stat maxMana;
    public float currentMana;
    [Header("Stats")]
    public Stat attack;
    public Stat defense;
    public Stat special;
    public Stat moveSpeed;
    [Header("Resistances")]
    public Resistance fire;
    public Resistance ice;
    public Resistance electric;
    public Resistance dark;

    public virtual void OnEnable()
    {
        currentHealth = maxHealth.GetValue();
        currentMana = maxMana.GetValue();
    }

    public void ChangeCurrentHealth(float amount)
    {
        float temp = currentHealth + amount;
        currentHealth = Mathf.Clamp(temp, 0, maxHealth.GetValue());
    }

    public void ChangeCurrentMana(float amount)
    {
        float temp = currentMana + amount;
        currentMana = Mathf.Clamp(temp, 0, maxMana.GetValue());
    } 
}
