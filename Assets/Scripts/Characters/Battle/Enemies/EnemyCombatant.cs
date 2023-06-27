using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatant : Combatant
{
    public List<Action> lastActions = new List<Action>();
    public bool IsBoss { get; private set; } = false;
    [field: SerializeField] public List<WeightedAction> WeightedActions { get; private set; } = new List<WeightedAction>();

    protected override void Awake()
    {
        base.Awake();
        CombatantType = CombatantType.Enemy;   
    }

    public override void SetCharacterData(CharacterInfo characterInfo, PlayableCharacterID linkedCharacterID = PlayableCharacterID.None)
    {
        base.SetCharacterData(characterInfo);
        EnemyInfo enemyInfo = (EnemyInfo)characterInfo;
        WeightedActions = enemyInfo.WeightedActions;
    }

    public override void OnKO()
    {
        base.OnKO();
        Tile.UnassignOccupier();
        StartCoroutine(DestroyEnemyCo());
    }

    private IEnumerator DestroyEnemyCo()
    {
        yield return wait1;
        Destroy(gameObject);
    }
}
