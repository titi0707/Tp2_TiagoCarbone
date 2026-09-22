using UnityEngine;

public class EnemyStateController : MonoBehaviour
{
    public enum Estado { Patrullando, Alerta, Persiguiendo }

    
    [SerializeField] private EnemyPatrol patrullaje;
    [SerializeField] private EnemyDetection deteccionVision;
    [SerializeField] private EnemyProximityDetection deteccionCercania;
    [SerializeField] private Transform jugador;

    
    [SerializeField] private float tiempoAlerta = 1f;

    
    [SerializeField] private float velocidadPersecucion = 4f;

    
    [SerializeField] private float radioNuevoRecorrido = 3f;

    
    [SerializeField] private Estado estadoActual = Estado.Patrullando;

    public Estado EstadoActual => estadoActual;

    private float temporizadorAlerta = 0f;
    private Vector3 ultimaPosicionConocida;
    private PlayerHidingStatus estadoEscondidoJugador;

    private void Awake()
    {
        if (jugador != null)
            estadoEscondidoJugador = jugador.GetComponent<PlayerHidingStatus>();
    }

    private void Update()
    {
        if (estadoEscondidoJugador != null && estadoEscondidoJugador.EstaEscondido)
        {
            if (estadoActual != Estado.Patrullando)
                patrullaje.EstablecerNuevoRecorrido(ultimaPosicionConocida, radioNuevoRecorrido);

            estadoActual = Estado.Patrullando;
            patrullaje.enabled = true;
            return;
        }

        bool jugadorEncontrado = deteccionVision.JugadorDetectado || deteccionCercania.JugadorCerca;

        if (jugadorEncontrado)
            ultimaPosicionConocida = jugador.position;

        switch (estadoActual)
        {
            case Estado.Patrullando:
                patrullaje.enabled = true;

                if (jugadorEncontrado)
                {
                    estadoActual = Estado.Alerta;
                    temporizadorAlerta = tiempoAlerta;
                }
                break;

            case Estado.Alerta:
                patrullaje.enabled = false;
                MirarHaciaJugador();

                if (!jugadorEncontrado)
                {
                    patrullaje.EstablecerNuevoRecorrido(ultimaPosicionConocida, radioNuevoRecorrido);
                    estadoActual = Estado.Patrullando;
                    break;
                }

                temporizadorAlerta -= Time.deltaTime;
                if (temporizadorAlerta <= 0f)
                    estadoActual = Estado.Persiguiendo;
                break;

            case Estado.Persiguiendo:
                patrullaje.enabled = false;

                if (!jugadorEncontrado)
                {
                    patrullaje.EstablecerNuevoRecorrido(ultimaPosicionConocida, radioNuevoRecorrido);
                    estadoActual = Estado.Patrullando;
                    break;
                }

                Perseguir();
                break;
        }
    }

    private void Perseguir()
    {
        transform.position = Vector3.MoveTowards(transform.position, jugador.position, velocidadPersecucion * Time.deltaTime);
        MirarHaciaJugador();
    }

    private void MirarHaciaJugador()
    {
        Vector3 direccion = jugador.position - transform.position;
        direccion.y = 0f;
        if (direccion.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(direccion);
    }
}