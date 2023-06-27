using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using Pathfinding;

public class OverworldEnemy : MonoBehaviour
{
    public OverworldData overworldData;
    public Vector3 lookDirection;
    public float wanderSpeed;
    public float chaseSpeed;
    public GameObject player;
    [SerializeField] private GameObject alertIcon;
    // public float visionRadius;
    // public float visionAngle = 5.0f;
    [Header("GameObject Components")]
    public Animator animator;
    [Header("A* Pathfinding")]
    public AIPath aiPath;
    public AIDestinationSetter setter;

    public enum EnemyState
    {
        Wander,
        Chase
    }
    public EnemyState currentState;

    public void Start()
    {   
        lookDirection = new Vector3(0, -1, 0);

        animator = GetComponent<Animator>();
        
        aiPath = GetComponent<AIPath>();
        setter = GetComponent<AIDestinationSetter>();

        currentState = EnemyState.Wander;
        aiPath.maxSpeed = wanderSpeed;

        StartCoroutine(SetDestination());
    }

    public void Update()
    {
        if(player != null)
        {
            if(setter.target != player.transform)
            {
                setter.target = player.transform;
            }
            if(currentState != EnemyState.Chase)
            {
                currentState = EnemyState.Chase;
                aiPath.maxSpeed = chaseSpeed;
            } 
            return;
        }
        if(aiPath.canMove)
        {
            if(!Mathf.Approximately(aiPath.desiredVelocity.x, 0.0f) || !Mathf.Approximately(aiPath.desiredVelocity.y, 0.0f))
            {
                lookDirection = aiPath.desiredVelocity;
                animator.SetFloat("Look X", Mathf.Round(lookDirection.x));
                animator.SetFloat("Look Y", Mathf.Round(lookDirection.y));
            }
        
            if(aiPath.reachedEndOfPath)
            {
                StartCoroutine(SetDestination());
            }
        }
    }

    private IEnumerator SetDestination()
    {
        float delay = Random.Range(0.5f, 2f);

        float speedTemp = aiPath.maxSpeed;
        aiPath.canMove = false;

        aiPath.destination = Random.insideUnitSphere * 5;

        yield return new WaitForSeconds(delay);
        aiPath.canMove = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(AlertCo());
            player = other.gameObject;
        }
    }

    private IEnumerator AlertCo()
    {
        alertIcon.SetActive(true);
        yield return new WaitForSeconds(1f);
        alertIcon.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            player = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Battle Start");
            overworldData.lockInput = true;
            StartCoroutine(LoadBattleCo());
        }
    }

    private IEnumerator LoadBattleCo()
    {
        // onScreenFadeOut.Raise();
        SceneManager.LoadScene("SampleBattleScene");
        yield return new WaitForSeconds(1f); 
        Scene scene = SceneManager.GetSceneByName("SampleBattleScene");
        SceneManager.SetActiveScene(scene);
    }

    // private void OnDrawGizmos()
    // {
    //     Handles.color = Color.blue;
    //     Handles.DrawWireArc(transform.position, lookDirection, transform.position, visionAngle, visionRadius); 
    // }

}
