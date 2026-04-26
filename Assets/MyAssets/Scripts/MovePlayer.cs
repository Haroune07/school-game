using NUnit.Framework;
using System;
using System.Collections.Generic;
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

    [Header("Roll Settings")]
    public float rollSpeed = 15f;
    public float rollCooldown = 1f;

    [Header("Combat Settings")]
    public float attackDashImpulse = 5f;
    public float comboCooldown = 1.2f;
    public GameObject swordCollisionDetector;
    public Transform FireProjectileLauncher;
    public GameObject FireProjectile;

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

        if (jumpAction.action.WasPressedThisFrame())
        {
            jumpPressed = true;
        }

        currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        currentRunAnimSpeedScale = isSprinting ? sprintAnimSpeedScale : 1;

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
        if (attackAction.action.WasPressedThisFrame() && Time.time >= nextAttackTime)
        {
            if (Time.time - lastHitTime > comboWindow)
            {
                hitCount = minHitCount;
            }

            currentAttackIndex = hitCount;

            anim.SetInteger(hitCountInt, hitCount);
            anim.SetTrigger(attackTrigger);
            didAttack = true;

            // Trigger combo cooldown if we reached the last hit
            if (hitCount == maxHitCount)
            {
                nextAttackTime = Time.time + comboCooldown;
            }

            IncreaseHitCount();
            lastHitTime = Time.time;
        }

        // Roll logic
        if (rollAction.action.WasPressedThisFrame() && Time.time >= nextRollTime && !isRolling && grounded)
        {
            anim.SetTrigger(rollTrigger);
            isRolling = true;
            nextRollTime = Time.time + rollCooldown;
        }

        // Timers and friction
        if (grounded)
        {
            coyoteTime = MaxCoyoteTime;
        }
        else
        {
            coyoteTime -= Time.deltaTime;
        }

        if (hittingWall)
        {
            rb.sharedMaterial.friction = 4f;

            if (jumpPressed)
            {
                anim.SetTrigger(wallJumpTrigger);
                wallJumpPressed = true;
            }
        }
        else
        {
            rb.sharedMaterial.friction = 0;
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
            rb.linearVelocity = new Vector2(sign * rollSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(input.x * currentSpeed, rb.linearVelocity.y);

            if (jumpPressed)
            {
                if (IsGrounded() || coyoteTime > 0 || wallJumpPressed)
                {
                    if (audioSource != null && jumpSound != null)
                    {
                        audioSource.PlayOneShot(jumpSound);
                    }

                    if (rb.linearVelocityY < 15)
                        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                }

                jumpPressed = false;
                wallJumpPressed = false;
            }

            if (didAttack)
            {
                rb.AddForce(new Vector2(sign, 0) * attackDashImpulse, ForceMode2D.Impulse);
                didAttack = false;
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("InstantDeath"))
        {
            isRolling = false; // Forcefully cancel roll to prevent sliding while dying
            anim.SetTrigger(hurtTrigger);
        }
    }

    private bool IsHittingWall()
    {
        float sign = Mathf.Sign(transform.localScale.x);
        Vector2 dir = new Vector2(sign, 0);
        return Physics2D.Raycast(wallCheck.position, dir, wallCheckDistance);
    }

    private void IncreaseHitCount()
    {
        hitCount = (hitCount % maxHitCount) + minHitCount;
    }

    private void SpawnProjectile()
    {
        float lookDir = transform.localScale.x > 0 ? 0 : 180;
        Instantiate(FireProjectile, FireProjectileLauncher.position, Quaternion.Euler(0, lookDir, 0));
    }

    private void PlayhitSound()
    {
        if (currentAttackIndex == maxHitCount)
        {
            audioSource.pitch = 0.9f;
        }
        else
        {
            audioSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
        }
        audioSource.PlayOneShot(slashSounds[currentAttackIndex - 1]);
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
    }
}