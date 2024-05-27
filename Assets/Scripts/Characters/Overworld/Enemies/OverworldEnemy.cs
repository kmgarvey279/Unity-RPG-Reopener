using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using Pathfinding;

public enum EmoteType
{
    Exclamation,
    Question
}


public class OverworldEnemy : MonoBehaviour
{
    private Transform target;
    private NPCMovement enemyMovement;
    [SerializeField] private GameObject battleTrigger;
    [SerializeField] private GameObject chaseTrigger;
    [SerializeField] private Animator spriteAnimator;
    public LayerMask layerToCheck;

    [SerializeField] private GameObject exclamationEmote;
    [SerializeField] private GameObject questionEmote;
    private Dictionary<EmoteType, GameObject> emotes;
    // public float visionRadius;
    // public float visionAngle = 5.0f;

    //[SerializeField] private Animator animator;
    //private AIPath aiPath;
    //private AIDestinationSetter setter;
    //private Seeker seeker;

    private EnemyContainer enemyContainer;
    private bool canInteract = true;


    public void Awake()
    {
        enemyMovement = GetComponent<NPCMovement>();
        enemyContainer = GetComponentInParent<EnemyContainer>();
        emotes = new Dictionary<EmoteType, GameObject>()
        { 
            { EmoteType.Exclamation, exclamationEmote },
            { EmoteType.Question, questionEmote }
        };
    }

    private void OnEnable()
    {
        if (!canInteract)
        {
            spriteAnimator.SetTrigger("FlashOn");
        }
    }

    private void OnDisable()
    {
        foreach(KeyValuePair<EmoteType, GameObject> entry in emotes)
        {
            entry.Value.SetActive(false);
        }
    }

    public void OnPlayerEnterPursuitRadius(Collider2D other)
    {
        if (target == null)
        {
            target = other.gameObject.transform;
        }

        if (other && enemyMovement.CurrentState != NPCMovement.NPCMoveState.Chase)
        {
            Vector2 directionToCheck = target.position - transform.position;
            float rangeToCheck = 5f;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToCheck, rangeToCheck, layerToCheck);
            Debug.DrawRay(transform.position, directionToCheck, Color.red);
            if (hit.collider && hit.collider.CompareTag("PlayerInteract"))
            {

                Debug.DrawRay(transform.position, directionToCheck, Color.yellow);
                StartCoroutine(DisplayEmote(EmoteType.Exclamation));
                enemyMovement.SwitchToChaseState(target);
            }
        }
    }

    public void OnPlayerExitPursuitRadius(Collider2D other)
    {
        if (enemyMovement.CurrentState == NPCMovement.NPCMoveState.Chase)
        {
            Debug.Log("end chase");
            StartCoroutine(DisplayEmote(EmoteType.Question));
            enemyMovement.ReturnToStartPoint();
        }
    }

    public void OnPlayerEnterBattleTriggerRadius()
    {
        Debug.Log("trigger battle");
        SceneSetupManagerOverworld sceneSetupManager = FindObjectOfType<SceneSetupManagerOverworld>();
        StartCoroutine(sceneSetupManager.OnEnterBattle(enemyContainer));
    }

    private IEnumerator DisplayEmote(EmoteType emoteType)
    {
        emotes[emoteType].SetActive(true);
        yield return new WaitForSeconds(1f);
        emotes[emoteType].SetActive(false);
    }

    public void DisableInteractions()
    {
        canInteract = false;
        spriteAnimator.SetTrigger("FlashOn");

        enemyMovement.LockMovement(true);
        battleTrigger.SetActive(false);
        chaseTrigger.SetActive(false);
    }

    public void EnableInteractions()
    {
        canInteract = true;
        spriteAnimator.SetTrigger("FlashOff");

        enemyMovement.LockMovement(false);
        battleTrigger.SetActive(true);
        chaseTrigger.SetActive(true);
    }
}
