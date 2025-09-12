using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenatoPlayerController : MonoBehaviour
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
    }

    void Update()
    {
        // ---- INPUT LATERALE ----
        if (Input.GetKeyDown(KeyCode.A)) MoveLane(-1);
        if (Input.GetKeyDown(KeyCode.D)) MoveLane(1);

        // ---- POSIZIONE TARGET ----
        float midLane = (totalLanes - 1) / 2f;
        float targetX = (currentLane - midLane) * laneWidth;
        Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        // movimento fluido verso la corsia
        float diffX = targetPosition.x - transform.position.x;
        moveDirection.x = diffX * laneChangeSpeed;

        // ---- JUMP & SLIDE ----
        if (controller.isGrounded && !isSliding)
        {
            verticalVelocity = -1f;

            if (Input.GetKeyDown(KeyCode.W)) // salto
                verticalVelocity = jumpForce;

            else if (Input.GetKeyDown(KeyCode.S)) // scivolata
                StartCoroutine(Slide());
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        moveDirection.y = verticalVelocity;
        moveDirection.z = forwardSpeed;

        // ---- MUOVO IL PLAYER ----
        controller.Move(moveDirection * Time.deltaTime);
    }

    void MoveLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, 0, totalLanes - 1);
    }

    public IEnumerator Slide()
    {
        isSliding = true;
        float originalHeight = controller.height;
        Vector3 originalCenter = controller.center;

        controller.height = originalHeight / 2;
        controller.center = new Vector3(originalCenter.x, originalCenter.y - originalHeight / 4, originalCenter.z);

        yield return new WaitForSeconds(slideDuration);

        controller.height = originalHeight;
        controller.center = originalCenter;
        isSliding = false;
    }
}
