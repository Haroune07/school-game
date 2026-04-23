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
    public InputActionReference attackAction;


    [Header("Animation Settings")]
    public string yVelFloat = "YVelocity";
    public string isRunningBool = "IsRunning";
    public string groundedBool = "Grounded";
    public string deathTrigger = "Die";
    public string attackTrigger = "Attack";
    public string wallCollideBool = "WallCollide";
    public string hurtTrigger = "Hurt";
    public string speedMulString = "SpeedMultiplier";
    public float sprintAnimSpeedScale = 2;

    [Header("Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance = .2f;
    [Header("Audio")]
    public AudioClip jumpSound;
    
    [Header("Effects")]
    public ParticleSystem playerWalkTrailParticles;

    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource audioSource;
    private Vector3 initialScale;
    private Vector2 input;
    private float currentSpeed;
    private bool isSprinting;
    private bool jumpPressed;
    private float currentRunAnimSpeedScale = 1;

    private float coyoteTime;
    public float MaxCoyoteTime = .01f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        initialScale = transform.localScale;
        currentSpeed = walkSpeed;
        coyoteTime = MaxCoyoteTime;
    }

    void Update()
    {
        input = move.action.ReadValue<Vector2>();
        isSprinting = sprint.action.IsPressed();
        bool grounded = IsGrounded();

        if (jumpAction.action.WasPressedThisFrame())
        {
            jumpPressed = true;
        }

        currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        currentRunAnimSpeedScale = isSprinting ? sprintAnimSpeedScale : 1;

        if (input.x != 0)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(input.x) * Mathf.Abs(initialScale.x),
                initialScale.y,
                initialScale.z
            );

            if (!playerWalkTrailParticles.isPlaying && grounded)
            {
                playerWalkTrailParticles.Play();
            }
            else if(!grounded)
            {
                playerWalkTrailParticles.Stop();
            }
        }
        else
        {
            if (playerWalkTrailParticles.isPlaying)
            {
                playerWalkTrailParticles.Stop();
            }
        }

        if (attackAction.action.WasPressedThisFrame()) anim.SetTrigger(attackTrigger);

        if (grounded)
        {
            coyoteTime = MaxCoyoteTime;
        }
        else
        {
            coyoteTime -= Time.deltaTime;
        }
        
        bool hittingWall = IsHittingWall();
        anim.SetBool(isRunningBool, Mathf.Abs(input.x) > 0.05f);
        anim.SetBool(groundedBool, grounded);
        anim.SetFloat(yVelFloat, rb.linearVelocityY);
        anim.SetBool(wallCollideBool, hittingWall);
        anim.SetFloat(speedMulString, currentRunAnimSpeedScale);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(input.x * currentSpeed, rb.linearVelocity.y);

        if (jumpPressed)
        {
            if (IsGrounded() || coyoteTime > 0)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("InstantDeath")){
            anim.SetTrigger(hurtTrigger);
        }
    }

    private bool IsHittingWall()
    {
        Vector2 dir = new Vector2(Mathf.Sign(transform.localScale.x), 0);
        return Physics2D.Raycast(wallCheck.position, dir, wallCheckDistance);
    }
}
