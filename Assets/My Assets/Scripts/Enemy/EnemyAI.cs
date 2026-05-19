// EnemyAI.cs — replaces your current one
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Detection")]
    public float awareDistance = 25f;
    public float attackRange = 1.5f;

    private EnemyController controller;
    private EnemyAttack attack;

    private enum State { Idle, Chase, Attack }
    private State state = State.Idle;

    void Awake()
    {
        controller = GetComponent<EnemyController>();
        attack = GetComponent<EnemyAttack>();
    }

    void Update()
    {
        if (player == null || GetComponent<EnemyHealth>().currentHealth <= 0) return;

        float dist = Vector2.Distance(transform.position, player.position);

        switch (state)
        {
            case State.Idle:
                HandleIdle(dist);
                break;
            case State.Chase:
                HandleChase(dist);
                break;
            case State.Attack:
                HandleAttack(dist);
                break;
        }
    }

    void HandleIdle(float dist)
    {
        if (dist < awareDistance)
            state = State.Chase;
    }

    void HandleChase(float dist)
    {
        if (dist > awareDistance)
        {
            controller.Stop();
            state = State.Idle;
            return;
        }

        if (dist <= attackRange && attack.CanAttack())
        {
            controller.Stop();
            state = State.Attack;
            return;
        }

        Vector2 dir = (player.position - transform.position).normalized;
        controller.Move(dir);
    }

    void HandleAttack(float dist)
    {
        // If enemy gets interrupted (knockback) or player runs — go back to chase
        if (dist > attackRange)
        {
            state = State.Chase;
            return;
        }

        if (attack.CanAttack())
        {
            attack.TriggerAttack();
        }
    }

    // Called by EnemyCombat when hit
    public void OnKnockback()
    {
        state = State.Chase;
    }
}