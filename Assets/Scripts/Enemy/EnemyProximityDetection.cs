using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyProximityDetection : MonoBehaviour
{
   
    [SerializeField] private bool jugadorCerca = false;
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
            jugadorCerca = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorCerca = false;
    }
}