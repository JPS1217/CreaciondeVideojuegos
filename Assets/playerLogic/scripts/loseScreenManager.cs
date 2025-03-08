using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseScreenManager : MonoBehaviour
{
    public GameObject loseScreenCanvas; // Referencia al Canvas de la pantalla de derrota
    public Text loseText; // Referencia al texto de "You Died"
    public Button restartButton; // Referencia al botón de reiniciar

    void Start()
    {
        // Ocultar la pantalla de derrota al inicio
        loseScreenCanvas.SetActive(false);

        // Asignar la función ReiniciarNivel al botón
        restartButton.onClick.AddListener(ReiniciarNivel);
    }

    // Método para mostrar la pantalla de derrota
    public void ShowLoseScreen()
    {
        loseScreenCanvas.SetActive(true);
        Time.timeScale = 0f; // Pausar el juego

        // Habilitar y desbloquear el cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Método para reiniciar el nivel
    public void ReiniciarNivel()
    {
        Time.timeScale = 1f; // Reanudar el juego
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reiniciar la escena actual

        // Después de reiniciar, asegurarse de que el cursor esté oculto y bloqueado (si es necesario)
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}