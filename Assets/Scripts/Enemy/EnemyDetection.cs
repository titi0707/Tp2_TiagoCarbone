using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
  
    [SerializeField] private Transform jugador;
    [SerializeField] private float distanciaDeteccion = 8f;
    [SerializeField] private float anguloVision = 90f; 
    [SerializeField] private LayerMask capasObstaculo;

   
    [SerializeField] private bool jugadorDetectado = false;
    public bool JugadorDetectado => jugadorDetectado;

    private void Update()
    {
        jugadorDetectado = PuedeVerAlJugador();
    }

    private bool PuedeVerAlJugador()
    {
        if (jugador == null) return false;

        Vector3 origen = transform.position + Vector3.up * 1f;
        Vector3 destino = jugador.position + Vector3.up * 1f;
        Vector3 direccion = destino - origen;
        float distancia = direccion.magnitude;

        if (distancia > distanciaDeteccion) return false;

        
        float anguloHaciaJugador = Vector3.Angle(transform.forward, direccion);
        if (anguloHaciaJugador > anguloVision * 0.5f) return false;

        if (Physics.Raycast(origen, direccion.normalized, distancia, capasObstaculo))
            return false;

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        if (jugador == null) return;

        Gizmos.color = jugadorDetectado ? Color.green : Color.red;
        Vector3 origen = transform.position + Vector3.up * 1f;
        Vector3 destino = jugador.position + Vector3.up * 1f;
        Gizmos.DrawLine(origen, destino);
        Gizmos.DrawWireSphere(transform.position, distanciaDeteccion);

        
        Quaternion rotacionIzquierda = Quaternion.AngleAxis(-anguloVision * 0.5f, Vector3.up);
        Quaternion rotacionDerecha = Quaternion.AngleAxis(anguloVision * 0.5f, Vector3.up);
        Vector3 direccionIzquierda = rotacionIzquierda * transform.forward;
        Vector3 direccionDerecha = rotacionDerecha * transform.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + direccionIzquierda * distanciaDeteccion);
        Gizmos.DrawLine(transform.position, transform.position + direccionDerecha * distanciaDeteccion);
    }
}