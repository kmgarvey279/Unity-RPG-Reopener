using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{    
    [Header("Movement/Collision")]
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    private Vector2 moveDirection;
    private bool isRunning;
    private readonly float walkSpeed = 6f;
    private readonly float runSpeed = 20f;
    private bool isSwitchingRooms;
    
    [Header("Sprite")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite;

    [Header("Interactable")]
    private PlayerInteractionController playerInteractionController;
    private PlayerItemPickupController playerItemPickupController;

    [Header("Game state info")]
    [SerializeField] private OverworldData overworldData;

    [Header("Sound FX")]
    [SerializeField] private AudioClip step1SFX;
    [SerializeField] private AudioClip step2SFX;

    [Header("Signals")]
    [SerializeField] private SignalSender OnChangeRoomStart;
    [SerializeField] private SignalSender OnChangeRoomEnd;

    private WaitForSeconds wait005 = new WaitForSeconds(0.05f);
    private WaitForSeconds wait025 = new WaitForSeconds(0.25f);
    private WaitForSeconds wait05 = new WaitForSeconds(0.5f);

    private void OnEnable()
    {
        InputManager.Instance.OnPressMove.AddListener(Move);
        InputManager.Instance.OnPressInteract.AddListener(Interact);
        InputManager.Instance.OnPressRun.AddListener(Run);
        //InputManager.onReleaseRun.AddListener(RunStop);
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPressMove.RemoveListener(Move);
        InputManager.Instance.OnPressInteract.RemoveListener(Interact);
        InputManager.Instance.OnPressRun.RemoveListener(Run);
    }   

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        playerItemPickupController = GetComponent<PlayerItemPickupController>();
        playerInteractionController = GetComponentInChildren<PlayerInteractionController>();
        moveDirection = new Vector3(0, 0);

        StartCoroutine(TriggerWalkSFX());
    }

    public void SetDirection(Vector2 newDirection)
    {
        animator.SetFloat("Look X", newDirection.x);
        animator.SetFloat("Look Y", newDirection.y);
    }

    private void Update()
    {
        if (InputManager.Instance.InputLocked)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (moveDirection.x != 0 || moveDirection.y != 0)
        {
            SetDirection(moveDirection);
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
        if (isRunning)
        {
            rb.velocity = moveDirection * runSpeed;
        }
        else
        {
            rb.velocity = moveDirection * walkSpeed;
        }
    }

    private void Move(Vector2 vectorInput)
    {
        moveDirection.Set(vectorInput.x, vectorInput.y);
    }

    private void Run(bool isPressed)
    {
        isRunning = isPressed;
        Debug.Log("Is running: " + isRunning);
    }

    private IEnumerator TriggerWalkSFX()
    {
        bool isEvenTick = false;
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        while (true)
        {
            if (moveDirection.x != 0 || moveDirection.y != 0)
            {
                if (isEvenTick)
                {
                    SoundFXManager.Instance.PlayClipInWorld(step2SFX, transform, 0.5f);

                }
                else
                {
                    SoundFXManager.Instance.PlayClipInWorld(step1SFX, transform, 0.5f);

                }
                isEvenTick = !isEvenTick;
                yield return wait05;
            }
            yield return wait;
        }

        yield return null;
    }

    private void Interact(bool isPressed)
    {
        if (playerInteractionController.Interactable != null)
        {
            if (playerInteractionController.Interactable.FaceOnInteract)
            {
                SetDirection((playerInteractionController.Interactable.transform.position - transform.position).normalized);
            }
            playerInteractionController.Interactable.Interact();
        }
    }

    public void ChangeRoom()
    {

    }

    public IEnumerator ChangeRoomCo(Vector2 direction)
    {
        if (!isSwitchingRooms)
        {
            isSwitchingRooms = true;
            InputManager.Instance.LockInput();

            SetDirection(direction);
            animator.SetBool("Walking", true);

            yield return wait025;

            Vector3 destination = new Vector3(transform.position.x + direction.x, transform.position.y + direction.y);
            Vector3 start = transform.position;
            float moveDuration = 0.25f;
            float timer = 0;
            while (transform.position != destination)
            {
                transform.position = Vector3.Lerp(start, destination, timer / moveDuration);
                timer += Time.deltaTime;

                yield return null;
            }
            animator.SetBool("Walking", false);

            InputManager.Instance.UnlockInput();
            isSwitchingRooms = false;
        }
    }

    public void OnPauseStart()
    {
    }

    public void OnPauseEnd()
    {
    }
}