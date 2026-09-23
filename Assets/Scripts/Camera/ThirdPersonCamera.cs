using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    
    [SerializeField] private Transform jugador;
    [SerializeField] private LayerMask capasObstaculo;

   
    [SerializeField] private float distancia = 2.5f; 
    [SerializeField] private float altura = 1.4f;     
    [SerializeField] private float offsetHombro = 0.5f;

    
    [SerializeField] private float radioColision = 0.2f;

    
    [SerializeField] private float suavizadoPosicion = 0.08f;
    [SerializeField] private float velocidadRotacion = 12f;

    private Vector3 velocidadActual;
    private Quaternion rotacionCamara;

    private void Start()
    {
        rotacionCamara = jugador.rotation;
    }

    private void LateUpdate()
    {
        if (jugador == null) return;

        rotacionCamara = Quaternion.Slerp(rotacionCamara, jugador.rotation, velocidadRotacion * Time.deltaTime);

        Vector3 puntoPivote = jugador.position + Vector3.up * altura;
        Vector3 offsetLocal = new Vector3(offsetHombro, 0f, -distancia);
        Vector3 posicionDeseada = puntoPivote + rotacionCamara * offsetLocal;

        
        Vector3 direccionCamara = posicionDeseada - puntoPivote;
        float distanciaDeseada = direccionCamara.magnitude;

        if (Physics.SphereCast(puntoPivote, radioColision, direccionCamara.normalized, out RaycastHit hit, distanciaDeseada, capasObstaculo))
        {
            posicionDeseada = puntoPivote + direccionCamara.normalized * (hit.distance - 0.1f);
        }

        transform.position = Vector3.SmoothDamp(transform.position, posicionDeseada, ref velocidadActual, suavizadoPosicion);
        transform.rotation = rotacionCamara;
    }
}