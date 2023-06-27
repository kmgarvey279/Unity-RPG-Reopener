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
    [SerializeField] private GameObject damagePopupPrefab;
    [SerializeField] private GameObject critPopupPrefab;
    [SerializeField] private Transform healthSpawn;
    [SerializeField] private List<Transform> popupSpawnPoints;

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
        healthBar.SetInitialValue(combatant.HP.GetValue(), combatant.HP.CurrentValue);
        barrierBar.SetInitialValue(combatant.Barrier.GetValue(), combatant.Barrier.CurrentValue);
    }

    public void Display(bool show)
    {
        healthDisplay.gameObject.SetActive(show);
    }

    public IEnumerator DisplayHealthChange(PopupType popupType, string amount, bool isCrit = false, ElementalResistance resistance = ElementalResistance.Neutral)
    {
        GameObject damagePopupObject = null;
        if(isCrit)
        {
            damagePopupObject = Instantiate(critPopupPrefab, healthSpawn.position, Quaternion.identity);
        }
        else
        {
            damagePopupObject = Instantiate(damagePopupPrefab, healthSpawn.position, Quaternion.identity);
        }
        int roll = Random.Range(0, popupSpawnPoints.Count);
        damagePopupObject.transform.SetParent(healthSpawn, true);
        damagePopupObject.transform.position = popupSpawnPoints[roll].position;
        damagePopupObject.GetComponent<DamagePopup>().TriggerHealthPopup(popupType, amount);
        yield return null;

        if (!healthDisplay.gameObject.activeInHierarchy)
        {
            healthDisplay.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }

        barrierBar.ResolveChange(combatant.Barrier.CurrentValue);
        healthBar.DisplayChange(combatant.HP.CurrentValue);
    }

    public IEnumerator ResolveHealthChange()
    {
        healthBar.ResolveChange(combatant.HP.CurrentValue);

        yield return new WaitForSeconds(2f);
        healthDisplay.gameObject.SetActive(false);
    }
}
