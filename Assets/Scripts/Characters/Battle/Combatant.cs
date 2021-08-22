using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;
using UnityEngine.Tilemaps;

[System.Serializable]
public class BattleStats
{
    public string characterName;
    public DynamicStat health;
    public DynamicStat mana;
    public Dictionary<StatType, Stat> statDict;
    public Dictionary<AttackProperty, int> resistDict;

    public List<Action> skills;
    public Action attack1;
    public Action attack2;
    //list of status effects
    //alive/dead?

    public BattleStats(CharacterInfo characterInfo)
    {
        this.characterName = characterInfo.characterName;
        this.health = characterInfo.health;
        this.mana = characterInfo.mana;
        this.statDict = characterInfo.statDict;
        this.resistDict = characterInfo.resistDict;

        this.attack1 = characterInfo.attack1;
        this.attack2 = characterInfo.attack2;
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
    public GameObject spriteFill;
    [Header("Child Scripts")]
    public HealthDisplay healthDisplay;
    public MaskController maskController;
    [Header("Events")]
    public SignalSender onMoveComplete;
    public SignalSenderGO onTargetSelect;
    public SignalSenderGO onTargetDeselect;
    [Header("Grid")]
    public Tile tile;
    public List<Tile> path = new List<Tile>();
    [Header("Coroutine Timers")]
    public float recoveryTime = 1f;
    public float koTime = 1f;

    public virtual void Awake()
    {
        battleStats = new BattleStats(characterInfo);

        rigidbody = GetComponent<Rigidbody2D>();

        healthDisplay = GetComponentInChildren<HealthDisplay>();
        maskController = GetComponentInChildren<MaskController>();
    }

    public void Start()
    {
        SnapToTileCenter();
    }

    public void SnapToTileCenter()
    {
        Tilemap tilemap = GetComponentInParent<Battlefield>().gridManager.tilemap;
        
        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
        Vector3 newPosition = tilemap.GetCellCenterWorld(cellPosition);
        transform.position = newPosition;
    }

    public int GetStatValue(StatType statType)
    {
        return battleStats.statDict[statType].GetValue();
    }

    public Vector2 GetDirection()
    {
        return new Vector2(animator.GetFloat("Look X"), animator.GetFloat("Look Y"));
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

    public void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        animator.SetFloat("Look X", direction.x);
        animator.SetFloat("Look Y", direction.y);
    }

    private void Update()
    {
        float speed = 4f;
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
                float step = speed * Time.deltaTime; 
                transform.position = Vector3.MoveTowards(transform.position, path[0].transform.position, step);   
            }
        }
    }

    public void TakeDamage(float rawDamage)
    {
        //switch to damage animation
        animator.SetBool("Hurt", true);
        //get final damage amount
        int finalDamage = Mathf.RoundToInt((rawDamage - (float)battleStats.statDict[StatType.Defense].GetValue()) * Random.Range(0.85f, 1f));
        finalDamage = Mathf.Clamp(finalDamage, 1, 9999);
        //update health
        battleStats.health.ChangeCurrentValue(-finalDamage);
        //display damage + new health
        healthDisplay.HandleHealthChange(DamagePopupType.Damage, finalDamage);
        //ko target if hp <= 0
        if(battleStats.health.GetCurrentValue() <= 0)
        {
            StartCoroutine(KO());
        }
        else 
        {
            StartCoroutine(Recover());
        }
    }

    private IEnumerator Recover()
    {
        yield return new WaitForSeconds(recoveryTime);
        animator.SetBool("Hurt", false);
    }

    private IEnumerator KO()
    {
        yield return new WaitForSeconds(koTime);
        Debug.Log("Dead");
    }

    public void Select()
    {
        maskController.TriggerSelected();
        onTargetSelect.Raise(this.gameObject);
        healthDisplay.ToggleBarVisibility(true);
    }

    public void Deselect()
    {
        maskController.EndAnimation();
        onTargetDeselect.Raise(this.gameObject);
        healthDisplay.ToggleBarVisibility(false);
    }

    public void GrayOut()
    {
        maskController.TriggerUnselectable();
    }

    public void ClearSpriteMask()
    {
        maskController.EndAnimation();
    }
}
