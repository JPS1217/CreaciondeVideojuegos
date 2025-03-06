using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float gravityScale = 5f;
    public float rotateSpeed = 5f;

    private Vector3 moveDirection;
    public CharacterController charController;
    public Camera playerCamera;
    public GameObject playerModel;
    public Animator animator;

    // Coyote Time Variables
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    // Ataque
    public float attackCooldown = 0.5f; // Tiempo entre ataques
    private bool isAttacking = false;

    void Update()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Obtener la dirección de la cámara en el plano XZ
        Vector3 cameraForward = playerCamera.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = playerCamera.transform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        // Movimiento basado en la dirección de la cámara
        Vector3 moveInput = (cameraForward * verticalInput) + (cameraRight * horizontalInput);
        moveInput.Normalize();
        moveInput *= moveSpeed;

        // Guardar la velocidad vertical actual
        float yStore = moveDirection.y;
        moveDirection = moveInput;

        // Manejo de Coyote Time
        if (charController.isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            moveDirection.y = -0.5f;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            moveDirection.y = yStore;
        }

        // Salto con Coyote Time
        if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0f)
        {
            moveDirection.y = jumpForce;
            animator.SetBool("isJumping", true);
            coyoteTimeCounter = 0f;
        }

        moveDirection.y += Physics.gravity.y * gravityScale * Time.deltaTime;
        charController.Move(moveDirection * Time.deltaTime);

        // Rotar personaje en dirección del movimiento
        if (!isAttacking && (horizontalInput != 0 || verticalInput != 0))
        {
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

        // Ataque con clic izquierdo
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(Attack());
        }

        // Animaciones
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animator.SetBool("isGrounded", charController.isGrounded);
    }

    private System.Collections.IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack"); // Activar la animación de ataque

        yield return new WaitForSeconds(attackCooldown); // Esperar el tiempo del ataque

        isAttacking = false;
    }
}
