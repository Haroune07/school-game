using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    [Header("Input References")]
    public InputActionReference move;
    public InputActionReference sprint;
    public InputActionReference jumpAction;
    public InputActionReference attackAction;
    public InputActionReference rollAction;

    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 10f;
    public float jumpSpeed = 14f;
    public float MaxCoyoteTime = .05f;

    [Tooltip("Higher = snappier movement, Lower = smoother acceleration")]
    public float acceleration = 0.2f;

    [Header("Roll Settings")]
    public float rollSpeed = 15f;
    public float rollCooldown = 1f;
    public string playerLayer = "Player";
    public string enemyAttackLayer = "Attack";

    [Header("Combat Settings")]
    public float attackDashImpulse = 5f;
    public float comboCooldown = 1.2f;
    public GameObject swordCollisionDetector;
    public Transform FireProjectileLauncher;
    public GameObject FireProjectile;

    [Header("Physics Materials")]
    public PhysicsMaterial2D groundMaterial;
    public PhysicsMaterial2D wallMaterial;

    [Header("Animation Settings")]
    public float sprintAnimSpeedScale = 2;
    public string yVelFloat = "YVelocity";
    public string isRunningBool = "IsRunning";
    public string groundedBool = "Grounded";
    public string deathTrigger = "Die";
    public string attackTrigger = "Attack";
    public string wallCollideBool = "WallCollide";
    public string hurtTrigger = "Hurt";
    public string speedMulString = "SpeedMultiplier";
    public string wallJumpTrigger = "WallJump";
    public string rollTrigger = "Roll";
    public string hitCountInt = "HitCount";

    [Header("Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance = .4f;

    [Header("Audio")]
    public AudioClip jumpSound;
    public AudioClip walkSound;
    public AudioClip[] slashSounds = new AudioClip[3];

    [Header("Effects")]
    public ParticleSystem playerWalkTrailParticles;

    // --- Private State Variables ---
    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource audioSource;

    private Vector3 initialScale;
    private Vector2 input;

    private float currentSpeed;
    private float currentRunAnimSpeedScale = 1;
    private float coyoteTime;

    private bool isSprinting;
    private bool jumpPressed;
    private bool wallJumpPressed;

    // Roll State
    private bool isRolling = false;
    private float nextRollTime = 0f;

    // Combat State
    private bool didAttack = false;
    private int hitCount;
    private int currentAttackIndex;
    private float lastHitTime = 0;
    private float nextAttackTime = 0f;

    private const int minHitCount = 1;
    private const int maxHitCount = 3;
    private const float comboWindow = .8f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        initialScale = transform.localScale;

        currentSpeed = walkSpeed;
        coyoteTime = MaxCoyoteTime;

        hitCount = minHitCount;
    }

    void Update()
    {
        input = move.action.ReadValue<Vector2>();

        isSprinting = sprint.action.IsPressed();

        bool grounded = IsGrounded();
        bool hittingWall = IsHittingWall();

        // Jump buffering
        if (jumpAction.action.WasPressedThisFrame())
        {
            jumpPressed = true;
        }

        currentSpeed = isSprinting && grounded
            ? sprintSpeed
            : walkSpeed;

        currentRunAnimSpeedScale = isSprinting
            ? sprintAnimSpeedScale
            : 1;

        // Visual orientation and particles
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
            else if (!grounded)
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

        // Attack logic
        if (attackAction.action.WasPressedThisFrame()
            && Time.time >= nextAttackTime)
        {
            if (Time.time - lastHitTime > comboWindow)
            {
                hitCount = minHitCount;
            }

            currentAttackIndex = hitCount;

            anim.SetInteger(hitCountInt, hitCount);
            anim.SetTrigger(attackTrigger);

            didAttack = true;

            if (hitCount == maxHitCount)
            {
                nextAttackTime = Time.time + comboCooldown;
            }

            IncreaseHitCount();

            lastHitTime = Time.time;
        }

        // Roll logic
        if (rollAction.action.WasPressedThisFrame()
            && Time.time >= nextRollTime
            && !isRolling
            && grounded)
        {
            anim.SetTrigger(rollTrigger);

            isRolling = true;
            nextRollTime = Time.time + rollCooldown;

            // Disable collision between player and enemy attacks (grace frames)
            Physics2D.IgnoreLayerCollision(
                LayerMask.NameToLayer(playerLayer),
                LayerMask.NameToLayer(enemyAttackLayer),
                true
            );
        }

        // Coyote time
        if (grounded)
        {
            coyoteTime = MaxCoyoteTime;
        }
        else
        {
            coyoteTime -= Time.deltaTime;
        }

        // Physics material switching
        if (hittingWall && !grounded)
        {
            rb.sharedMaterial = wallMaterial;

            if (jumpPressed)
            {
                anim.SetTrigger(wallJumpTrigger);
                wallJumpPressed = true;
            }
        }
        else
        {
            rb.sharedMaterial = groundMaterial;
        }

        // Animations
        anim.SetBool(isRunningBool, Mathf.Abs(input.x) > 0.05f);
        anim.SetBool(groundedBool, grounded);
        anim.SetFloat(yVelFloat, rb.linearVelocityY);
        anim.SetBool(wallCollideBool, hittingWall);
        anim.SetFloat(speedMulString, currentRunAnimSpeedScale);
    }

    private void FixedUpdate()
    {
        float sign = Mathf.Sign(transform.localScale.x);

        if (isRolling)
        {
            rb.linearVelocity = new Vector2(
                sign * rollSpeed,
                rb.linearVelocity.y
            );
        }
        else
        {
            float targetVelX = input.x * currentSpeed;

            // Lerp gradually moves current velocity toward target velocity.
            // This creates smoother acceleration/deceleration and allows
            // moving platforms to naturally transfer momentum to the player.
            float smoothVelX = Mathf.Lerp(
                rb.linearVelocity.x,
                targetVelX,
                acceleration
            );

            rb.linearVelocity = new Vector2(
                smoothVelX,
                rb.linearVelocity.y
            );

            if (jumpPressed)
            {
                if (IsGrounded()
                    || coyoteTime > 0
                    || wallJumpPressed)
                {
                    if (audioSource != null && jumpSound != null)
                    {
                        audioSource.PlayOneShot(jumpSound);
                    }

                    if (rb.linearVelocityY < 15)
                    {
                        rb.AddForce(
                            Vector2.up * jumpSpeed,
                            ForceMode2D.Impulse
                        );
                    }
                }

                jumpPressed = false;
                wallJumpPressed = false;
            }

            if (didAttack && IsGrounded())
            {
                rb.AddForce(
                    new Vector2(sign, 0) * attackDashImpulse,
                    ForceMode2D.Impulse
                );

                didAttack = false;
            }
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            groundCheck.position,
            Vector2.down,
            groundCheckDistance
        );

        foreach (var hit in hits)
        {
            if (hit.collider != null && !hit.collider.isTrigger && !hit.transform.IsChildOf(transform))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsHittingWall()
    {
        float sign = Mathf.Sign(transform.localScale.x);

        Vector2 dir = new Vector2(sign, 0);

        RaycastHit2D[] hits = Physics2D.RaycastAll(
            wallCheck.position,
            dir,
            wallCheckDistance
        );

        foreach (var hit in hits)
        {
            if (hit.collider != null && !hit.collider.isTrigger && !hit.transform.IsChildOf(transform))
            {
                return true;
            }
        }

        return false;
    }

    private void IncreaseHitCount()
    {
        hitCount = (hitCount % maxHitCount) + minHitCount;
    }

    private void SpawnProjectile()
    {
        float lookDir = transform.localScale.x > 0
            ? 0
            : 180;

        Instantiate(
            FireProjectile,
            FireProjectileLauncher.position,
            Quaternion.Euler(0, lookDir, 0)
        );
    }

    private void PlayhitSound()
    {
        if (currentAttackIndex == maxHitCount)
        {
            audioSource.pitch = 0.9f;
        }
        else
        {
            audioSource.pitch = UnityEngine.Random.Range(
                0.95f,
                1.05f
            );
        }

        audioSource.PlayOneShot(
            slashSounds[currentAttackIndex - 1]
        );

        audioSource.pitch = 1f;
    }

    private void TurnOnAttackCollision()
    {
        swordCollisionDetector.SetActive(true);
    }

    private void TurnOffAttackCollision()
    {
        swordCollisionDetector.SetActive(false);
    }

    private void PlayFootstepSound()
    {
        if (IsGrounded() && walkSound != null)
        {
            audioSource.PlayOneShot(walkSound);
        }
    }

    public void FinishRoll()
    {
        isRolling = false;

        // Re-enable collision when roll animation ends
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer(playerLayer),
            LayerMask.NameToLayer(enemyAttackLayer),
            false
        );
    }
}