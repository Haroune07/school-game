// EnemyAttack.cs — new
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Settings")]
    public float attackCooldown = 1f;

    [Header("References")]
    public GameObject swordCollisionDetector;

    private Animator anim;
    private float nextAttackTime = 0f;
    private int currentAttackCount = 0;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public bool CanAttack() => Time.time >= nextAttackTime;

    public void TriggerAttack()
    {
        anim.SetInteger("AttackCount", currentAttackCount);
        anim.SetTrigger("Attack");

        nextAttackTime = Time.time + attackCooldown;
        currentAttackCount = currentAttackCount == 0 ? 1 : 0;
    }

    public void TurnOnAttackCollision()
    {
        swordCollisionDetector.SetActive(true);
    }

    public void TurnOffAttackCollision()
    {
        swordCollisionDetector.SetActive(false);
    }
}