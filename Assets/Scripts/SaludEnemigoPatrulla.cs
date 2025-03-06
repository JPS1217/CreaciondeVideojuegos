using UnityEngine;

public class SaludEnemigo : MonoBehaviour
{
    public int saludMaxima = 100; // Salud máxima del enemigo
    private int saludActual;       // Salud actual del enemigo

    void Start()
    {
        saludActual = saludMaxima; // Inicializa la salud al máximo
    }

    public void RecibirDaño(int cantidad)
    {
        saludActual -= cantidad; // Reduce la salud
        if (saludActual <= 0)
        {
            Morir(); // Llama a la función de muerte si la salud llega a 0
        }
    }

    void Morir()
    {
        Debug.Log("Enemigo derrotado!");
        Destroy(gameObject); // Destruye el objeto del enemigo
    }
}