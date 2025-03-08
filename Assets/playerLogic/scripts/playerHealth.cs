using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public HealthBar healthBar;  // Asignar desde el Inspector
    public LoseScreenManager loseScreenManager;  // Referencia al LoseScreenManager

    // Variables para el feedback visual
    public Color damageColor = Color.red; // Color cuando recibe daño
    public float damageFlashDuration = 0.2f; // Duración del efecto de parpadeo

    // Variables para el feedback de sonido
    public AudioSource audioSource; // AudioSource para reproducir sonidos
    public AudioClip damageSound; // Sonido cuando recibe daño

    private SkinnedMeshRenderer[] playerRenderers; // Todos los SkinnedMeshRenderer del jugador
    private Material[][] originalMaterials; // Guardar los materiales originales

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        // Obtener todos los SkinnedMeshRenderer del jugador y sus hijos (incluyendo inactivos)
        playerRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(true);

        // Guardar los materiales originales
        originalMaterials = new Material[playerRenderers.Length][];
        for (int i = 0; i < playerRenderers.Length; i++)
        {
            originalMaterials[i] = new Material[playerRenderers[i].materials.Length];
            for (int j = 0; j < playerRenderers[i].materials.Length; j++)
            {
                // Crear una copia del material para no modificar el original
                originalMaterials[i][j] = new Material(playerRenderers[i].materials[j]);
            }
        }

        // Asegurarse de que el AudioSource esté asignado
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0){
            currentHealth -= damage;
            if(currentHealth>=maxHealth){
                currentHealth=maxHealth;
            }
            healthBar.SetHealth(currentHealth);
        }; // Si el daño es negativo, no hacer nada

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        // Feedback visual: Cambiar el color del jugador
        StartCoroutine(FlashDamage());

        // Feedback de sonido: Reproducir sonido de daño
        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Corrutina para el efecto de parpadeo al recibir daño
    private IEnumerator FlashDamage()
    {
        // Cambiar el color de todos los materiales a rojo
        foreach (var renderer in playerRenderers)
        {
            foreach (var material in renderer.materials)
            {
                material.color = damageColor;
            }
        }

        yield return new WaitForSeconds(damageFlashDuration); // Esperar

        // Restaurar los materiales originales
        for (int i = 0; i < playerRenderers.Length; i++)
        {
            playerRenderers[i].materials = originalMaterials[i];
        }
    }

    void Die()
    {
        // Mostrar la pantalla de derrota
        if (loseScreenManager != null)
        {
            loseScreenManager.ShowLoseScreen();
        }
        else
        {
            Debug.LogError("LoseScreenManager no asignado en el Inspector.");
        }

        gameObject.SetActive(false); 
    }
}