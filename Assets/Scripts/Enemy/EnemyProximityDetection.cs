using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyProximityDetection : MonoBehaviour
{
    [SerializeField] private LayerMask capasObstaculo;
    [SerializeField] private bool jugadorCerca = false;

    private Transform jugadorEnRango;

    public bool JugadorCerca => jugadorCerca;

    private void Reset()
    {
        SphereCollider col = GetComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = 2f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorEnRango = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = null;
            jugadorCerca = false;
        }
    }

    private void Update()
    {
        if (jugadorEnRango == null)
        {
            jugadorCerca = false;
            return;
        }

        Vector3 origen = transform.position + Vector3.up * 1f;
        Vector3 destino = jugadorEnRango.position + Vector3.up * 1f;
        Vector3 direccion = destino - origen;
        float distancia = direccion.magnitude;

        bool hayObstaculo = Physics.Raycast(origen, direccion.normalized, out RaycastHit hit, distancia, capasObstaculo);

        
        Debug.DrawLine(origen, destino, hayObstaculo ? Color.red : Color.green);

        if (hayObstaculo)
            Debug.Log("Rayo de proximidad bloqueado por: " + hit.collider.name);

        jugadorCerca = !hayObstaculo;
    }
}