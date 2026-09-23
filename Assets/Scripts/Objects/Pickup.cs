using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DistractionPickup : MonoBehaviour
{
    [SerializeField] private int cantidadQueOtorga = 1;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        DistractionInventory inventario = other.GetComponent<DistractionInventory>();
        if (inventario != null)
        {
            inventario.AgregarObjeto(cantidadQueOtorga);
            Destroy(gameObject);
        }
    }
}