using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Punto de destino (coordenadas en el mundo)")]
    [SerializeField] private Vector3 puntoB;

    [Header("Movimiento")]
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

   
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, puntoB);
        Gizmos.DrawSphere(puntoB, 0.15f);
    }
}