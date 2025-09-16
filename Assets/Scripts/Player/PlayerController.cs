using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]private float forwardSpeed = 10f;
    [SerializeField]private float laneChangeSpeed = 10f;
    [SerializeField]private float jumpForce = 8f;
    [SerializeField]private float gravity = -20f;
    [SerializeField]private float slideDuration = 0.7f;

    private CharacterController controller;
    private Vector3 moveDirection;

    private int currentLane;
    private int totalLanes;
    private float laneWidth;

    private float verticalVelocity;
    private bool isSliding;

    public bool IsSliding => isSliding;
    public float ForwardSpeed => forwardSpeed;
    public float JumpForce{ get => jumpForce;  set => jumpForce = value;}
    public bool IsGrounded => controller.isGrounded;
    public int CurrentLane => currentLane;

    public AnimationState CurrentState { get; private set; } = AnimationState.RUN;
    void Start()
    {
        controller = GetComponent<CharacterController>();

        //  prendi il numero di corsie dal LVLBuilder
        LVLBuilder builder = FindObjectOfType<LVLBuilder>();
        if (builder != null)
        {
            totalLanes = builder.NumberOfLanes;
            laneWidth = builder.LaneWidth;
        }
        else
        {
            Debug.LogWarning("LVLBuilder non trovato, default a 3 corsie");
            totalLanes = 3;
            laneWidth = 3;
        }

        // inizia al centro
        currentLane = totalLanes / 2;
        CurrentState = AnimationState.RUN;
    }

    void Update()
    {
        HandleInput();
        HandleMovement();
        UpdateState();
    }
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A)) MoveLane(-1);
        if (Input.GetKeyDown(KeyCode.D)) MoveLane(1);

        if (controller.isGrounded && !isSliding)
        {
            if (Input.GetKeyDown(KeyCode.W))
                verticalVelocity = jumpForce;
            else if (Input.GetKeyDown(KeyCode.S))
                StartCoroutine(Slide());
        }
    }

    void HandleMovement()
    {
     
        float midLane = (totalLanes - 1) / 2f;
        float targetX = (currentLane - midLane) * laneWidth;
        Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

     
        float diffX = targetPosition.x - transform.position.x;
        moveDirection.x = diffX * laneChangeSpeed;

      
        if (controller.isGrounded && !isSliding && verticalVelocity <= 0f)
            verticalVelocity = -1f;
        else
            verticalVelocity += gravity * Time.deltaTime;

        moveDirection.y = verticalVelocity;
        moveDirection.z = forwardSpeed;

      
        controller.Move(moveDirection * Time.deltaTime);
    }

    void MoveLane(int direction)
    {
        int previousLane = currentLane;
        currentLane = Mathf.Clamp(currentLane + direction, 0, totalLanes - 1);

        if (currentLane < previousLane) CurrentState = AnimationState.MOVELEFT;
        else if (currentLane > previousLane) CurrentState = AnimationState.MOVERIGHT;
    }

    public IEnumerator Slide()
    {
        isSliding = true;
        CurrentState = AnimationState.SLIDE;
        float originalHeight = controller.height;
        Vector3 originalCenter = controller.center;

        controller.height = originalHeight / 2;
        controller.center = new Vector3(originalCenter.x, originalCenter.y - originalHeight / 4, originalCenter.z);

        yield return new WaitForSeconds(slideDuration);

        controller.height = originalHeight;
        controller.center = originalCenter;
        isSliding = false;
        CurrentState = AnimationState.RUN;
    }
    void UpdateState()
    {
  
        if (!isSliding && controller.isGrounded && Mathf.Approximately(moveDirection.x, 0f))
            CurrentState = AnimationState.RUN;

        if (!controller.isGrounded && verticalVelocity > 0)
            CurrentState = AnimationState.JUMP;
    }

}
