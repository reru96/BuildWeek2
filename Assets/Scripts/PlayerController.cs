using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float laneDistance = 3f;
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float slideDuration = 0.5f;

    private CharacterController controller;
    private Vector3 moveDirection;
    private int currentLane = 1;
    private float verticalVelocity;
    private bool isSliding;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal < 0 && currentLane > 0)
        {
            currentLane--;
        }
        else if (horizontal > 0 && currentLane < 2)
        {
            currentLane++;
        }

        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if (currentLane == 0) targetPosition += Vector3.left * laneDistance;
        if (currentLane == 2) targetPosition += Vector3.right * laneDistance;

        Vector3 diff = targetPosition - transform.position;
        Vector3 move = diff.normalized * moveSpeed;
        moveDirection.x = move.x;

        if (controller.isGrounded)
        {
            verticalVelocity = -0.1f;

            if (Input.GetKeyDown(KeyCode.W))
            {
                verticalVelocity = jumpForce;
            }

            if (Input.GetKeyDown(KeyCode.S) && !isSliding)
            {
                StartCoroutine(Slide());
            }
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        moveDirection.y = verticalVelocity;
        moveDirection.z = moveSpeed;

        controller.Move(moveDirection * Time.deltaTime);
    }

    System.Collections.IEnumerator Slide()
    {
        isSliding = true;
        controller.height = controller.height / 2;
        yield return new WaitForSeconds(slideDuration);
        controller.height = controller.height * 2;
        isSliding = false;
    }
}
