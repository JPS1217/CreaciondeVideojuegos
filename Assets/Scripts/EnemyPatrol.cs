using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] puntosDePatrulla;
    public float velocidad = 2f;
    private int siguientePunto = 0;

    void Update()
    {
        if (puntosDePatrulla.Length == 0) return;

        Transform objetivo = puntosDePatrulla[siguientePunto];
        transform.position = Vector3.MoveTowards(transform.position, objetivo.position, velocidad * Time.deltaTime);

        if (Vector3.Distance(transform.position, objetivo.position) < 0.2f)
        {
            siguientePunto = (siguientePunto + 1) % puntosDePatrulla.Length;
        }
    }
}