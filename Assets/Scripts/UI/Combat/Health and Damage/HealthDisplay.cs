using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    protected class QueuedStatusPopup
    {
        public StatusEffect StatusEffect;
        public int StackChange = 0;

        public QueuedStatusPopup(StatusEffect statusEffect, int stackChange)
        {
            StatusEffect = statusEffect;
            StackChange = stackChange;
        }
    }

    private Combatant combatant;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Mask mask;
    [Header("Bars")]
    [SerializeField] private AnimatedBar healthBar;
    [SerializeField] private AnimatedBar barrierBar;
    [Header("Guard")]
    [SerializeField] private GameObject guardContainer;
    [SerializeField] private PointBar guardPointBar;
    [Header("Popups")]
    [SerializeField] private GameObject battlePopupPrefab;
    [SerializeField] private GameObject statusPopupPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform spawnPoint2;
    private List<QueuedStatusPopup> QueuedAddPopups = new List<QueuedStatusPopup>();
    private bool addPopupActive = false;
    private List<QueuedStatusPopup> QueuedRemovePopups = new List<QueuedStatusPopup>();
    private bool removePopupActive = false;
    private static float popupDuration = 1f;
    private WaitForSeconds waitForDuration = new WaitForSeconds(0.5f);

    private void Awake() 
    {
        combatant = GetComponentInParent<Combatant>();
        mask.enabled = true;
    }

    private void Update()
    {
        while (QueuedAddPopups.Count > 0 && addPopupActive == false)
        {
            StartCoroutine(TriggedAddStatusPopupCo(QueuedAddPopups[0]));
        }

        while (QueuedRemovePopups.Count > 0 && removePopupActive == false)
        {
            StartCoroutine(TriggedRemoveStatusPopupCo(QueuedRemovePopups[0]));
        }
    }

    private void OnDisable()
    {
        QueuedAddPopups.Clear();
        addPopupActive = false;
        QueuedRemovePopups.Clear();
        removePopupActive = false;
    }

    public void SetUICamera(Camera camera)
    {
        canvas.worldCamera = camera;
        canvas.overrideSorting = true;
    }

    public void SetValues()
    {
        healthBar.SetInitialValue(combatant.MaxHP, combatant.HP);
        barrierBar.SetInitialValue(combatant.MaxHP, 0);
        if (combatant is EnemyCombatant)
        {
            guardContainer.SetActive(true);
        }
        else
        {
            guardContainer.SetActive(false);
        }
    }

    public void SetGuardValues()
    {
        if (combatant is EnemyCombatant)
        {
            EnemyCombatant enemyCombatant = (EnemyCombatant)combatant;
            //guardBar.SetInitialValue(enemyCombatant.Guard.MaxValue, enemyCombatant.Guard.CurrentValue);
            guardPointBar.SetValue(enemyCombatant.Guard);
        }
    }

    public void Display()
    {
        healthBar.SetValue(combatant.HP);

        if (combatant is EnemyCombatant)
        {
            EnemyCombatant enemyCombatant = (EnemyCombatant)combatant;

            guardPointBar.SetValue(enemyCombatant.Guard);
        }
        mask.enabled = false;
    }

    public void Hide()
    {
        mask.enabled = true;
    }

    //public void DisplayVulnerabilityIcon(bool isRevealed, bool isVulnerable)
    //{
    //    if (combatant is PlayableCombatant)
    //    {
    //        return;
    //    }

    //    vulnContainer.SetActive(true);
    //    if (isRevealed)
    //    {
    //        unknownIcon.SetActive(false);
    //    }
    //    else
    //    {
    //        unknownIcon.SetActive(true);
    //    }
    //    if (isVulnerable)
    //    {
    //        vulnerableIcon.SetActive(true);
    //    }
    //    else
    //    {
    //        vulnerableIcon.SetActive(false);
    //    }
    //}

    public void DisplayNumberPopup(PopupType popupType, CombatantType combatantType, int amount, bool isCrit = false, bool isVulnerable = false)
    {
        Debug.Log("Triggering " + popupType.ToString() + " popup. Value: " + amount);
        GameObject battlePopupObject = Instantiate(battlePopupPrefab, spawnPoint.position, Quaternion.identity);
        battlePopupObject.transform.SetParent(spawnPoint, true);
        battlePopupObject.GetComponent<BattlePopup>().TriggerNumber(popupType, combatantType, amount, isCrit, isVulnerable, popupDuration);
    }

    public void DisplayTextPopup(PopupType popupType, string text)
    {
        GameObject battlePopupObject = Instantiate(battlePopupPrefab, spawnPoint.position, Quaternion.identity);
        battlePopupObject.transform.SetParent(spawnPoint, true);
        battlePopupObject.GetComponent<BattlePopup>().TriggerText(popupType, text, popupDuration);
    }

    public void QueueAddStatusPopup(StatusEffect statusEffect, int stackChange)
    {
        QueuedStatusPopup newQueuedPopup = new QueuedStatusPopup(statusEffect, stackChange);
        QueuedAddPopups.Add(newQueuedPopup);
    }

    private IEnumerator TriggedAddStatusPopupCo(QueuedStatusPopup queuedStatusPopup)
    {
        addPopupActive = true;
        GameObject statusPopupObject = Instantiate(statusPopupPrefab, spawnPoint.position, Quaternion.identity);
        statusPopupObject.transform.SetParent(spawnPoint, true);
        statusPopupObject.GetComponent<StatusPopup>().TriggerAdd(queuedStatusPopup.StatusEffect, queuedStatusPopup.StackChange, popupDuration);

        yield return waitForDuration;
        if (QueuedAddPopups.Contains(queuedStatusPopup))
        {
            QueuedAddPopups.Remove(queuedStatusPopup);
        }
        addPopupActive = false;
    }

    public void QueueRemoveStatusPopup(StatusEffect statusEffect, int stackChange)
    {
        QueuedStatusPopup newQueuedPopup = new QueuedStatusPopup(statusEffect, stackChange);
        QueuedRemovePopups.Add(newQueuedPopup);
    }

    private IEnumerator TriggedRemoveStatusPopupCo(QueuedStatusPopup queuedStatusPopup)
    {
        removePopupActive = true;
        GameObject statusPopupObject = Instantiate(statusPopupPrefab, spawnPoint2.position, Quaternion.identity);
        statusPopupObject.transform.SetParent(spawnPoint2, true);
        statusPopupObject.GetComponent<StatusPopup>().TriggerRemove(queuedStatusPopup.StatusEffect, queuedStatusPopup.StackChange, popupDuration);
        
        yield return waitForDuration;
        if (QueuedRemovePopups.Contains(queuedStatusPopup))
        {
            QueuedRemovePopups.Remove(queuedStatusPopup);
        }
        removePopupActive = false;
    }

    public void DisplayChanges()
    {
        mask.enabled = false;
        //barrier
        barrierBar.DisplayChange(combatant.Barrier);
        barrierBar.ResolveChange(combatant.Barrier);
        //health
        healthBar.DisplayChange(combatant.HP);
        //stance
        if (combatant is EnemyCombatant)
        {
            EnemyCombatant enemyCombatant = (EnemyCombatant)combatant;
            guardPointBar.UpdatePoints(enemyCombatant.Guard);
        }
    }

    //public void DisplayStanceChange()
    //{
    //    if (combatant is EnemyCombatant)
    //    {
    //        EnemyCombatant enemyCombatant = (EnemyCombatant)combatant;
    //        mask.enabled = false;
    //    }
    //}
    //    stanceContainer.SetActive(true);
    //    if (combatant is EnemyCombatant)
    //    {
    //        EnemyCombatant enemyCombatant = (EnemyCombatant)combatant;

    //        stanceValue.SetText(enemyCombatant.Stance.CurrentValue.ToString());
    //        if (enemyCombatant.Stance.CurrentValue == 0)
    //        {
    //            stanceValue.SetTextColor(brokenStanceColor);
    //        }
    //        else
    //        {
    //            stanceValue.SetTextColor(defaultStanceColor);
    //        }
    //    }
    //}

    //public void DisplayBarrierChange()
    //{
    //    barrierBar.DisplayChange(combatant.Barrier.CurrentValue);
    //    barrierBar.ResolveChange(combatant.Barrier.CurrentValue);
    //}

    public void ResolveChanges()
    {
        healthBar.ResolveChange(combatant.HP);
        //stance
        //if (combatant is EnemyCombatant)
        //{
        //    EnemyCombatant enemyCombatant = (EnemyCombatant)combatant;
        //    guardPoints.DisplayGuardPoints(enemyCombatant.Guard.CurrentValue);
        //}
    }
}
