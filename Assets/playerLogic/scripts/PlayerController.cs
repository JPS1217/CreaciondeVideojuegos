using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float gravityScale = 5f;
    public float rotateSpeed = 5f;
    public float jumpHorizontalBoost = 1.5f;

    private Vector3 moveDirection;
    public CharacterController charController;
    public Camera playerCamera;
    public GameObject playerModel;
    public Animator animator;

    public float attackCooldown = 0.5f; // Cooldown entre ataques
    public float attackAnimationDuration = 0.3f; // Duración de la animación de ataque
    private bool isAttacking = false;

    public float attackRange = 2f;  // Rango de ataque del jugador
    public int attackDamage = 10;   // Daño del ataque
    public float attackAngle = 90f; // Ángulo de detección del ataque (en grados)

    public float dashSpeed = 10f;   // Velocidad del dash
    public float dashDuration = 0.2f; // Duración del dash
    private bool isDashing = false; // Estado del dash

    void Update()
    {
        if (!isAttacking && !isDashing)
        {
            float verticalInput = Input.GetAxisRaw("Vertical");
            float horizontalInput = Input.GetAxisRaw("Horizontal");

            Vector3 cameraForward = playerCamera.transform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 cameraRight = playerCamera.transform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();

            Vector3 moveInput = (cameraForward * verticalInput) + (cameraRight * horizontalInput);
            moveInput.Normalize();
            moveInput *= moveSpeed;

            float yStore = moveDirection.y;
            moveDirection.x = moveInput.x;
            moveDirection.z = moveInput.z;

            if (charController.isGrounded)
            {
                moveDirection.y = -0.5f;
            }
            else
            {
                moveDirection.y = yStore;
            }

            if (Input.GetButtonDown("Jump") && charController.isGrounded)
            {
                moveDirection.y = jumpForce;
                moveDirection.x *= jumpHorizontalBoost;
                moveDirection.z *= jumpHorizontalBoost;
                animator.SetBool("isJumping", true);
            }

            moveDirection.y += Physics.gravity.y * gravityScale * Time.deltaTime;
            charController.Move(moveDirection * Time.deltaTime);

            if (horizontalInput != 0 || verticalInput != 0)
            {
                Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
            }

            animator.SetFloat("Speed", Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
            animator.SetBool("isGrounded", charController.isGrounded);
        }

        if (Input.GetMouseButtonDown(0) && !isAttacking && !isDashing)
        {
            StartCoroutine(Attack());
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isAttacking && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Attack()
{
    isAttacking = true;
    animator.SetTrigger("Attack");

    // Desactivar movimiento durante la animación de ataque
    yield return new WaitForSeconds(attackAnimationDuration);

    // Detectar colisiones durante el ataque
    Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange);
    foreach (Collider enemy in hitEnemies)
    {
        // Verificar si el enemigo está dentro del ángulo de ataque
        Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
        float angleToEnemy = Vector3.Angle(playerModel.transform.forward, directionToEnemy); // Usar playerModel.transform.forward

        if (angleToEnemy < attackAngle / 2)
        {
            CactusAI cactus = enemy.GetComponent<CactusAI>();
            if (cactus != null)
            {
                cactus.TakeDamage(attackDamage);
            }
        }
    }

    // Reactivar movimiento después de la animación
    isAttacking = false;

    // Cooldown antes de poder atacar de nuevo
    yield return new WaitForSeconds(attackCooldown - attackAnimationDuration);
}

    private IEnumerator Dash()
    {
        isDashing = true;
        animator.SetTrigger("Dash");

        float startTime = Time.time;

        Vector3 dashDirection = moveDirection.normalized;
        if (dashDirection == Vector3.zero)
        {
            dashDirection = playerModel.transform.forward;
        }

        while (Time.time < startTime + dashDuration)
        {
            charController.Move(dashDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Dibujar el ángulo de ataque
        Vector3 forward = transform.forward;
        Vector3 left = Quaternion.Euler(0, -attackAngle / 2, 0) * forward;
        Vector3 right = Quaternion.Euler(0, attackAngle / 2, 0) * forward;

        Gizmos.DrawLine(transform.position, transform.position + left * attackRange);
        Gizmos.DrawLine(transform.position, transform.position + right * attackRange);
    }
}