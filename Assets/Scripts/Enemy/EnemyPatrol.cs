using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public enum TipoPatrulla { IdaYVuelta, Circuito }

    
    [SerializeField] private TipoPatrulla tipoPatrulla = TipoPatrulla.IdaYVuelta;

    
    [SerializeField] private Vector3 puntoB;

   
    [SerializeField] private Vector3[] puntosCircuito;

   
    [SerializeField] private float velocidadMovimiento = 2f;
    [SerializeField] private float tiempoEsperaEnPunto = 1.5f;

    private Vector3 puntoA;
    private Vector3 puntoObjetivoActual;
    private int indiceCircuito = 0;
    private float temporizadorEspera = 0f;
    private bool esperando = false;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        puntoA = transform.position;

        if (tipoPatrulla == TipoPatrulla.IdaYVuelta)
        {
            puntoObjetivoActual = puntoB;
        }
        else
        {
            indiceCircuito = 0;
            puntoObjetivoActual = (puntosCircuito != null && puntosCircuito.Length > 0) ? puntosCircuito[0] : transform.position;
        }
    }

    private void FixedUpdate()
    {
        if (esperando)
        {
            temporizadorEspera -= Time.fixedDeltaTime;
            if (temporizadorEspera <= 0f)
                esperando = false;
            return;
        }
        MoverHaciaPuntoActual();
    }

    private void MoverHaciaPuntoActual()
    {
        Vector3 destino = puntoObjetivoActual;
        destino.y = transform.position.y;

        Vector3 nuevaPosicion = Vector3.MoveTowards(transform.position, destino, velocidadMovimiento * Time.fixedDeltaTime);
        rb.MovePosition(nuevaPosicion);

        Vector3 direccion = destino - transform.position;
        direccion.y = 0f;
        if (direccion.sqrMagnitude > 0.01f)
            rb.MoveRotation(Quaternion.LookRotation(direccion));

        if (Vector3.Distance(transform.position, destino) < 0.1f)
        {
            esperando = true;
            temporizadorEspera = tiempoEsperaEnPunto;
            AvanzarAlSiguientePunto();
        }
    }

    private void AvanzarAlSiguientePunto()
    {
        if (tipoPatrulla == TipoPatrulla.IdaYVuelta)
        {
            puntoObjetivoActual = (puntoObjetivoActual == puntoB) ? puntoA : puntoB;
        }
        else
        {
            if (puntosCircuito == null || puntosCircuito.Length == 0) return;

            indiceCircuito = (indiceCircuito + 1) % puntosCircuito.Length;
            puntoObjetivoActual = puntosCircuito[indiceCircuito];
        }
    }

    public void EstablecerNuevoRecorrido(Vector3 centro, float distancia = 3f)
    {
        esperando = false;
        temporizadorEspera = 0f;

        if (tipoPatrulla == TipoPatrulla.IdaYVuelta)
        {
            float anguloAleatorio = Random.Range(0f, 360f);
            Vector3 direccion = Quaternion.Euler(0f, anguloAleatorio, 0f) * Vector3.forward;

            puntoA = centro - direccion * distancia;
            puntoB = centro + direccion * distancia;
            puntoObjetivoActual = puntoB;
        }
        else
        {
            
            if (puntosCircuito == null || puntosCircuito.Length == 0) return;

            int indiceMasCercano = 0;
            float distanciaMinima = Vector3.Distance(transform.position, puntosCircuito[0]);

            for (int i = 1; i < puntosCircuito.Length; i++)
            {
                float d = Vector3.Distance(transform.position, puntosCircuito[i]);
                if (d < distanciaMinima)
                {
                    distanciaMinima = d;
                    indiceMasCercano = i;
                }
            }

            indiceCircuito = indiceMasCercano;
            puntoObjetivoActual = puntosCircuito[indiceCircuito];
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (tipoPatrulla == TipoPatrulla.IdaYVuelta)
        {
            Vector3 origen = Application.isPlaying ? puntoA : transform.position;
            Gizmos.DrawLine(origen, puntoB);
            Gizmos.DrawSphere(puntoB, 0.15f);
        }
        else if (puntosCircuito != null && puntosCircuito.Length > 1)
        {
            for (int i = 0; i < puntosCircuito.Length; i++)
            {
                Vector3 actual = puntosCircuito[i];
                Vector3 siguiente = puntosCircuito[(i + 1) % puntosCircuito.Length];
                Gizmos.DrawLine(actual, siguiente);
                Gizmos.DrawSphere(actual, 0.15f);
            }
        }
    }
}