using UnityEngine;

[RequireComponent(typeof(DistractionInventory))]
public class PlayerDistraction : MonoBehaviour
{
    [SerializeField] private GameObject prefabDistraccion;
    [SerializeField] private float fuerzaLanzamiento = 8f;

    private DistractionInventory inventario;

    private void Awake()
    {
        inventario = GetComponent<DistractionInventory>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            LanzarDistraccion();
        }
    }

    private void LanzarDistraccion()
    {
        if (!inventario.TieneObjetos())
        {
            Debug.Log("No te quedan objetos de distracción");
            return;
        }

        Vector3 origen = transform.position + transform.forward + Vector3.up;
        GameObject objeto = Instantiate(prefabDistraccion, origen, Quaternion.identity);

        Rigidbody rb = objeto.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direccion = (transform.forward + Vector3.up * 0.5f).normalized;
            rb.AddForce(direccion * fuerzaLanzamiento, ForceMode.VelocityChange);
        }

        inventario.UsarObjeto();
    }
}