using UnityEngine;

public class EnemyStateController : MonoBehaviour
{
    public enum Estado { Patrullando, Alerta, Persiguiendo, Investigando }

    [SerializeField] private EnemyPatrol patrullaje;
    [SerializeField] private EnemyDetection deteccionVision;
    [SerializeField] private EnemyProximityDetection deteccionCercania;
    [SerializeField] private Transform jugador;

    [SerializeField] private float tiempoAlerta = 1f;

    [SerializeField] private float velocidadPersecucion = 4f;

    
    [SerializeField] private float distanciaCaptura = 1f;
    [SerializeField] private float velocidadInvestigacion = 3f;
    [SerializeField] private float tiempoInvestigando = 2f;

    [SerializeField] private float radioNuevoRecorrido = 3f;

    [SerializeField] private LayerMask capaObstaculos;

    
    [SerializeField] private Estado estadoActual = Estado.Patrullando;

    public Estado EstadoActual => estadoActual;

    private float temporizadorAlerta = 0f;
    private float temporizadorInvestigacion = 0f;
    private Vector3 ultimaPosicionConocida;
    private Vector3 puntoInvestigacion;
    private PlayerHidingStatus estadoEscondidoJugador;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (jugador != null)
            estadoEscondidoJugador = jugador.GetComponent<PlayerHidingStatus>();
    }

    private void FixedUpdate()
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
                temporizadorAlerta -= Time.fixedDeltaTime;
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

            case Estado.Investigando:
                patrullaje.enabled = false;

                if (jugadorEncontrado)
                {
                    estadoActual = Estado.Alerta;
                    temporizadorAlerta = tiempoAlerta;
                    break;
                }

                MoverHaciaPunto(puntoInvestigacion, velocidadInvestigacion);

                if (DistanciaHorizontal(transform.position, puntoInvestigacion) < 0.3f)
                {
                    temporizadorInvestigacion -= Time.fixedDeltaTime;
                    if (temporizadorInvestigacion <= 0f)
                    {
                        patrullaje.EstablecerNuevoRecorrido(puntoInvestigacion, radioNuevoRecorrido);
                        estadoActual = Estado.Patrullando;
                    }
                }
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool esObstaculo = (capaObstaculos.value & (1 << collision.gameObject.layer)) != 0;
        if (!esObstaculo) return;

        if (estadoActual == Estado.Patrullando)
        {
            patrullaje.EstablecerNuevoRecorrido(transform.position, radioNuevoRecorrido);
        }
        else
        {
            EsquivarObstaculo();
        }
    }

    private void EsquivarObstaculo()
    {
        float anguloEsquive = Random.Range(60f, 120f) * (Random.value < 0.5f ? 1f : -1f);
        Vector3 nuevaDireccion = Quaternion.Euler(0f, anguloEsquive, 0f) * transform.forward;
        rb.MovePosition(transform.position + nuevaDireccion.normalized * 0.5f);
    }

    public void Investigar(Vector3 posicion)
    {
        if (estadoActual == Estado.Alerta || estadoActual == Estado.Persiguiendo) return;

        posicion.y = transform.position.y;
        puntoInvestigacion = posicion;
        temporizadorInvestigacion = tiempoInvestigando;
        estadoActual = Estado.Investigando;
        patrullaje.enabled = false;
    }

    private void Perseguir()
    {
        Vector3 destino = jugador.position;
        destino.y = transform.position.y;

        Vector3 nuevaPosicion = Vector3.MoveTowards(transform.position, destino, velocidadPersecucion * Time.fixedDeltaTime);
        rb.MovePosition(nuevaPosicion);
        MirarHaciaJugador();

        if (DistanciaHorizontal(transform.position, jugador.position) < distanciaCaptura)
        {
            GameManager.Instancia.PerderJuego();
        }
    }

    private void MoverHaciaPunto(Vector3 destino, float velocidad)
    {
        destino.y = transform.position.y;

        Vector3 nuevaPosicion = Vector3.MoveTowards(transform.position, destino, velocidad * Time.fixedDeltaTime);
        rb.MovePosition(nuevaPosicion);

        Vector3 direccion = destino - transform.position;
        direccion.y = 0f;
        if (direccion.sqrMagnitude > 0.01f)
            rb.MoveRotation(Quaternion.LookRotation(direccion));
    }

    private void MirarHaciaJugador()
    {
        Vector3 direccion = jugador.position - transform.position;
        direccion.y = 0f;
        if (direccion.sqrMagnitude > 0.01f)
            rb.MoveRotation(Quaternion.LookRotation(direccion));
    }

    private float DistanciaHorizontal(Vector3 a, Vector3 b)
    {
        a.y = 0f;
        b.y = 0f;
        return Vector3.Distance(a, b);
    }
}