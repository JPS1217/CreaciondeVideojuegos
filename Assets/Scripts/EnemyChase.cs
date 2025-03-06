using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public Transform jugador;
    public float velocidad = 3f;
    public float rangoDeteccion = 10f;

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia < rangoDeteccion)
        {
            transform.position = Vector3.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime);
            transform.LookAt(jugador);
        }
    }
}