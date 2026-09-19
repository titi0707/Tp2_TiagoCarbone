using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float velocidadMovimiento = 4f;
    [SerializeField] private float velocidadRotacion = 10f;

    private Rigidbody rb;
    private Vector3 direccionMovimiento;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDireccion = new Vector3(horizontal, 0f, vertical);

        if (inputDireccion.sqrMagnitude > 0.01f && Camera.main != null)
        {
            float anguloCamara = Camera.main.transform.eulerAngles.y;
            direccionMovimiento = Quaternion.Euler(0f, anguloCamara, 0f) * inputDireccion;
        }
        else
        {
            direccionMovimiento = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        MoverPersonaje();
        RotarHaciaDireccion();
    }

    private void MoverPersonaje()
    {
        Vector3 nuevaPosicion = rb.position + direccionMovimiento * velocidadMovimiento * Time.fixedDeltaTime;
        rb.MovePosition(nuevaPosicion);
    }

    private void RotarHaciaDireccion()
    {
        if (direccionMovimiento.sqrMagnitude < 0.01f) return;

        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento, Vector3.up);
        Quaternion nuevaRotacion = Quaternion.Slerp(rb.rotation, rotacionObjetivo, velocidadRotacion * Time.fixedDeltaTime);
        rb.MoveRotation(nuevaRotacion);
    }
}