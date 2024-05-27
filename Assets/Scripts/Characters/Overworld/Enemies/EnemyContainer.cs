using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContainer : MonoBehaviour
{
    [field: SerializeField] public GuidWrapper EnemyInstanceID { get; private set; }
    [field: SerializeField] public EnemyPartyData EnemyPartyData { get; private set; }
    public OverworldEnemy OverworldEnemy { get; private set; }

    public void Awake()
    {
        OverworldEnemy = GetComponentInChildren<OverworldEnemy>();
    }

    public void ActivateEnemy(bool isActive)
    {
        //bool wasDefeated = SaveManager.Instance.SessionData.DefeatedEnemyIDs.Contains(EnemyInstanceID.ToString());
        //if (isActive && !wasDefeated)
        //{
        //    OverworldEnemy.gameObject.SetActive(true);
        //}
        Debug.Log(OverworldEnemy.name + " active is now " + isActive);
        OverworldEnemy.gameObject.SetActive(isActive);
    }
}
