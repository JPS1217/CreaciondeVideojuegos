using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 10f;  // Distancia para empezar a perseguir
    public float attackRange = 2f;  // Distancia para atacar
    public float attackCooldown = 1.5f; // Tiempo entre ataques
    private float lastAttackTime;

    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && Time.time > lastAttackTime + attackCooldown)
        {
            Attack();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            Idle();
        }

        // Actualizar la velocidad para el Animator
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
        agent.isStopped = false;

        animator.SetBool("isWalking", true);
        animator.SetBool("isChasing", true);
        animator.SetBool("isAttacking", false);
    }

    void Attack()
    {
        agent.isStopped = true; // Detener al enemigo al atacar
        animator.SetBool("isWalking", false);
        animator.SetBool("isChasing", false);
        animator.SetBool("isAttacking", true);

        lastAttackTime = Time.time;
    }

    void Idle()
    {
        agent.isStopped = true;
        animator.SetBool("isWalking", false);
        animator.SetBool("isChasing", false);
        animator.SetBool("isAttacking", false);
    }
}
