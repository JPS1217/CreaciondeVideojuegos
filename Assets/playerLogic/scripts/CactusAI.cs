using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CactusAI : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isAttacking = false;

    public int health = 30; // Vida del enemigo

    // Variables para el feedback visual al recibir daño
    public Color damageColor = Color.red; // Color cuando recibe daño
    public float damageFlashDuration = 0.2f; // Duración del efecto de parpadeo

    private SkinnedMeshRenderer[] cactusRenderers; // Todos los SkinnedMeshRenderer del cactus
    private Color[][] originalColors; // Guardar los colores originales de todos los materiales

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Obtener todos los SkinnedMeshRenderer del cactus y sus hijos
        cactusRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        // Guardar los colores originales de todos los materiales
        originalColors = new Color[cactusRenderers.Length][];
        for (int i = 0; i < cactusRenderers.Length; i++)
        {
            originalColors[i] = new Color[cactusRenderers[i].materials.Length];
            for (int j = 0; j < cactusRenderers[i].materials.Length; j++)
            {
                originalColors[i][j] = cactusRenderers[i].materials[j].color;
            }
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isAttacking)
        {
            if (distanceToPlayer <= attackRange)
            {
                AttackPlayer();
            }
            else if (distanceToPlayer <= chaseRange)
            {
                ChasePlayer();
            }
            else
            {
                Idle();
            }
        }
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        animator.SetBool("isMoving", true);
    }

    void AttackPlayer()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            agent.isStopped = true;
            animator.SetBool("isMoving", false);
            animator.SetTrigger("Attack");

            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

            // Iniciar la corrutina para aplicar el daño en el 50% de la animación
            StartCoroutine(DelayedDamage(attackCooldown * 0.2f));

            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    // Corrutina para aplicar el daño en el 50% de la animación
    private IEnumerator DelayedDamage(float delay)
    {
        yield return new WaitForSeconds(delay);

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            Idle();
        }
    }

    void Idle()
    {
        agent.isStopped = true;
        animator.SetBool("isMoving", false);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"Enemigo recibió {damage} de daño. Vida restante: {health}");

        // Feedback visual: Cambiar el color del cactus a rojo
        StartCoroutine(FlashDamage());

        if (health <= 0)
        {
            Die();
        }
    }

    // Corrutina para el efecto de parpadeo al recibir daño
    private IEnumerator FlashDamage()
    {
        // Cambiar el color de todos los materiales a rojo
        foreach (var renderer in cactusRenderers)
        {
            foreach (var material in renderer.materials)
            {
                material.color = damageColor;
            }
        }

        yield return new WaitForSeconds(damageFlashDuration); // Esperar

        // Restaurar los colores originales de todos los materiales
        for (int i = 0; i < cactusRenderers.Length; i++)
        {
            for (int j = 0; j < cactusRenderers[i].materials.Length; j++)
            {
                cactusRenderers[i].materials[j].color = originalColors[i][j];
            }
        }
    }

    void Die()
    {
        Debug.Log("Enemigo ha muerto.");
        Destroy(gameObject);
    }
}