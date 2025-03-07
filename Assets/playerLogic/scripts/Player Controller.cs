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

    public float attackCooldown = 0.5f;
    private bool isAttacking = false;

    public float attackRange = 2f;  // Rango de ataque del jugador
    public int attackDamage = 10;   // Daño del ataque

    void Update()
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

        if (!isAttacking && (horizontalInput != 0 || verticalInput != 0))
        {
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(Attack());
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animator.SetBool("isGrounded", charController.isGrounded);
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.2f); // Pequeño retraso para sincronizar la animación

        // Detectar colisiones durante el ataque
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider enemy in hitEnemies)
        {
            CactusAI cactus = enemy.GetComponent<CactusAI>();
            if (cactus != null)
            {
                cactus.TakeDamage(attackDamage);
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}