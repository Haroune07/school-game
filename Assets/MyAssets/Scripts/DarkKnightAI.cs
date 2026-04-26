using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;

    private EnemyController controller;

    public EnemyHealthManger enemyHealthManger;

    public float awareDistance = 25;


    void Awake()
    {
        controller = GetComponent<EnemyController>();
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if(dist < awareDistance)
        {
            enemyHealthManger.healthBar.gameObject.SetActive(true);

            if (dist > 1f)
            {
                Vector2 dir = (player.position - transform.position).normalized;
                controller.Move(dir);
            }
        }

        
        else
        {
            controller.Stop();
        }
    }
}