using UnityEngine;

public class PatrullaEnemigo : MonoBehaviour
{
    public Transform[] puntosPatrulla; // Array de puntos de patrulla
    public float velocidad = 3f;       // Velocidad de movimiento
    private int puntoActual = 0;       // Índice del punto actual
    private Animator animator;         // Referencia al Animator

    void Start()
    {
        animator = GetComponent<Animator>(); // Obtén el componente Animator
    }

    void Update()
    {
        if (puntosPatrulla.Length == 0) return; // Si no hay puntos, no hacer nada

        // Mueve al enemigo hacia el punto actual
        transform.position = Vector3.MoveTowards(transform.position, puntosPatrulla[puntoActual].position, velocidad * Time.deltaTime);

        // Si el enemigo llega al punto actual, cambia al siguiente punto
        if (Vector3.Distance(transform.position, puntosPatrulla[puntoActual].position) < 1)
        {
            puntoActual = (puntoActual + 1) % puntosPatrulla.Length; // Avanza al siguiente punto
        }
    }
}