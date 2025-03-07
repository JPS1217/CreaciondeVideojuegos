using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoseScreenManager : MonoBehaviour
{
    public GameObject loseScreen;
    public Button restartButton; // Asigna el botón en el Inspector
    private CanvasGroup canvasGroup;

    void Start()
    {
        if (loseScreen != null)
        {
            loseScreen.SetActive(false); // Ocultar al inicio
            canvasGroup = loseScreen.GetComponent<CanvasGroup>();

            if (canvasGroup != null)
                canvasGroup.alpha = 0f; // Hacerlo invisible
        }

        if (restartButton != null)
        {
            restartButton.GetComponentInChildren<Text>().text = "Retry"; // Cambia el texto
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartGame); // Agregar función al botón
        }
    }

    public void ShowLoseScreen()
    {
        if (loseScreen != null)
        {
            loseScreen.SetActive(true);
            if (canvasGroup != null)
            {
                StartCoroutine(FadeIn());
            }

            Cursor.lockState = CursorLockMode.None; // Mostrar el cursor
            Cursor.visible = true;
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recarga el nivel
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float duration = 1f;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = time / duration; // Transición de opacidad
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }
}
