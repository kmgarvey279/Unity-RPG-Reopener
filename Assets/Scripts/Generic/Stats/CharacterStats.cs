using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterInfo characterInfo;
    public Stat maxHealth;
    public float currentHealth;
    public Stat maxMagic;
    public float currentMagic;
    public Stat attack;
    public Stat defense;
    public Stat special;

    private void OnEnable()
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
