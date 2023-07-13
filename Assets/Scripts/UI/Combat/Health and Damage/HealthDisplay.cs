using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopupType
{
    Heal, 
    Damage,
    Buff,
    Debuff,
    Miss
}

public class HealthDisplay : MonoBehaviour
{
    private Combatant combatant;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject healthDisplay;
    [SerializeField] private AnimatedBar healthBar;
    [SerializeField] private AnimatedBar barrierBar;
    [SerializeField] private GameObject battlePopupPrefab;
    [SerializeField] private Transform healthSpawn;

    private void Awake() 
    {
        combatant = GetComponentInParent<Combatant>();
    }

    public void SetUICamera(Camera camera)
    {
        canvas.worldCamera = camera;
        canvas.overrideSorting = true;
    }

    private void Start()
    {
        healthBar.SetInitialValue(combatant.HP.MaxValue, combatant.HP.Value);
        barrierBar.SetInitialValue(combatant.Barrier.MaxValue, combatant.Barrier.Value);
    }

    public void Display(bool show)
    {
        healthDisplay.gameObject.SetActive(show);
    }

    public void DisplayPopup(PopupType popupType, CombatantType combatantType, string popupText, bool isCrit = false, ElementalResistance resistance = ElementalResistance.Neutral)
    {
        GameObject battlePopupObject = Instantiate(battlePopupPrefab, healthSpawn.position, Quaternion.identity);
        battlePopupObject.transform.SetParent(healthSpawn, true);
        battlePopupObject.GetComponent<BattlePopup>().Trigger(popupType, combatantType, popupText, isCrit);
    }

    public IEnumerator DisplayHealthChange()
    {
        if (!healthDisplay.gameObject.activeInHierarchy)
        {
            healthDisplay.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }

        //barrierBar.ResolveChange(combatant.Barrier.CurrentValue);
        healthBar.DisplayChange(combatant.HP.Value);
        yield return null;
    }

    public IEnumerator ResolveHealthChange()
    {
        healthBar.ResolveChange(combatant.HP.Value);

        yield return new WaitForSeconds(2f);
        healthDisplay.gameObject.SetActive(false);
    }
}
