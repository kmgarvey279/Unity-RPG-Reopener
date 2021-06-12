using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Tilemaps;

[System.Serializable]
public class BattleStats
{
    public string name;
    public DynamicStat health;
    public DynamicStat mana;
    public Dictionary<StatType, Stat> statDict;
    public Dictionary<AttackProperty, int> resistDict;

    public List<Action> skills;
    public Action meleeAttack;
    public Action rangedAttack;
    //list of status effects
    //alive/dead?

    public BattleStats(CharacterInfo characterInfo)
    {
        this.name = characterInfo.name;
        this.health = characterInfo.health;
        this.mana = characterInfo.mana;
        this.statDict = characterInfo.statDict;
        this.resistDict = characterInfo.resistDict;

        this.meleeAttack = characterInfo.meleeAttack;
        this.rangedAttack = characterInfo.rangedAttack;
        this.skills = characterInfo.skills;
    }
}

public class Combatant : MonoBehaviour
{
    [Header("Character Stats")]
    public CharacterInfo characterInfo;
    public BattleStats battleStats;
    [Header("Game Object Components")]
    public Rigidbody2D rigidbody;
    public Animator animator;
    [Header("Child Scripts")]
    public HealthDisplay healthDisplay;
    public TargetIcon targetIcon;
    [Header("Events")]
    public SignalSender onMoveComplete;
    [Header("Grid")]
    public Tile tile;
    public List<Tile> path = new List<Tile>();

    public virtual void Start()
    {
        battleStats = new BattleStats(characterInfo);

        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        healthDisplay = GetComponentInChildren<HealthDisplay>();
        targetIcon = GetComponentInChildren<TargetIcon>();

        SnapToTileCenter();
    }

    public void SnapToTileCenter()
    {
        Tilemap tilemap = GetComponentInParent<Battlefield>().gridManager.tilemap;
        
        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
        Vector3 newPosition = tilemap.GetCellCenterWorld(cellPosition);
        // newPosition.y = newPosition.y - 0.25f;   
        transform.position = newPosition;
    }

    public int GetStatValue(StatType statType)
    {
        return battleStats.statDict[statType].GetValue();
    }

    public void SetTile(Tile tile)
    {
        this.tile = tile;
    }

    public void Move(Tile destination)
    {
        GridManager gridManager = GetComponentInParent<Battlefield>().gridManager;
        path = gridManager.GetPath(tile, destination);
        animator.SetBool("Moving", true);
    }

    // Update is called once per frame
    private void Update()
    {
        float speed = 5f;
        if(path.Count > 0)
        {
            if(Vector3.Distance(transform.position, path[0].transform.position) < 0.0001f)
            {
                path.RemoveAt(0);
                if(path.Count == 0)
                {
                    animator.SetBool("Moving", false);
                    onMoveComplete.Raise();
                }
            }
            else
            {
                Vector3 moveDirection = (path[0].transform.position - transform.position).normalized;
                if(!Mathf.Approximately(moveDirection.x, 0.0f) || !Mathf.Approximately(moveDirection.y, 0.0f))
                {
                    animator.SetFloat("Look X", Mathf.Round(moveDirection.x));
                    animator.SetFloat("Look Y", Mathf.Round(moveDirection.y));
                }
                float step =  speed * Time.deltaTime; 
                transform.position = Vector3.MoveTowards(transform.position, path[0].transform.position, step);   
            }
        }
    }

    public void TakeDamage(float rawDamage)
    {
        int finalDamage = Mathf.RoundToInt((rawDamage - (float)battleStats.statDict[StatType.Defense].GetValue()) * Random.Range(0.85f, 1f));
        battleStats.health.ChangeCurrentValue(-finalDamage);
        healthDisplay.HandleHealthChange(DamagePopupType.Damage, finalDamage);
        
        if(battleStats.health.GetCurrentValue() <= 0)
        {
            Debug.Log("Dead");
        }
    }
}
