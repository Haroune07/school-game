using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 10f;
    public float jumpSpeed = 14f;

    [Header("Input References")]
    public InputActionReference move;
    public InputActionReference sprint;
    public InputActionReference jumpAction;
    public InputActionReference hitAction;


    [Header("Animation Settings")]
    public string yVelFloat = "YVelocity";
    public string isRunningBool = "IsRunning";
    public string groundedBool = "Grounded";
    public string hitTrigger = "Hit";

    [Header("Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.2f;

    [Header("Audio")]
    public AudioClip jumpSound;

    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource audioSource;
    private Vector3 initialScale;
    private Vector2 input;
    private float currentSpeed;
    private bool isSprinting;
    private bool jumpPressed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        initialScale = transform.localScale;
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        input = move.action.ReadValue<Vector2>();
        isSprinting = sprint.action.IsPressed();

        if (jumpAction.action.WasPressedThisFrame())
        {
            jumpPressed = true;
        }

        currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        if (input.x != 0)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(input.x) * Mathf.Abs(initialScale.x),
                initialScale.y,
                initialScale.z
            );
        }

        bool grounded = IsGrounded();
        anim.SetBool(isRunningBool, Mathf.Abs(input.x) > 0.05f);
        anim.SetBool(groundedBool, grounded);
        anim.SetFloat(yVelFloat, rb.linearVelocityY);
        if (hitAction.action.WasPressedThisFrame())
            anim.SetTrigger(hitTrigger);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(input.x * currentSpeed, rb.linearVelocity.y);

        if (jumpPressed)
        {
            if (IsGrounded())
            {
                if (audioSource != null && jumpSound != null)
                {
                    audioSource.PlayOneShot(jumpSound);
                }

                rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            }

            jumpPressed = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance);
    }
}
