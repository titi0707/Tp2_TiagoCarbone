using UnityEngine;

public class EnemyStateController : MonoBehaviour
{
    private enum Estado { Patrullando, Persiguiendo }

    
    [SerializeField] private EnemyPatrol patrullaje;
    [SerializeField] private EnemyDetection deteccionVision;
    [SerializeField] private EnemyProximityDetection deteccionCercania;
    [SerializeField] private Transform jugador;

    
    [SerializeField] private float velocidadPersecucion = 4f;

    
    [SerializeField] private Estado estadoActual = Estado.Patrullando;

    private void Update()
    {
        bool jugadorEncontrado = deteccionVision.JugadorDetectado || deteccionCercania.JugadorCerca;
        estadoActual = jugadorEncontrado ? Estado.Persiguiendo : Estado.Patrullando;

        switch (estadoActual)
        {
            case Estado.Patrullando:
                patrullaje.enabled = true;
                break;

            case Estado.Persiguiendo:
                patrullaje.enabled = false;
                Perseguir();
                break;
        }
    }

    private void Perseguir()
    {
        transform.position = Vector3.MoveTowards(transform.position, jugador.position, velocidadPersecucion * Time.deltaTime);

        Vector3 direccion = jugador.position - transform.position;
        direccion.y = 0f;
        if (direccion.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(direccion);
    }
}