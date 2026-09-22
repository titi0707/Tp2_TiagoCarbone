using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    
    [SerializeField] private Vector3 puntoB;

    
    [SerializeField] private float velocidadMovimiento = 2f;
    [SerializeField] private float tiempoEsperaEnPunto = 1.5f;

    private Vector3 puntoA;
    private Vector3 puntoObjetivoActual;
    private float temporizadorEspera = 0f;
    private bool esperando = false;

    private void Start()
    {
        puntoA = transform.position;
        puntoObjetivoActual = puntoB;
    }

    private void Update()
    {
        if (esperando)
        {
            temporizadorEspera -= Time.deltaTime;
            if (temporizadorEspera <= 0f)
                esperando = false;
            return;
        }

        MoverHaciaPuntoActual();
    }

    private void MoverHaciaPuntoActual()
    {
        transform.position = Vector3.MoveTowards(transform.position, puntoObjetivoActual, velocidadMovimiento * Time.deltaTime);

        Vector3 direccion = puntoObjetivoActual - transform.position;
        direccion.y = 0f;
        if (direccion.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(direccion);

        if (Vector3.Distance(transform.position, puntoObjetivoActual) < 0.1f)
        {
            esperando = true;
            temporizadorEspera = tiempoEsperaEnPunto;
            puntoObjetivoActual = (puntoObjetivoActual == puntoB) ? puntoA : puntoB;
        }
    }

    public void EstablecerNuevoRecorrido(Vector3 centro, float distancia = 3f)
    {
        float anguloAleatorio = Random.Range(0f, 360f);
        Vector3 direccion = Quaternion.Euler(0f, anguloAleatorio, 0f) * Vector3.forward;

        puntoA = centro - direccion * distancia;
        puntoB = centro + direccion * distancia;
        puntoObjetivoActual = puntoB;
        esperando = false;
        temporizadorEspera = 0f;
    }

    private void OnDrawGizmos()
    {
       
        Vector3 origen = Application.isPlaying ? puntoA : transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(origen, puntoB);
        Gizmos.DrawSphere(puntoB, 0.15f);
    }
}