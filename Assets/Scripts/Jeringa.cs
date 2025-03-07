using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class Crystal : MonoBehaviour
{
    public AudioClip sonidoCristal; // Sonido al recoger el cristal
    public bool esFinal = false; // Si este cristal termina el nivel

    public Transform player;
    public int curacion=20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el jugador tocó el cristal
        {
            AudioSource.PlayClipAtPoint(sonidoCristal, transform.position); // Reproducir sonido

            if (esFinal)
            {
                Application.Quit(); // Cierra el juego (solo funciona en build)

                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // Detiene el juego en el editor
                #endif

            }
            else
            {
                // Si es el primer cristal, solo se destruye
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                playerHealth.TakeDamage(curacion);
                Destroy(gameObject);
            }
        }
    }
}

