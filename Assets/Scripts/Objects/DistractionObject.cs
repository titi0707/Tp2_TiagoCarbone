using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DistractionObject : MonoBehaviour
{
    [SerializeField] private float radioRuido = 6f;
    [SerializeField] private LayerMask capaEnemigos;

    private bool yaGeneroRuido = false;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("[Distraccion] Choqué contra: " + collision.gameObject.name);
        GenerarRuido();
    }

    private void GenerarRuido()
    {
        if (yaGeneroRuido) return;
        yaGeneroRuido = true;

        Debug.Log("[Distraccion] Buscando enemigos en radio " + radioRuido);
        Collider[] enemigosCercanos = Physics.OverlapSphere(transform.position, radioRuido, capaEnemigos);
        Debug.Log("[Distraccion] Colliders encontrados: " + enemigosCercanos.Length);

        foreach (Collider col in enemigosCercanos)
        {
            Debug.Log("[Distraccion] Encontré: " + col.gameObject.name);
            EnemyStateController controlador = col.GetComponent<EnemyStateController>();
            if (controlador != null)
            {
                Debug.Log("[Distraccion] Avisando a: " + controlador.gameObject.name);
                controlador.Investigar(transform.position);
            }
            else
            {
                Debug.Log("[Distraccion] Ese collider no tiene EnemyStateController encima");
            }
        }
    }
}