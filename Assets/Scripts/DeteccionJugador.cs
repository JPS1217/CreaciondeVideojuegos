using UnityEngine;

public class DeteccionJugador : MonoBehaviour
{
    public float rangoDeteccion = 5f; // Rango de detección del jugador
    public Transform jugador;         // Referencia al jugador

    void Update()
    {
        // Calcula la distancia entre el enemigo y el jugador
        float distancia = Vector3.Distance(transform.position, jugador.position);

        // Si el jugador está dentro del rango de detección
        if (distancia <= rangoDeteccion)
        {
            Debug.Log("Jugador detectado!");
            // Aquí puedes agregar comportamiento adicional, como perseguir al jugador
        }
    }
}